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

    private float conveyYOffset=0.5f;
    private Projectile conveyObject;
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
    private void Update()
    {
        if (playerInput.Dis > 0.02f)
        {

            //旋转
            Vector3 targetForward = Vector3.Slerp(this.transform.forward,
                playerInput.Dright * DirTran.right + playerInput.Dup * DirTran.forward, 0.4f);
            this.transform.forward = targetForward;
            //位移
            velocity = this.transform.forward * runSpeed;
            this.transform.localPosition += velocity * Time.deltaTime;

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
            GResource target=GridManager.instance.GetResources(GetSelectLocation());
            if (target != null)
            {
                ProjectileType projectile;
                if (!conveyObject&&target.canGather)
                {
                    projectile= target.projectile;
                    PickUp(projectile);
                }
                else if(conveyObject)
                {
                    projectile = conveyObject.Reaction(target.type);
                    PickUp(projectile);
                }
                
            }
        }
        if(Input.GetKeyDown(playerInput.KeyPutDown))
        {
            if (conveyObject!=null)
                Putdown();
        }
        if (Input.GetKeyDown(playerInput.KeyRotation))
        {
            Vector2Int select = GetSelectLocation();
            Debug.Log( select );
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
    void PickUp(ProjectileType projectileType)
    {
        if (conveyObject)
        {
            Destroy(conveyObject.gameObject);
        }
        conveyObject = Instantiate(PrefabManager.instance.GetProjectilePrefab(projectileType),transform.position+new Vector3(0,conveyYOffset,0),Quaternion.identity,transform).GetComponent<Projectile>();
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
        if(tower!=null)
        {
            IReceiveable target = tower as IReceiveable;
            if(target!=null)
                target.Receive(conveyObject);
        }
        else
        {
            if(conveyObject.type==ProjectileType.RawMaterial)
            {
                Build(GetSelectLocation());
                Destroy(conveyObject.gameObject);
            }
        }
    }
    void Build(Vector2Int targetLocation)
    {
        GridManager.instance.InstansiateChessAt(PrefabManager.instance.GetBuildingBasePrefab(),targetLocation);
    }
}
