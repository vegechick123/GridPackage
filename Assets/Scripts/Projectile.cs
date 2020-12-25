using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 包括玩家能举起的物品以及发射塔能发射的物品
/// </summary>
public enum ProjectileType
{
    RawMaterial,//原材料
    BuildingMaterial,//建筑材料
    NormalBullet//普通炮弹
}
public class Projectile : MonoBehaviour
{
    public ProjectileType type;
    public IReceiveable receiveTarget;

    /*-----------------炮击相关参数-------------------*/
    public const float g = 9.8f;

    public GameObject targetGameObject;
    [SerializeField]
    private float speed = 10;
    public float Speed
    {
        get => speed;
        set
        {
            if (value <= 2f)
                speed = 2f;
            else
                speed = value;
        }
    }
    /// <summary>
    /// 攻击到达时的距离误差
    /// </summary>
    public float delta = 0.5f;

    private float verticalSpeed;
    private Vector3 moveDirection;

    private float angleSpeed;
    private float angle;

    /*-------------------------------------------------*/
    /*---------------------------------炮击的核心-----------------------------------*/
    void InitBullet()
    {
        float tmepDistance = Vector3.Distance(transform.position, targetGameObject.transform.position);
        float tempTime = tmepDistance / Speed;
        float riseTime, downTime;
        riseTime = downTime = tempTime / 2;
        verticalSpeed = g * riseTime;
        transform.LookAt(targetGameObject.transform.position);

        float tempTan = verticalSpeed / Speed;
        double hu = Math.Atan(tempTan);
        angle = (float)(180 / Math.PI * hu);
        transform.eulerAngles = new Vector3(-angle, transform.eulerAngles.y, transform.eulerAngles.z);
        angleSpeed = angle / riseTime;

        moveDirection = targetGameObject.transform.position - transform.position;
    }
    private float time;
    IEnumerator AtkUpdate()
    {
        while (true)
        {
            if ((transform.position.y <= targetGameObject.transform.position.y 
                && Math.Abs(transform.position.x - targetGameObject.transform.position.x)< delta
                 && Math.Abs(transform.position.z - targetGameObject.transform.position.z) < delta
                )|| transform.position.y < 0
                )
            {
                //finish
                HitTarget();
                yield break;
            }
            time += Time.deltaTime;
            float test = verticalSpeed - g * time;
            transform.Translate(moveDirection.normalized * Speed * Time.deltaTime, Space.World);
            transform.Translate(Vector3.up * test * Time.deltaTime, Space.World);
            float testAngle = -angle + angleSpeed * time;
            transform.eulerAngles = new Vector3(testAngle, transform.eulerAngles.y, transform.eulerAngles.z);
            yield return null;
        }
    }
    public virtual void Shoot(GameObject target)
    {
        targetGameObject = target;
        receiveTarget = targetGameObject.GetComponent<IReceiveable>();
        InitBullet();
        StartCoroutine(AtkUpdate());
    }
    /// <summary>
    /// 根据射击距离改变射击速度
    /// </summary>
    /// <param name="target"></param>
    /// <param name="Speed"></param>
    public virtual void Shoot(GameObject target,float Speed)
    {
        this.Speed = Speed;
        Shoot(target);
        
    }
    /*----------------------------------------------------------------------*/

    void HitTarget()
    {
        if (receiveTarget != null)
        {
            receiveTarget.Receive(this);
        }
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            HitTarget();
        }
    }
    /// <summary>
    /// 炮弹经过拥有资源的发射塔时，会根据对应地的资源发生反应
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    public ProjectileType Reaction(ResourceType resource)
    {
        switch (type)
        {
            case ProjectileType.RawMaterial:
                switch (resource)
                {
                    case ResourceType.Iron:
                        return ProjectileType.NormalBullet;
                    default:
                        return type;
                }
            case ProjectileType.NormalBullet:
                switch (resource)
                {
                    default:
                        return type;
                }
            default:return type;
        }
    }



}
