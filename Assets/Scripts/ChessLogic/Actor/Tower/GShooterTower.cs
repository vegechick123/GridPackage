using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 发射塔
/// </summary>
public class GShooterTower : GTower,IReceiveable
{
    //发射塔的射击方向
    public Direction direction;
    //所占有的资源
    public ResourceType ownResourse { get; protected set; }

    /*-------------自动采集发射相关-------------*/
    //采集间隔时间
    [SerializeField]
    private float gatherDeltaTime=3f;
    //采集完成的事件
    [HideInInspector]
    public UnityEvent onGatherComplete;    
    private float currentTime = 0f;
    /*-----------------------------------------*/
    /// <summary>
    /// 朝着对应方向发射反应后的炮弹后销毁原炮弹
    /// </summary>
    /// <param name="projectile"></param>
    public void Receive(Projectile projectile)
    {
        Shoot(projectile.Reaction(ownResourse));
        Destroy(projectile.gameObject);
    }
    /// <summary>
    /// 朝着对应方向发射炮弹
    /// </summary>
    /// <param name="projectileType"></param>
    protected void Shoot(ProjectileType projectileType)
    {
        Debug.Log(gameObject+":Shoot");
        GameObject origin=PrefabManager.instance.GetProjectilePrefab(projectileType);
        GameObject result = Instantiate(origin);
    }
    protected override void Awake()
    {
        base.Awake();
        ownResourse = GridManager.instance.GetResourcesType(location);
        if(ownResourse==ResourceType.RawMaterial)
            onGatherComplete.AddListener(()=>Shoot(ProjectileType.RawMaterial));
    }
    void Update()
    {

        currentTime += Time.deltaTime;
        if(currentTime>gatherDeltaTime)
        {
            currentTime = gatherDeltaTime;
            onGatherComplete.Invoke();
        }
    }
    public override void OnValidate()
    {
        base.OnValidate();
        if(GridManager.instance)
            FaceToward(direction.ToVector2());
    }

}
