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
    public IReceiveable target;

    /*-----------------炮击相关参数-------------------*/
    public const float g = 9.8f;

    public GameObject Target;
    public float speed = 10;
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
        float tmepDistance = Vector3.Distance(transform.position, Target.transform.position);
        float tempTime = tmepDistance / speed;
        float riseTime, downTime;
        riseTime = downTime = tempTime / 2;
        verticalSpeed = g * riseTime;
        transform.LookAt(Target.transform.position);

        float tempTan = verticalSpeed / speed;
        double hu = Math.Atan(tempTan);
        angle = (float)(180 / Math.PI * hu);
        transform.eulerAngles = new Vector3(-angle, transform.eulerAngles.y, transform.eulerAngles.z);
        angleSpeed = angle / riseTime;

        moveDirection = Target.transform.position - transform.position;
    }
    private float time;
    IEnumerator AtkUpdate()
    {
        while (true)
        {
            if ((transform.position.y <= Target.transform.position.y 
                && Math.Abs(transform.position.x - Target.transform.position.x)< delta
                 && Math.Abs(transform.position.z - Target.transform.position.z) < delta
                )|| transform.position.y < 0
                )
            {
                //finish
                Destroy(this.gameObject);//todo
                yield break;
            }
            time += Time.deltaTime;
            float test = verticalSpeed - g * time;
            transform.Translate(moveDirection.normalized * speed * Time.deltaTime, Space.World);
            transform.Translate(Vector3.up * test * Time.deltaTime, Space.World);
            float testAngle = -angle + angleSpeed * time;
            transform.eulerAngles = new Vector3(testAngle, transform.eulerAngles.y, transform.eulerAngles.z);
            yield return null;
        }
    }
    public virtual void Shoot(GameObject target)
    {
        Target = target;
        InitBullet();
        StartCoroutine(AtkUpdate());
    }

    /*----------------------------------------------------------------------*/


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            if (target != null)
            {
                target.Receive(this);
            }
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
