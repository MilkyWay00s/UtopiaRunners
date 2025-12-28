using UnityEngine;

public class MissilePodSkill : MonoBehaviour
{
    public GameObject missileSkill;
    private MissileCount missileCount;

    void Start()
    {
        // 씬에 있는 MissileCount를 자동으로 찾아서 참조
        missileCount = FindObjectOfType<MissileCount>();

        if (missileCount != null && missileSkill != null)
        {
            SpawnMissiles();
        }
        else
        {
            Debug.LogWarning("MissileCount 또는 missileSkill이 설정되지 않음!");
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

        missileCount.missile = 0; // 스택 초기화
        Destroy(gameObject);       // 자기 자신 삭제
    }
}
