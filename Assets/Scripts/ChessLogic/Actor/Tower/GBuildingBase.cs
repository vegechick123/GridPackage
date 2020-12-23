using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GBuildingBase : GTower,IReceiveable
{
    protected GResource ownResource;

    protected int currentResourceCount;

    protected void Start()
    {
        ownResource = GridManager.instance.GetResources(location);
    }
    public void Receive(Projectile projectile)
    {
        if (projectile.type == ownResource.needMaterialType)
            currentResourceCount++;
        Destroy(projectile.gameObject);
        if (currentResourceCount >= ownResource.needMaterialCount)
            Complete();
    }
    [ContextMenu("Complete")]
    //所有资源收集完成，转换成对应的建筑
    public void Complete()
    {
        Destroy(gameObject);
        GridManager.instance.InstansiateChessAt(PrefabManager.instance.GetTowerPrefab(ownResource.type),location);
    }
}
