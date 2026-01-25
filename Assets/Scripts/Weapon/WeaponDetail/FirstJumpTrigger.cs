using UnityEngine;

public class FirstJumpTrigger : MonoBehaviour
{
    [Header("Wiring")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private ElectricOrbBehaviour chainWeapon;

    //캐릭터 오브젝트에 붙이기
    //chainWeapon에 ElectricOrb 오브젝트 넣어야함

    void Awake()
    {
        if (!playerController)
            playerController = GetComponent<PlayerController>();
    }

    void OnEnable()
    {
        if (playerController != null)
            playerController.OnFirstJump += HandleFirstJump;
    }

    void OnDisable()
    {
        if (playerController != null)
            playerController.OnFirstJump -= HandleFirstJump;
    }

    void HandleFirstJump()
    {
        if (chainWeapon == null) return;
        chainWeapon.EnableChainOnce();
    }
}
