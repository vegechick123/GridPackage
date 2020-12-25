using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 管理各种prefab的Manager
/// </summary>
/// 
public class PrefabManager : Manager<PrefabManager>
{
    [Serializable]
    struct ProjectileInfo
    {
        [ReadOnly]
        public ProjectileType type;
        public GameObject prefab;
    }
    [Serializable]
    struct ResourcesInfo
    {
        [ReadOnly]
        public ResourceType type;
        public GameObject prefab;
    }
    [Serializable]
    struct EnemyInfo
    {
        [ReadOnly]
        public int id;
        public GameObject prefab;
    }

    [SerializeField]
    private ProjectileInfo[] projectilePrefab;

    [SerializeField]
    private ResourcesInfo[] towerPrefab;

    [SerializeField]
    private EnemyInfo[] enemyInfo;

    [SerializeField]
    private GameObject buildingBasePrefab;
    /// <summary>
    /// 获得对应ProjectileType的prefab
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public GameObject GetProjectilePrefab(ProjectileType type)
    {
        return projectilePrefab[(int)type].prefab;
    }
    /// <summary>
    /// 获得对应ProjectileType的prefab
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public GameObject GetTowerPrefab(ResourceType type)
    {
        return towerPrefab[(int)type].prefab;
    }
    public GameObject GetBuildingBasePrefab()
    {
        return buildingBasePrefab;
    }

    public GameObject GetEnemyPrefabById(int id)
    {
        if (id <= 0 || id + 1 > enemyInfo.Length)
            throw new Exception("Out of Length!");
        return enemyInfo[id - 1].prefab;
    }

    /// <summary>
    /// 确保编辑器面板的预制体数量正确
    /// </summary>
    private void OnValidate()
    {
        Array values = Enum.GetValues(typeof(ProjectileType));
        Array.Resize<ProjectileInfo>(ref projectilePrefab, values.Length);
        for(int i=0;i<projectilePrefab.Length;i++)
        {
            projectilePrefab[i].type = (ProjectileType)values.GetValue(i);
        }

        values = Enum.GetValues(typeof(ResourceType));
        Array.Resize<ResourcesInfo>(ref towerPrefab, values.Length);
        for (int i = 0; i < towerPrefab.Length; i++)
        {
            towerPrefab[i].type = (ResourceType)values.GetValue(i);
        }
        for(int i=0;i< enemyInfo.Length;i++)
        {
            enemyInfo[i].id = i + 1;
        }
    }
}
