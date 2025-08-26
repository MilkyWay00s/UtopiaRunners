using UnityEngine;

public interface IWeaponContext {
    Transform Muzzle { get; }
    float Damage { get; set; }
    float FireRate { get; set; }
    void SpawnProjectile(GameObject prefab);
    event Action OnShotFired;
}

public interface IWeapon {
    IWeaponContext Context { get; }
    void Tick(float dt);
    bool TryFire();
}

public interface IWeaponModifier {
    void OnAttach(IWeapon weapon);
    void OnDetach();
    void OnBeforeFire(ref bool canFire);
    void OnAfterFire();
    void OnComputeDamage(ref float dmg);
    void Tick(float dt);
}

public interface IAbility {
    void OnEquip(CharacterAgent agent);
    bool TryActivate();
    void Tick(float dt);
    void OnUnequip();
    bool IsActive { get; }
    float CooldownLeft { get; }
}

// ===========================
// BasicGun.cs
// ===========================
using UnityEngine;

public class BasicGun : IWeapon {
    public IWeaponContext Context { get; private set; }
    private float _cooldown;

    public BasicGun(IWeaponContext ctx) { Context = ctx; }

    public void Tick(float dt) {
        if (_cooldown > 0) _cooldown -= dt;
    }

    public bool TryFire() {
        if (_cooldown > 0) return false;
        bool canFire = true;
        (this as IModHost)?.NotifyBeforeFire(ref canFire);
        if (!canFire) return false;

        Context.SpawnProjectile(null);
        Context.OnShotFired?.Invoke();

        (this as IModHost)?.NotifyAfterFire();

        _cooldown = 1f / Context.FireRate;
        return true;
    }
}

public interface IModHost {
    void NotifyBeforeFire(ref bool canFire);
    void NotifyAfterFire();
    void NotifyComputeDamage(ref float dmg);
}

// ===========================
// WeaponWithModifiers.cs
// ===========================
using System.Collections.Generic;

public class WeaponWithModifiers : IWeapon, IModHost {
    private readonly IWeapon _core;
    private readonly List<IWeaponModifier> _mods = new();
    public IWeaponContext Context => _core.Context;

    public WeaponWithModifiers(IWeapon core, IEnumerable<IWeaponModifier> mods) {
        _core = core;
        foreach (var m in mods) { _mods.Add(m); m.OnAttach(this); }
    }

    public bool TryFire() => _core.TryFire();

    public void Tick(float dt) {
        _core.Tick(dt);
        foreach (var m in _mods) m.Tick(dt);
    }

    public void NotifyBeforeFire(ref bool canFire) {
        foreach (var m in _mods) m.OnBeforeFire(ref canFire);
    }

    public void NotifyAfterFire() {
        foreach (var m in _mods) m.OnAfterFire();
    }

    public void NotifyComputeDamage(ref float dmg) {
        foreach (var m in _mods) m.OnComputeDamage(ref dmg);
    }

    public void DetachAll() {
        foreach (var m in _mods) m.OnDetach();
        _mods.Clear();
    }
}

// ===========================
// Example Abilities
// ===========================

// FireRateBoostAbility.cs
public class FireRateBoostAbility : IAbility {
    private readonly CharacterAgent _agent;
    private readonly float _mult;
    private readonly float _duration;
    private readonly float _cooldown;
    private float _left, _cd;
    public bool IsActive => _left > 0f;
    public float CooldownLeft => _cd;

    public FireRateBoostAbility(CharacterAgent agent, float mult, float duration, float cooldown) {
        _agent = agent; _mult = mult; _duration = duration; _cooldown = cooldown;
    }

    public void OnEquip(CharacterAgent agent) { }

    public bool TryActivate() {
        if (IsActive || _cd > 0f) return false;
        _left = _duration;
        _agent.Weapon.Context.FireRate *= _mult;
        return true;
    }

    public void Tick(float dt) {
        if (_cd > 0) _cd -= dt;
        if (!IsActive) return;
        _left -= dt;
        if (_left <= 0f) {
            _agent.Weapon.Context.FireRate /= _mult;
            _cd = _cooldown;
        }
    }

    public void OnUnequip() {
        if (IsActive) _agent.Weapon.Context.FireRate /= _mult;
        _left = 0f;
    }
}

// ExtraJumpAbility.cs
public class ExtraJumpAbility : IAbility {
    private CharacterAgent _agent;
    private int _extraJumps;
    public bool IsActive => true;
    public float CooldownLeft => 0f;

    public ExtraJumpAbility(CharacterAgent agent, int extraJumps) {
        _agent = agent; _extraJumps = extraJumps;
    }

    public void OnEquip(CharacterAgent agent) { _agent.Movement.SetExtraJumps(_extraJumps); }
    public bool TryActivate() => false;
    public void Tick(float dt) { }
    public void OnUnequip() { _agent.Movement.SetExtraJumps(0); }
}

// ===========================
// CharacterAgent.cs
// ===========================
using System.Collections.Generic;
using UnityEngine;

public class CharacterAgent : MonoBehaviour, IWeaponContext {
    [SerializeField] Transform muzzle;
    public Transform Muzzle => muzzle;

    public float Damage { get; set; }
    public float FireRate { get; set; }

    public MovementController Movement { get; private set; }
    public IWeapon Weapon { get; private set; }
    List<IAbility> _abilities = new();

    public event System.Action OnShotFired;

    void Awake() { Movement = GetComponent<MovementController>(); }

    public void Init(CharacterSpec spec, WeaponSpec weaponSpec) {
        Damage = weaponSpec.baseDamage;
        FireRate = weaponSpec.fireRate;
        Weapon = new BasicGun(this);
        foreach (var a in spec.abilities) _abilities.Add(AbilityFactory.Create(a, this));
        foreach (var ab in _abilities) ab.OnEquip(this);
    }

    public void Tick(float dt) {
        Weapon.Tick(dt);
        foreach (var ab in _abilities) ab.Tick(dt);
    }

    public void SpawnProjectile(GameObject prefab) {
        float dmg = Damage;
        var go = Instantiate(prefab, Muzzle.position, Muzzle.rotation);
        go.GetComponent<Projectile>().Init(dmg);
    }
}

// ===========================
// MovementController.cs
// ===========================
using UnityEngine;

public class MovementController : MonoBehaviour {
    [SerializeField] Rigidbody2D rb;
    int _maxExtraJumps, _remExtraJumps;

    public void SetExtraJumps(int n) { _maxExtraJumps = n; _remExtraJumps = n; }
    public void OnLanded() { _remExtraJumps = _maxExtraJumps; }

    public bool TryJump(float force) {
        if (IsGrounded() || _remExtraJumps > 0) {
            if (!IsGrounded()) _remExtraJumps--;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
            return true;
        }
        return false;
    }

    bool IsGrounded() { return true; }
}

// ===========================
// RunGameManager.cs
// ===========================
using UnityEngine;

public class RunGameManager : MonoBehaviour {
    [SerializeField] CharacterSpec selectedCharA, selectedCharB;
    [SerializeField] WeaponSpec selectedWeapon;
    [SerializeField] CharacterAgent agentPrefab;

    TagSystem _tag;
    HealthSystem _health;

    void Start() {
        var a = Instantiate(agentPrefab);
        var b = Instantiate(agentPrefab);

        a.Init(selectedCharA, selectedWeapon);
        b.Init(selectedCharB, selectedWeapon);

        _tag = gameObject.AddComponent<TagSystem>();
        _tag.Setup(a, b);

        _health = gameObject.AddComponent<HealthSystem>();
        _health.OnDeath += OnGameOver;
    }

    void Update() {
        float dt = Time.deltaTime;
        _tag.Active.Tick(dt);
        _health.Tick(dt);

        if (Input.GetKeyDown(KeyCode.Tab)) _tag.Swap();
        if (Input.GetKey(KeyCode.Space)) _tag.Active.Movement.TryJump(8f);
        if (Input.GetMouseButtonDown(0)) _tag.Active.Weapon.TryFire();
    }

    void OnGameOver() { Debug.Log("Game Over"); }
}

// ===========================
// TagSystem.cs
// ===========================
using UnityEngine;

public class TagSystem : MonoBehaviour {
    public CharacterAgent Active { get; private set; }
    public CharacterAgent Bench { get; private set; }

    public void Setup(CharacterAgent a, CharacterAgent b) { Active = a; Bench = b; }

    public void Swap() { (Active, Bench) = (Bench, Active); }
}

// ===========================
// HealthSystem.cs
// ===========================
using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour {
    public float maxHP = 100f;
    public float hpDrainPerSecond = 2f;
    public float Current { get; private set; }
    public event Action OnDeath;

    void Awake() { Current = maxHP; }
    public void Tick(float dt) {
        Current -= hpDrainPerSecond * dt;
        if (Current <= 0) { Current = 0; OnDeath?.Invoke(); }
    }
    public void Kill() { Current = 0; OnDeath?.Invoke(); }
}

// ===========================
// Projectile.cs
// ===========================
using UnityEngine;

public class Projectile : MonoBehaviour {
    private float _damage;
    public void Init(float dmg) { _damage = dmg; }
    void OnTriggerEnter2D(Collider2D col) { Destroy(gameObject); }
}

// ===========================
// AbilityFactory.cs
// ===========================
public static class AbilityFactory {
    public static IAbility Create(AbilitySpec spec, CharacterAgent agent) {
        switch (spec.abilityType) {
            case "FireRateBoost": return new FireRateBoostAbility(agent, spec.f1, spec.duration, spec.cooldown);
            case "ExtraJump": return new ExtraJumpAbility(agent, (int)spec.f1);
            default: return null;
        }
    }
}

// ===========================
// ModifierSpec.cs (stub)
// ===========================
using UnityEngine;

[CreateAssetMenu(menuName = "Runner/ModifierSpec")]
public class ModifierSpec : ScriptableObject {
    public string modifierType;
    public float f1, f2, f3;
}

public static class ModifierFactory {
    public static IWeaponModifier Create(ModifierSpec spec) {
        // TODO: implement specific modifiers
        return null;
    }
}
