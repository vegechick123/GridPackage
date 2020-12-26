using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GChess
{
    public Vector2 move;
    public float runSpeed;
    [Header("方向指示物")]
    public Transform DirTran;
    [Header("当前面向方向")]
    public Direction direction;

    private PlayerInput playerInput;
    private Vector3 velocity;

    private float conveyYOffset = 0.5f;
    [HideInInspector]
    public Projectile conveyObject;
    [SerializeField]
    private GameObject highLightEdgePrefab;
    private GameObject highLightEdge;


    protected override void Awake()
    {
        base.Awake();
        playerInput = this.GetComponent<PlayerInput>();
        if (DirTran == null)
            DirTran = Camera.main.transform;

    }
    //void FixedUpdate()
    //{    //取消弹性
    //    this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
    //}
    private void Update()
    {
        //postion锁定
        foreach (Transform _transform in GetAllChilds())
        {
            _transform.localPosition = new Vector3(0, _transform.localPosition.y, 0);
        }
        if (playerInput.Dis > 0.02f)
        {

            //旋转
            Vector3 targetForward = Vector3.Slerp(this.transform.forward,
                playerInput.Dright * DirTran.right + playerInput.Dup * DirTran.forward, 0.4f);
            this.transform.forward = targetForward;
            //位移
            velocity = this.transform.forward * runSpeed;
            this.transform.position += velocity * Time.deltaTime;
            //location更新
            location = GridManager.instance.Vector3ToVector2Int(this.transform.position);
            //Direction朝向更新
            if (this.transform.eulerAngles.y >= 315 || this.transform.eulerAngles.y < 45)
                direction = Direction.up;
            else if (this.transform.eulerAngles.y >= 45 && this.transform.eulerAngles.y < 135)
                direction = Direction.right;
            else if (this.transform.eulerAngles.y >= 135 && this.transform.eulerAngles.y < 225)
                direction = Direction.down;
            else if (this.transform.eulerAngles.y >= 225 && this.transform.eulerAngles.y < 315)
                direction = Direction.left;


        }
        HighLightHandle();
        if (Input.GetKeyDown(playerInput.KeyPickUp))
        {
            GTower targetTower = GridManager.instance.GetTower(GetSelectLocation());
            if (targetTower != null)
            {
                targetTower.BePickUp(this);
            }
            else
            {
                GResource targetResource = GridManager.instance.GetResources(GetSelectLocation());
                if (targetResource != null)
                {
                    targetResource.BePickUp(this);
                }
            }
        }
        if (Input.GetKeyDown(playerInput.KeyPutDown))
        {
            if (conveyObject != null)
                Putdown();
        }
        if (Input.GetKeyDown(playerInput.KeyRotation))
        {
            Vector2Int select = GetSelectLocation();
            Debug.Log(select);
            GChess chess = GridManager.instance.GetTower(select);
            if (chess)
            {
                Debug.Log(chess);
                if (chess.chessType == ChessType.shooterTower)
                {
                    chess.GetComponent<GShooterTower>().direction = direction;
                }
            }
        }
    }
    //处理选中格子的高亮
    void HighLightHandle()
    {
        Vector2Int select = GetSelectLocation();
        if (!highLightEdge)
        {
            highLightEdge = Instantiate(highLightEdgePrefab);
        }
        highLightEdge.transform.position = GridManager.instance.GetFloorPosition3D(select);
    }
    public void PickUp(ProjectileType projectileType,_Color _color)
    {
        if (conveyObject)
        {
            Destroy(conveyObject.gameObject);
        }
        conveyObject = Instantiate(PrefabManager.instance.GetProjectilePrefab(projectileType), transform.position + new Vector3(0, conveyYOffset, 0), Quaternion.identity, transform).GetComponent<Projectile>();
        conveyObject.Color = ColorMixing.instance.GetColor(_color);
    }
    Transform[] GetAllChilds()
    {
        Queue<Transform> transforms = new Queue<Transform>();
        for(int i=0;i<transform.childCount;i++)
        {
            transforms.Enqueue(transform.GetChild(i));
        }
        return transforms.ToArray();
    }
    Vector2Int GetSelectLocation()
    {
        Vector2Int select = location + direction.ToVector2();
        return select;
    }
    void Putdown()
    {
        if (!conveyObject)
            return;
        GTower tower = GridManager.instance.GetTower(GetSelectLocation());
        if (tower != null)
        {
            IReceiveable target = tower as IReceiveable;
            if (target != null)
                target.Receive(conveyObject);
        }
        else
        {
            if (conveyObject.type == ProjectileType.RawMaterial)
            {
                Build(GetSelectLocation());
                Destroy(conveyObject.gameObject);
            }
        }
    }
    void Build(Vector2Int targetLocation)
    {
        GridManager.instance.InstansiateChessAt(PrefabManager.instance.GetBuildingBasePrefab(), targetLocation);
    }
}