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
    NormalBullet,//普通炮弹
    BlueBullet,
    RedBullet
}
public class Projectile : MonoBehaviour
{
    [SerializeField]
    private Color color;
    public Color Color
    {
        get => color;
        set
        {
            color = value;
            var mat = this.GetComponentInChildren<MeshRenderer>().material;
            mat.SetColor("_Color",color);
            mat.SetColor("_BaseColor", color);
        }
    }
    public ProjectileType type;
    public IReceiveable receiveTarget;
    /*------------------炮弹伤害----------------------------*/
    public int damage;

    /*-----------------炮击相关参数-------------------*/
    public const float g = 9.8f;

    public GameObject targetGameObject;
    Vector3 targetV3;
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
        if (targetGameObject.GetComponent<GFloor>())
            targetV3 = targetGameObject.transform.position + new Vector3(0, 1f, 0);
        else
            targetV3 = targetGameObject.transform.position;

        float tmepDistance = Vector3.Distance(transform.position, targetV3);
        float tempTime = tmepDistance / Speed;
        float riseTime, downTime;
        riseTime = downTime = tempTime / 2;
        verticalSpeed = g * riseTime;
        transform.LookAt(targetV3);

        float tempTan = verticalSpeed / Speed;
        double hu = Math.Atan(tempTan);
        angle = (float)(180 / Math.PI * hu);
        transform.eulerAngles = new Vector3(-angle, transform.eulerAngles.y, transform.eulerAngles.z);
        angleSpeed = angle / riseTime;

        moveDirection = targetV3 - transform.position;
    }
    private float time;
    IEnumerator AtkUpdate()
    {
        this.GetComponentInChildren<MeshRenderer>().material.color=color;
        if (targetGameObject.GetComponent<GFloor>())
            targetV3 = targetGameObject.transform.position + new Vector3(0, 1f, 0);
        else
            targetV3 = targetGameObject.transform.position;
        while (true)
        {
            if ((transform.position.y <= targetV3.y
                && Math.Abs(transform.position.x - targetV3.x) < delta
                 && Math.Abs(transform.position.z - targetV3.z) < delta
                ) || transform.position.y < 0
                )
            {
                PaintingQuad.Create(new Vector2(transform.position.x, transform.position.z), color);
                if (targetGameObject != null)
                {
                    HitTarget();
                }
                else
                {
                    //创建颜料VFX
                   // PaintingQuad.Create(new Vector2(transform.position.x, transform.position.z), Color.white);
                    Destroy(this.gameObject);
                }
                yield break;
            }

            time += Time.deltaTime;
            float test = verticalSpeed - g * time;
            transform.Translate(moveDirection.normalized * Speed * Time.deltaTime, Space.World);
            transform.Translate(Vector3.up * test * Time.deltaTime, Space.World);
            float testAngle = -angle + angleSpeed * time;
            transform.eulerAngles = new Vector3(testAngle, transform.eulerAngles.y, transform.eulerAngles.z);
            //超出世界范围自动删除
            if (transform.position.y < -10)
                Destroy(this.gameObject);
            yield return null;
        }
    }
    public virtual void Shoot(GameObject target)
    {
        targetGameObject = target;
        receiveTarget = targetGameObject.GetComponent<IReceiveable>();
        InitBullet();
        StartCoroutine(AtkUpdate());
        //
        GameObject particle = Instantiate(PrefabManager.instance.shootingParticle, transform.position, PrefabManager.instance.shootingParticle.transform.rotation);
        var main = particle.GetComponent<ParticleSystem>().main;
        main.startColor = color;
    }
    /// <summary>
    /// 根据射击距离改变射击速度
    /// </summary>
    /// <param name="target"></param>
    /// <param name="Speed"></param>
    public virtual void Shoot(GameObject target, float Speed)
    {
        this.Speed = Speed;
        Shoot(target);

    }
    /*----------------------------------------------------------------------*/

    void HitTarget()
    {        
        if (receiveTarget != null && this!=null)
        {
            receiveTarget.Receive(this);
        }
        Destroy(gameObject);
    }
    public void Broken()
    {
        AudioSource.PlayClipAtPoint(SoundManager.instance.GetRandomPigmentSound(), transform.position);
        PaintingQuad.Create(new Vector2(transform.position.x, transform.position.z), Color, type != ProjectileType.RawMaterial);
        Destroy(gameObject);
    }    
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Ground")
    //    {
    //        HitTarget();
    //    }
    //}
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
                    case ResourceType.BlueFruit:
                        return ProjectileType.BlueBullet;
                    case ResourceType.RedFruit:
                        return ProjectileType.RedBullet;
                    default:
                        return type;
                }
            default: return type;
        }
    }



}
