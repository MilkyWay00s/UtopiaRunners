using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [Header("플레이어 스탯")]
    public float attackPower = 1f;   
    public float armorPower = 1f;      
    public float maxHealth = 200f;    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpgradeAttack(float amount)
    {
        attackPower += amount;
        Debug.Log($"공격력이 {attackPower}로 증가했습니다.");
    }

    public void UpgradeArmor(float amount)
    {
        armorPower += amount;
        Debug.Log($"방어력이 {armorPower}로 증가했습니다.");
    }

    public void UpgradeHealth(float amount)
    {
        maxHealth += amount;
        Debug.Log($"최대체력이 {maxHealth}로 증가했습니다.");
    }
}
