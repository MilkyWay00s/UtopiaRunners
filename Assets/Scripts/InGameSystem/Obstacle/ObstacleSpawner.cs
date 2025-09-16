using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public float spawnInterval;

    private float timer = 0f;

    private ObjPoolManager poolManager;
    private void Awake()
    {
        poolManager = GetComponent<ObjPoolManager>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnObstacle();
            timer = 0f;
        }
    }

    void SpawnObstacle()
    {
        int index = Random.Range(0, obstaclePrefabs.Length);
        Vector3 spawnPos = transform.position;
        poolManager.GetObject(index, spawnPos, Quaternion.identity);
    }
}