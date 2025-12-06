using System.Collections;
using UnityEngine;

public class BossController2 : MonoBehaviour
{
    [Header("Boss Patterns")]
    public BossPatternData[] patterns;

    private bool isUsingPattern = false;

    void Update()
    {
        if (!isUsingPattern)
        {
            StartCoroutine(UseRandomPattern());
        }
    }

    IEnumerator UseRandomPattern()
    {
        isUsingPattern = true;

        int index = Random.Range(0, patterns.Length);
        BossPatternData selectedPattern = patterns[index];

        Debug.Log("Boss used pattern : " + selectedPattern.patternName);

        yield return StartCoroutine(ExecutePattern(selectedPattern));

        yield return new WaitForSeconds(selectedPattern.cooldown);

        isUsingPattern = false;
    }

    IEnumerator ExecutePattern(BossPatternData pattern)
    {
        foreach (var info in pattern.spawnInfos)
        {
            Vector3 spawnPos = transform.TransformPoint(info.offset);

            Quaternion rotation = Quaternion.Euler(0f, 0f, info.rotationZ);

            Instantiate(info.prefab, spawnPos, rotation);

            yield return new WaitForSeconds(info.delay);
        }
    }
}
