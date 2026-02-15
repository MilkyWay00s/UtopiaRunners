using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyrosController : BossController2
{
    protected override IEnumerator ExecutePattern(BossPatternData pattern)
    {
        if (pattern.patternName == "FeatherMissile")
        {
            yield return StartCoroutine(ExecuteFeatherMissilePattern(pattern));
        }
        else
        {
            yield return StartCoroutine(base.ExecutePattern(pattern));
        }
    }

    IEnumerator ExecuteFeatherMissilePattern(BossPatternData pattern)
    {
        float feathersWaitingTime = 0.5f;
        List<Feather> spawnedFeathers = new List<Feather>();

        foreach (var info in pattern.spawnInfos)
        {
            Vector3 spawnPos = transform.TransformPoint(info.offset);
            GameObject go = Instantiate(info.prefab, spawnPos, info.prefab.transform.rotation);

            Feather feather = go.GetComponent<Feather>();
            if (feather != null) spawnedFeathers.Add(feather);

            yield return new WaitForSeconds(info.delay);
        }

        yield return new WaitForSeconds(feathersWaitingTime);

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        foreach (var f in spawnedFeathers)
        {
            if (f != null)
            {
                f.Launch(player.transform.position);
            }
        }
    }
}