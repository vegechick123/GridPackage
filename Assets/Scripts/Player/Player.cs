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
    void Convey(IPortable target)
    {

    }
    Vector2Int GetCurrentLocation()
    {
        return Vector2Int.zero;
    }
    void Build(GTower prefab)
    {

    }
}
