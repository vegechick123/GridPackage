using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 发射塔
/// </summary>
public class GShooterTower : GTower, IReceiveable
{
    /*------------------攻击信息----------------------*/
    public int AtkRange;
    //发射塔的射击方向
    public Direction direction;
    //所占有的资源
    public float atkSpeed;
    // 辅助发射速度的时间变量
    private float t;
    /*----------------------------------------------*/
    public ResourceType ownResourse { get; protected set; }

    /*-------------自动采集发射相关-------------*/
    //采集间隔时间
    [SerializeField]
    private float gatherDeltaTime = 3f;
    //采集完成的事件
    [HideInInspector]
    public UnityEvent onGatherComplete;
    private float currentTime = 0f;


    /*-------------------攻击相关的函数---------------------*/
    /// <summary>
    /// 朝着对应方向发射反应后的炮弹后销毁原炮弹
    /// </summary>
    /// <param name="projectile"></param>
    public void Receive(Projectile projectile)
    {
        Shoot(projectile.Reaction(ownResourse), EnemySearch());
        Destroy(projectile.gameObject);
    }
    /// <summary>
    /// 朝着对应方向发射炮弹
    /// </summary>
    /// <param name="projectileType"></param>
    protected void Shoot(ProjectileType projectileType, GameObject target)
    {
        //没有敌人时自动攻击最远地方
        if (target == null)
        {
            Vector2Int farthest;
            GFloor gFloor = null;
            int x = 0;
            //检测最远的可射击格子
            while (gFloor == null)
            {
                if (AtkRange - x == 0)
                    break;
                farthest = (AtkRange - x) * direction.ToVector2() + location;
                gFloor = GridManager.instance.GetFloor(farthest);
                x++;
            }
            target = gFloor.gameObject;
            //射击速度设置
            atkSpeed = (AtkRange - x)*2;
        }else
        {
            atkSpeed = AtkRange;
        }
        currentTime = 0;
        Debug.Log(gameObject.name + ":Shoot");
        GameObject origin = PrefabManager.instance.GetProjectilePrefab(projectileType);
        GameObject bullet = Instantiate(origin);
        //射击起点设置
        bullet.transform.position = this.transform.position + new Vector3(0,1,0);

        Projectile pj = bullet.GetComponent<Projectile>();
        //射击更自然，射速跟射击距离有关
        pj.Shoot( target , atkSpeed );
    }
    /// <summary>
    /// 敌人搜索
    /// </summary>
    GameObject EnemySearch()
    {
        Debug.Log("EnemySearch");
        Vector2Int[] search = GridManager.instance.GetOneRayRange(location, direction.ToVector2(), AtkRange);
        GChess[] chesses = GridManager.instance.GetChessesInRange(search);//攻击范围的棋子
        //检测这些棋子是什么？
        foreach (var chess in chesses)
        {
            if (chess.chessType == ChessType.ememy)
            {
                return chess.gameObject;
            }
            else if (chess.chessType == ChessType.shooterTower)
            {
                return chess.gameObject;
            }
        }
        return null;
    }
    /*---------------------------------------------------------------------------*/


    protected override void Awake()
    {
        base.Awake();
    }
    void Start()
    {
        ownResourse = GridManager.instance.GetResourcesType(location);
        if (ownResourse == ResourceType.RawMaterial)
            onGatherComplete.AddListener(() =>Shoot(ProjectileType.RawMaterial,EnemySearch()));
    }
    void Update()
    {

        currentTime += Time.deltaTime;
        if (currentTime > gatherDeltaTime)
        {
            onGatherComplete.Invoke();
            
        }
        if (GridManager.instance)
            FaceToward(direction.ToVector2());
    }
    public override void OnValidate()
    {
        base.OnValidate();
        if (GridManager.instance)
            FaceToward(direction.ToVector2());
    }
    public override void BePickUp(Player player)
    {
        base.BePickUp(player);
        Destroy(gameObject);
        player.PickUp(ProjectileType.RawMaterial);
        GBuildingBase clone =GridManager.instance.InstansiateChessAt(PrefabManager.instance.GetBuildingBasePrefab(), location).GetComponent<GBuildingBase>();
        clone.currentResourceCount = clone.needResourceCount - 1;
    }
}
