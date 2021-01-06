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

    [SerializeField]
    private Transform conveyContainer;
    [HideInInspector]
    public Projectile conveyObject;
    [SerializeField]
    private GameObject highLightEdgePrefab;
    private GameObject highLightEdge;
    private Animator animator;

    protected override void Awake()
    {
        base.Awake();
        playerInput = this.GetComponent<PlayerInput>();
        if (DirTran == null)
            DirTran = Camera.main.transform;
        animator = GetComponentInChildren<Animator>();
    }
    //void FixedUpdate()
    //{    //取消弹性
    //    this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
    //}
    private void Update()
    {
        ////postion锁定
        //foreach (Transform _transform in GetAllChilds())
        //{
        //    _transform.localPosition = new Vector3(0, _transform.localPosition.y, 0);
        //}
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
                    chess.GetComponent<GShooterTower>().direction = chess.GetComponent<GShooterTower>().direction.ClockwiseNext();
                }
            }
        }
        //更新动画
        animator.SetBool("isConvey", conveyObject != null);
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
    public void PickUp(ProjectileType projectileType, _Color _color)
    {
        if (conveyObject)
        {
            Destroy(conveyObject.gameObject);
        }
        conveyObject = Instantiate(PrefabManager.instance.GetProjectilePrefab(projectileType), conveyContainer.position, Quaternion.identity, transform).GetComponent<Projectile>();

        foreach (var rd in conveyObject.GetComponentsInChildren<Rigidbody>())
        {
            Destroy(rd);
        }
        if (conveyObject.transform.Find("gems") && _color != _Color.orgin)
            conveyObject.transform.Find("gems").GetComponent<MeshRenderer>().materials[1].
                SetColor("_BaseColor", ColorMixing.instance.GetColor(_color));
        conveyObject.Color = ColorMixing.instance.GetColor(_color);
    }
    Transform[] GetAllChilds()
    {
        Queue<Transform> transforms = new Queue<Transform>();
        for (int i = 0; i < transform.childCount; i++)
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
            switch (conveyObject.type)
            {
                case ProjectileType.RawMaterial:
                    Build(GetSelectLocation(), ColorMixing.instance.AnalysisColor(conveyObject.Color));
                    Destroy(conveyObject.gameObject);
                    break;
                case ProjectileType.BuildingMaterial:
                    break;
                case ProjectileType.NormalBullet:
                    break;
                case ProjectileType.BlueBullet:
                    break;
                case ProjectileType.RedBullet:
                    break;
                case ProjectileType.Text:
                    Transform target=GridManager.instance.InstansiateChessAt(PrefabManager.instance.textPrefab,GetSelectLocation()).GetComponent<Transform>();
                    target.transform.position = target.transform.position + new Vector3(0, 0.05f, 0);
                    target.GetComponent<TextMesh>().text = conveyObject.gameObject.GetComponent<TextMesh>().text;
                    
                    target.GetComponent<LevelNumber>().levelNumber = conveyObject.GetComponent<LevelNumber>().levelNumber;
                    target.GetComponent<LevelNumber>().targetLocation= conveyObject.GetComponent<LevelNumber>().targetLocation;
                    
                    Destroy(conveyObject.gameObject);
                    break;
            }
        }
    }
    void Build(Vector2Int targetLocation, _Color color)
    {
        GChess chess = GridManager.instance.InstansiateChessAt(PrefabManager.instance.GetBuildingBasePrefab(), targetLocation);
        chess.GetComponent<GBuildingBase>()._color = color;
    }
}