using System;
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
    // 发射随机变化时间范围
    public float random;
    float Random;
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
    private Animator animator;
    //攻击间隔
    public float AtkInterval = 0.1f;
    public float atkAnimationTime = 0.4f;
    [SerializeField]
    bool canShoot = true;

    /*-------------------攻击相关的函数---------------------*/
    /// <summary>
    /// 朝着对应方向发射反应后的炮弹后销毁原炮弹
    /// </summary>
    /// <param name="projectile"></param>
    public void Receive(Projectile projectile)
    {
        StartCoroutine(AtkTimer());
        if (canShoot == true)
        {
            canShoot = false;
            this._color = ColorMixing.instance.AnalysisColor( projectile.Color);
            ReadyShoot(projectile.Reaction(ownResourse));
        }
        Destroy(projectile.gameObject);
    }
    /// <summary>
    /// 计时辅助器
    /// </summary>
    /// <returns></returns>
    IEnumerator AtkTimer()
    {
        yield return new WaitForSeconds(AtkInterval);
        canShoot = true;
    }
    void ReadyShoot(ProjectileType projectile)
    {
        animator.SetTrigger("Shoot");
        this.InvokeAfter(()=>Shoot(projectile,EnemySearch()),atkAnimationTime);
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
            atkSpeed = (AtkRange - x) * 2;
        }
        else
        {
            atkSpeed = AtkRange;
        }
        
        Debug.Log(gameObject.name + ":Shoot");
        GameObject origin = PrefabManager.instance.GetProjectilePrefab(projectileType);
        GameObject bullet = Instantiate(origin);
        //射击起点设置
        bullet.transform.position = this.transform.position + new Vector3(0, 1, 0);

        Projectile pj = bullet.GetComponent<Projectile>();
        //改变子弹颜色
        pj.Color = ColorMixing.instance.GetColor(_color);
        //射击更自然，射速跟射击距离有关
        pj.Shoot(target, atkSpeed);
        AudioSource.PlayClipAtPoint(SoundManager.instance.GetRandomShootSound(),transform.position);
    }
    /// <summary>
    /// 敌人搜索
    /// </summary>
    GameObject EnemySearch()
    {
        Vector2Int[] search = GridManager.instance.GetOneRayRange(location, direction.ToVector2(), AtkRange);

        GChess[] chesses = GridManager.instance.GetChessesInRange(search);//攻击范围的棋子
        //获得最近的 可攻击棋子
        while (true)
        {
            GChess chess = SortByDistance(chesses);
            if (chess == null)
                return null;
            //foreach (var chess in chesses)
            //{
            if (chess.chessType == ChessType.shooterTower)
            {
                return chess.gameObject;
            }
            else if (chess.chessType == ChessType.ememy)
            {
                return chess.gameObject;
            }
            //}
            //return null;
            chesses = DelOneChess(chess,chesses);
        }
    }
    /// <summary>
    /// 距离排序返回最近的
    /// </summary>
    /// <returns></returns>
    GChess SortByDistance(GChess[] chesses)
    {
        if (chesses.Length == 0)
            return null;
        int[] dis = new int[chesses.Length];
        int i = 0;
        foreach (var chess in chesses)
        {
            dis[i] = Power((location - chess.location).x) + Power((location - chess.location).y);
            i++;
        }
        int min = dis[0];
        int min_index = 0;
        for (int j = 1; j < dis.Length; j++)
        {
            if (dis[j] < min)
            {
                min = dis[j];
                min_index = j;
            }
        }
        return chesses[min_index];
    }
    GChess[] DelOneChess(GChess chess, GChess[] chesses)
    {
        List<GChess> list = new List<GChess>(chesses);
        list.Remove(chess);
        return list.ToArray();
    }
    int Power(int num)
    {
        return num * num;
    }
    /*---------------------------------------------------------------------------*/


    protected override void Awake()
    {
        base.Awake();
        Random = 0;
        animator = GetComponentInChildren<Animator>();
    }
    void Start()
    {
        ownResourse = GridManager.instance.GetResourcesType(location);
        if (ownResourse == ResourceType.RawMaterial)
        {
            onGatherComplete.AddListener(()=> { ReadyShoot(ProjectileType.RawMaterial);});
        }
    }
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > gatherDeltaTime + Random)
        {
           if( GridManager.instance.GetResources(location))
                if(GridManager.instance.GetResources(location).type==ResourceType.RawMaterial)
                this._color = GridManager.instance.GetResources(location)._color;
            onGatherComplete.Invoke();
            currentTime = 0;
            Random = UnityEngine.Random.Range(-random, random);
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
        player.PickUp(ProjectileType.RawMaterial,_color);
        GBuildingBase clone = GridManager.instance.InstansiateChessAt(PrefabManager.instance.GetBuildingBasePrefab(), location).GetComponent<GBuildingBase>();
        clone.currentResourceCount = clone.needResourceCount - 1;
    }
}
