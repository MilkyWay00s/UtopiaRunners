using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

[Serializable]
public enum WorldID
{
    None = 0,

    World1 = 1,
    World2,
    World3,
    World4
}
[CreateAssetMenu(menuName = "ScriptableObjects/WorldMapList")]
public class WorldMapList : ScriptableObject
{
    [Serializable]
    public class WorldInfo
    {
        [Header("월드 ID")]
        public WorldID worldID;
        [Header("월드 이름")]
        public string worldName;
        [Header("월드 설명")]
        public string worldDescription;
        [Header("월드 이미지")]
        public Sprite worldImage;
    }

    [Header("월드")]
    public List<WorldInfo> worlds = new List<WorldInfo>();
    public string GetAbilityName(WorldID id)
    {
        foreach (var world in worlds)
        {
            if (world.worldID == id)
            {
                return world.worldName;
            }
        }
        return string.Empty;
    }
    public string GetAbilityDescription(WorldID id)
    {
        foreach (var world in worlds)
        {
            if (world.worldID == id)
            {
                return world.worldDescription;
            }
        }
        return string.Empty;
    }
    public Sprite GetAbilityImage(WorldID id)
    {
        foreach (var world in worlds)
        {
            if (world.worldID == id)
            {
                return world.worldImage;
            }
        }
        return null;
    }
}
