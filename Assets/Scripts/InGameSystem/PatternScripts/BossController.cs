using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public List<ScriptableObject> patternObjects;
    private List<IBossPattern> patterns = new List<IBossPattern>();

    public Transform spawnOrigin;

    private void Start()
    {
        foreach (var obj in patternObjects)
        {
            if (obj is IBossPattern pattern)
            {
                patterns.Add(pattern);
            }
            else
            {
                Debug.LogWarning($"{obj.name}은 IPattern을 구현하지 않았습니다!");
            }
        }

        StartCoroutine(PatternRoutine());
    }

    private IEnumerator PatternRoutine()
    {
        while (true)
        {
            int randIndex = Random.Range(0, patterns.Count);
            IBossPattern pattern = patterns[randIndex];

            yield return StartCoroutine(pattern.ExecutePattern(spawnOrigin.position, this));

            yield return new WaitForSeconds(pattern.intervalAfterPattern);
        }
    }
}