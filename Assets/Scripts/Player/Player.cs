using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GChess
{
    public Vector2 move;
    public float runSpeed;
    [Header("方向指示物")]
    public Transform Direction;

    private PlayerInput playerInput;
    private Vector3 velocity;

    private float conveyYOffset=0.5f;
    private Projectile conveyObject;

    protected override void Awake()
    {
        base.Awake();
        playerInput = this.GetComponent<PlayerInput>();
        if (Direction == null)
            Direction = Camera.main.transform;
    }
    private void FixedUpdate()
    {
        if (playerInput.Dis > 0.02f)
        {
            //旋转
            Vector3 targetForward = Vector3.Slerp(this.transform.forward,
                playerInput.Dright * Direction.right + playerInput.Dup * Direction.forward, 0.2f);
            this.transform.forward = targetForward;
            //位移
            velocity = this.transform.forward * runSpeed;
            this.transform.localPosition += velocity * Time.deltaTime;

            //location更新
            location = GridManager.instance.Vector3ToVector2Int(this.transform.position); 
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(playerInput.KeyPickUp))
        {
            GResource target = GridManager.instance.GetResources(location);
            if (target != null)
                PickUp(target.projectile);
        }
        else if(Input.GetKeyDown(playerInput.KeyPutDown))
        {
            if(conveyObject)
                Putdown();
        }
    }
    void PickUp(ProjectileType projectileType)
    {
        if (conveyObject)
            Destroy(conveyObject);
        conveyObject = Instantiate(PrefabManager.instance.GetProjectilePrefab(projectileType),new Vector3(0,10,0),Quaternion.identity,transform).GetComponent<Projectile>();
        if(conveyObject.GetComponent<Rigidbody>())
        {
            Destroy(conveyObject.GetComponent<Rigidbody>());
        }
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
