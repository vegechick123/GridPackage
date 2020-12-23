using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 发射塔
/// </summary>
public class GShooterTower : GTower,IReceiveable
{
    /*------------------攻击信息----------------------*/
    public int AtkRange;
    //发射塔的射击方向
    public Vector2Int direction;
    //所占有的资源
    public float atkSpeed;
    // 辅助发射速度的时间变量
    private float t;
    /*----------------------------------------------*/
    public ResourceType ownResourse { get; protected set; }

    /*-------------自动采集发射相关-------------*/
    //采集间隔时间
    [SerializeField]
    private float gatherDeltaTime=3f;
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
        //Shoot(projectile.Reaction(ownResourse));
        Destroy(projectile.gameObject);
    }
    /// <summary>
    /// 朝着对应方向发射炮弹
    /// </summary>
    /// <param name="projectileType"></param>
    protected void Shoot(ProjectileType projectileType,GameObject target)
    {
        GameObject origin=PrefabManager.instance.GetProjectilePrefab(projectileType);
        Projectile pj = Instantiate(origin).GetComponent<Projectile>();
        pj.Shoot(target);
    }
    /// <summary>
    /// 敌人搜索
    /// </summary>
    void EnemySearch()
    {
        Vector2Int[] search = GridManager.instance.GetOneRayRange(location, direction, AtkRange);
        GChess[] chesses= GridManager.instance.GetChessesInRange(search);//攻击范围的棋子
        //检测这些棋子是 什么？
        if (t < 0)
        {
            t = 1;
            foreach (var chess in chesses)
            {
                if (chess.chessType == ChessType.ememy)
                {
                    Shoot(ProjectileType.NormalBullet, chess.gameObject);
                } else if (chess.chessType == ChessType.shooterTower)
                {
                    Shoot(ProjectileType.NormalBullet, chess.gameObject);
                }
            }
        }
        else
        {
            t -= Time.deltaTime* atkSpeed;
        }
    }
    /*---------------------------------------------------------------------------*/


    protected override void Awake()
    {
        base.Awake();
        t = 1;
        direction = new Vector2Int(0, 1);
        if (GridManager.instance.GetResources(location)!=null)
        ownResourse = GridManager.instance.GetResources(location).type;
       // if(ownResourse==ResourceType.RawMaterial)
           // onGatherComplete.AddListener(()=>Shoot(ProjectileType.RawMaterial));
    }
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime>gatherDeltaTime)
        {
            currentTime -= gatherDeltaTime;
            onGatherComplete.Invoke();
        }

        EnemySearch();
    }
    
}
