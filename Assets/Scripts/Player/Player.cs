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

    protected override void Awake()
    {
        base.Awake();
        playerInput = this.GetComponent<PlayerInput>();
        if (DirTran == null)
            DirTran = Camera.main.transform;
    }
    private void FixedUpdate()
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

        if (Input.GetKeyDown(playerInput.KeyA))
        {
            Vector2Int select = location + direction.ToVector2();
            Debug.Log( select );
            GChess chess = GridManager.instance.GetChess(select);
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
    private void Update()
    {
        if(Input.GetKeyDown(playerInput.KeyPickUp))
        {

        }
        else if(Input.GetKeyDown(playerInput.KeyPutDown))
        {

        }
    }
    void PickUp(ProjectileType projectileType)
    {
        if (conveyObject)
            Destroy(conveyObject);
        conveyObject = Instantiate(PrefabManager.instance.GetProjectilePrefab(projectileType),new Vector3(0,0.5f,0),Quaternion.identity,transform).GetComponent<Projectile>();
    }
    void Putdown()
    {
        if (!conveyObject)
            return;
        GTower tower = GetComponent<GTower>();
        if(tower!=null)
        {
            IReceiveable target = tower as IReceiveable;
            if(target!=null)
            target.Receive(conveyObject);
        }
        else
        {
            if(conveyObject.type==ProjectileType.BuildingMaterial)
            {
                Build(location);
            }
        }
    }
    void Build(Vector2Int targetLocation)
    {
        GridManager.instance.InstansiateChessAt(PrefabManager.instance.GetBuildingBasePrefab(),targetLocation);
    }
}
