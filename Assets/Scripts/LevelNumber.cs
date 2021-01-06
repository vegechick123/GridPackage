using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelNumber : MonoBehaviour
{
    public int levelNumber = -1;
    public Vector2Int targetLocation;
    void Start()
    {
        GActor self = GetComponent<GActor>();
        if(levelNumber>=0&&self&&self.location == targetLocation)
        {
            Instantiate(PrefabManager.instance.buildingParticle,
            transform.position + PrefabManager.instance.buildingParticle.transform.position,
            PrefabManager.instance.buildingParticle.transform.rotation);
            this.InvokeAfter(()=>LevelManager.instance.SwitchToLevel(levelNumber),1f);
        }
    }
}
