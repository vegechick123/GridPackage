using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Buff:byte
{
    normal,
    frozen,
    burn,
    toxicosis,
    foxed,
    roused
}
public enum State:int
{
    normal,
    gas,
    soild
}



public class Enemy : GChess,IReceiveable
{
    /*---------------敌人的基础属性面板-----------------*/
    [SerializeField]
    State state;
    public State State
    {
        get => state;
        set
        {
            if(value!=state)
            {
                CloseState();
                state = value;
                UpdateState();
            }
        }
    }


    public Buff buff;
    /// <summary>
    /// 移动速度
    /// </summary>
    public float speed;
    //即剩余生命值
    [SerializeField]
    private int currentHealth;
    public int CurrentHealth
    {
        get => currentHealth;
        set
        {
            if (value < 0)
                currentHealth = 0;
            else
                currentHealth = value;
        }
    }
    //生命值上限
    public int health;
    /// <summary>
    /// 预设的目的地
    /// </summary>
    [SerializeField]
    private Vector2Int destination;

    public MeshRenderer _renderer;

    public _Color _color;
    //protected IEnumerator coroutines;
    /*---------------------------------------------*/
    protected override void Awake()
    {
        base.Awake();
        //coroutines = MoveActor(destination);
    }
    private void Update()
    {

        if (CurrentHealth == 0)
            Destroy(this.gameObject);

        if (_color == _Color.blue && buff!=Buff.frozen )
        {
            buff = Buff.frozen;
            StartCoroutine(Frozen());
            speed /= 2;
        }
        if (_color != _Color.blue && buff == Buff.frozen)
        {
            StopCoroutine(Frozen());
            speed *= 2;
        }

        if (_color == _Color.green && buff != Buff.toxicosis)
        {
            buff = Buff.toxicosis;
            StartCoroutine(Toxicosis());
        }
        if (_color != _Color.green && buff == Buff.toxicosis)
            StopCoroutine(Toxicosis());

        if (_color == _Color.red && buff != Buff.burn)
        {
            StartCoroutine(Born());
            buff = Buff.burn;
            CurrentHealth -= 1;
        }
        if (_color != _Color.red && buff != Buff.burn)
            StopCoroutine(Born());

        if (_color == _Color.yellow && buff != Buff.foxed)
        {
            buff = Buff.foxed;
            StartCoroutine(Foxed());
        }
        if (_color != _Color.yellow && buff == Buff.foxed)
        {
            StopCoroutine(Foxed());
            speed = orginSpeed;
        }


        if (_color == _Color.orange && buff != Buff.roused)
        {
            CurrentHealth+=2;
            buff = Buff.roused;
        }


        if (_color == _Color.purple && buff != Buff.toxicosis)
        {
            buff = Buff.toxicosis;
            StartCoroutine(Toxicosis());
        }
        if (_color != _Color.purple && buff == Buff.toxicosis)
        {
            StopCoroutine(Toxicosis());
        }
    }
    #region Buff
    IEnumerator Frozen()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.5f);
            if (state == State.normal)
            {
                State = State.soild;
                yield break;
            }
            else if (state == State.gas)
            {
                state = State.normal;
            }
        }
    }
    IEnumerator Born()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.5f);
            CurrentHealth -= 1;
            if (state == State.normal)
            {
                state = State.gas;
                yield break;
            }
            else if (state == State.soild)
            {
                state = State.normal;
            }
        }
    }
    IEnumerator Toxicosis()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            CurrentHealth -= 1;
        }
    }
    float orginSpeed;
    IEnumerator Foxed()
    {
        orginSpeed = speed;
        speed = 0.01f;
        yield return new WaitForSeconds(0.5f);
        speed = orginSpeed;
    }
    #endregion

    #region 形态变化
    void UpdateState()
    {
        switch (state)
        {
            case State.gas: speed += 0.1f; break;
            case State.soild:
                health += health;
                currentHealth += health;
                break;
        }
        for (int i=0;i<3;i++)
        {
            if ((int)state == i)
                this.transform.GetChild(i).gameObject.SetActive(true);
            else
                this.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    void CloseState()
    {
        switch(state)
        {
            case State.gas:speed -= 0.1f; break ;
            case State.soild:
                health -= health;
                currentHealth -= health;
                break;
        }

    }
    #endregion
    /// <summary>
    /// 初始化
    /// </summary>
    public void InitEnemy(Vector2Int initPos, float speed,int health)
    {
        location = initPos;

        transform.localPosition =  GetChessPosition3D(initPos);

        this.speed = speed;
        this.health = health;
        CurrentHealth = health;
    }
    /// <summary>
    /// 初始化
    /// </summary>
    public void InitEnemy(Vector2Int initPos,Vector2Int desPos, float speed, int health)
    {
        location = initPos;
        destination = desPos;

        transform.localPosition = GetChessPosition3D(initPos);

        this.speed = speed;
        this.health = health;
        CurrentHealth = health;
    }
    /// <summary>
    /// Hack
    /// </summary>
    /// <param name="vector2"></param>
    /// <returns></returns>
    Vector3 GetChessPosition3D(Vector2Int vector2)
    {
        Vector3 output = GridManager.instance.GetChessPosition3D(vector2);
        //hack
        return new Vector3(output.x,
            output.y ,//+ 1,
            output.z);
    }
    #region 移动
    /*-------------------------------移动-----------------------------------*/
    [ContextMenu("MoveTest")]
    public void Move()
    {
        StartCoroutine(MoveActor(destination));
    }
    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="target">移动的终点</param>
    IEnumerator MoveActor(Vector2Int target)
    {
        destination = target;
        while (true)
        {
            //获得可以走动的格子
            Vector2Int[] targets = GridManager.instance.GetEnemyPath(location);
            //选择正确的方向走动，移动一格
            Vector2Int next = GoDir(targets);
            Vector3 nextPos = GetChessPosition3D(next);
            Vector3 localtionPos = GetChessPosition3D(location);
            for (float time = 0; time <= 1; time += speed * Time.deltaTime)
            {
                transform.position = Vector3.Lerp(localtionPos, nextPos, time);
                //自动转向
                transform.forward = Vector3.Slerp(transform.forward , GridManager.instance.GetDirection3D( location - next) , 0.2f);
                yield return null;
            }
            //更新现在的location,迭代进入下一次移动
            location = next;
            if (location == destination)
                yield break;
        }
    }
    /// <summary>
    /// 获得下一次移动的格子坐标
    /// </summary>
    /// <param name="targets"></param>
    /// <returns></returns>
    Vector2Int GoDir(Vector2Int[] targets)
    {
        foreach (Vector2Int vector2 in targets)
        {
            if (((vector2 - location) * (destination - location)).x > 0)
            {
                return vector2;
            }
            else if (((vector2 - location) * (destination - location)).y > 0)
            {
                return vector2;
            }
        }
        return location;
    }
    //public void Rebirth(Vector2Int initPos)
    //{
    //    StopAllCoroutines();
    //    location = initPos;

    //    transform.localPosition = GetChessPosition3D(initPos);
    //    Move();
    //}
    /*--------------------------------------------------------------------*/
    #endregion

    public void Receive(Projectile projectile)
    {
        //switch(projectile.type)
        //{
        //    case ProjectileType.NormalBullet:CurrentHealth -= 5;this.transform.Find("Blood").gameObject.SetActive(true); break;
        //    case ProjectileType.BuildingMaterial: CurrentHealth -= 2; this.transform.Find("Blood").gameObject.SetActive(true); break;
        //    case ProjectileType.RawMaterial: CurrentHealth -= 2; this.transform.Find("Blood").gameObject.SetActive(true); break;
        //}        
        CurrentHealth -= projectile.damage; this.transform.Find("Blood").gameObject.SetActive(true);
        PaintingQuad.Create(new Vector2(transform.position.x, transform.position.z), projectile.Color,projectile.type!=ProjectileType.RawMaterial);
        //自己的颜色也变化
        int index = 0;
        foreach (var mesh in GetComponentsInChildren<MeshRenderer>())
        {
            if (mesh.gameObject.name != "Blood")
                _renderer = mesh;
        }
        if (state == State.gas)
           index = 1;
        Color color = ColorMixing.instance.
            MixColor(_renderer.materials[index].GetColor("_BaseColor"), projectile.Color);
        _color = ColorMixing.instance.AnalysisColor(color);
       _renderer.materials[index].SetColor("_BaseColor",color); 

    }

}
