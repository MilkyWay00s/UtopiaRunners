using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "LaserPattern", menuName = "Boss/LaserPattern")]
public class LaserPattern : ScriptableObject, IBossPattern
{
    public string patternName;

    [Header("프리팹 설정")]
    public GameObject warningPrefab;
    public GameObject laserPrefab;

    [Header("스폰 위치")]
    public Vector2 offset;

    [Header("타이밍")]
    public float warningTime = 1f;
    public float laserDuration = 2f;

    [Header("크기 조절 (XY)")]
    public Vector2 warningScale = Vector2.one; 
    public Vector2 laserScale = Vector2.one;

    public float intervalAfterPattern => 1f;

    public IEnumerator ExecutePattern(Vector2 bossPos, MonoBehaviour executor)
    {
        yield return executor.StartCoroutine(SpawnWarning(bossPos));
        yield return executor.StartCoroutine(SpawnLaser(bossPos));
    }

    private IEnumerator SpawnWarning(Vector2 bossPos)
    {
        Vector2 spawnPos = bossPos + offset;  
        GameObject warning = Instantiate(warningPrefab, spawnPos, Quaternion.identity);

        Vector3 warningOriginal = warning.transform.localScale;
        warning.transform.localScale = new Vector3(warningScale.x, warningScale.y, warningOriginal.z);

        yield return new WaitForSeconds(warningTime);

        Destroy(warning);
    }

    private IEnumerator SpawnLaser(Vector2 bossPos)
    {
        Vector2 spawnPos = bossPos + offset;  
        GameObject laser = Instantiate(laserPrefab, spawnPos, Quaternion.identity);

        Vector3 laserOriginal = laser.transform.localScale;
        laser.transform.localScale = new Vector3(laserScale.x, laserScale.y, laserOriginal.z);

        yield return new WaitForSeconds(laserDuration);

        Destroy(laser);
    }

}
