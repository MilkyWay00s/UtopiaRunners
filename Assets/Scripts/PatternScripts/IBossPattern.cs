using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBossPattern
{
    float intervalAfterPattern { get; }

    IEnumerator ExecutePattern(Vector2 bossPos, MonoBehaviour executor);
}

