using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public ObstaclePattern[] patterns;
    public Transform spawnOrigin;
    public float interval = 3f;

    private void Start()
    {
        StartCoroutine(PatternRoutine());
    }

    private IEnumerator PatternRoutine()
    {
        while (true)
        {
            ExecuteRandomPattern();
            yield return new WaitForSeconds(interval);
        }
    }

    public void ExecutePattern(int index)
    {
        if (patterns.Length == 0 || index < 0 || index >= patterns.Length)
        {
            Debug.LogWarning("패턴이 없거나 인덱스가 잘못됨!");
            return;
        }

        ObstaclePattern pattern = patterns[index];

        foreach (var point in pattern.spawnPoints)
        {
            Vector2 spawnPos = (Vector2)spawnOrigin.position + point.offset;
            Instantiate(pattern.obstaclePrefab, spawnPos, Quaternion.identity);
        }
    }

    public void ExecuteRandomPattern()
    {
        int randIndex = Random.Range(0, patterns.Length);
        ExecutePattern(randIndex);
    }
}
