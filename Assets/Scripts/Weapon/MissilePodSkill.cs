using UnityEngine;

public class MissilePodSkill : MonoBehaviour
{
    public GameObject missileSkill;
    private MissileCount missileCount;

    void Start()
    {
        // ���� �ִ� MissileCount�� �ڵ����� ã�Ƽ� ����
        missileCount = FindObjectOfType<MissileCount>();

        if (missileCount != null && missileSkill != null)
        {
            SpawnMissiles();
        }
        else
        {
            Debug.LogWarning("MissileCount �Ǵ� missileSkill�� �������� ����!");
        }
    }

    void SpawnMissiles()
    {
        for (int i = 0; i < missileCount.missile; i++)
        {
            float offsetX = Random.Range(-2f, 2f);
            float offsetY = Random.Range(-2f, 2f);
            Vector3 spawnPos = transform.position + new Vector3(offsetX, offsetY, 0f);

            Instantiate(missileSkill, spawnPos, Quaternion.identity);
        }

        missileCount.missile = 0; // ���� �ʱ�ȭ
        Destroy(gameObject);       // �ڱ� �ڽ� ����
    }
}
