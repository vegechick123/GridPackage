﻿using System.Collections;
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
            Vector3 targetForward = Vector3.Slerp(this.transform.forward,
                playerInput.Dright * Direction.right + playerInput.Dup * Direction.forward, 0.2f);
            this.transform.forward = targetForward;

            velocity = this.transform.forward * runSpeed;
            this.transform.localPosition += velocity * Time.deltaTime;
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
