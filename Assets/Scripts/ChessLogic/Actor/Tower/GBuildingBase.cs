using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GBuildingBase : GTower,IReceiveable
{
    protected GResource ownResource;
    [NonSerialized]
    protected int currentResourceCount=1;
    //protected ResourceType ownResource.needMaterialType; 
    static int normalShooterTowerMaterialCount=2;
    bool isComplete =false;
    protected void Start()
    {
        ownResource = GridManager.instance.GetResources(location);
    }
    public void Receive(Projectile projectile)
    {
        
        if (ownResource == null||projectile.type == ownResource.needMaterialType)
            currentResourceCount++;
        Destroy(projectile.gameObject);
        if(ownResource==null)
        {
            if(currentResourceCount >= normalShooterTowerMaterialCount)
            {
                Complete();
            }
        }
        else if (currentResourceCount >= ownResource.needMaterialCount)
            Complete();
    }
    [ContextMenu("Complete")]
    //所有资源收集完成，转换成对应的建筑
    public void Complete()
    {
        if (isComplete)
            return;
        isComplete = true;
        Debug.Log("Complete");
        Destroy(gameObject);
        GridManager.instance.InstansiateChessAt(PrefabManager.instance.GetTowerPrefab(ownResource?ownResource.type:ResourceType.None),location);
    }
}
