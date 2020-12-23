using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 move;
    public float runSpeed;
    [Header("方向指示物")]
    public Transform Direction;

    private PlayerInput playerInput;
    private Vector3 velocity;


    private void Awake()
    {
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
