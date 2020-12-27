using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GBuildingBase : GTower, IReceiveable
{
    protected GResource ownResource;
    [NonSerialized]
    private int m_currentResourceCount = 1;
    [SerializeField]
    private MeshRenderer[] m_renderer;
    public int currentResourceCount
    {
        get { return m_currentResourceCount; }
        set
        {
            m_currentResourceCount = value;
            RefreshMesh();
            RefreshText();
        }
    }
    //protected ResourceType ownResource.needMaterialType; 
    static int normalShooterTowerMaterialCount = 2;
    public int needResourceCount
    {
        get
        {
            if (ownResource == null)
                return normalShooterTowerMaterialCount;
            else
                return ownResource.needMaterialCount;
        }
    }
    bool isComplete = false;
    TextMesh text;
    protected override void Awake()
    {
        base.Awake();
        text = GetComponentInChildren<TextMesh>();
    }
    protected void Start()
    {

        ownResource = GridManager.instance.GetResources(location);
        if (ownResource)
            ownResource.GetComponentInChildren<Renderer>().enabled = false;
        RefreshMesh();
        RefreshText();
        Instantiate(PrefabManager.instance.buildingParticle, transform.position + PrefabManager.instance.buildingParticle.transform.position, PrefabManager.instance.buildingParticle.transform.rotation);
    }
    public void Receive(Projectile projectile)
    {

        if (ownResource == null || projectile.type == ownResource.needMaterialType)
        {
            currentResourceCount++;
            Instantiate(PrefabManager.instance.buildingParticle, transform.position+ PrefabManager.instance.buildingParticle.transform.position, PrefabManager.instance.buildingParticle.transform.rotation);
        }
        Destroy(projectile.gameObject);
        if (currentResourceCount >= needResourceCount)
            Complete();
        RefreshText();
    }
    void RefreshMesh()
    {
        for (int i = 0; i < m_renderer.Length; i++)
        {
            if (m_renderer[i] != null)
            {
                m_renderer[i].enabled = currentResourceCount >= i;
            }
        }
    }
    void RefreshText()
    {
        text.text = $"{currentResourceCount}/{needResourceCount}";
    }
    [ContextMenu("Complete")]
    //所有资源收集完成，转换成对应的建筑
    public void Complete()
    {
        if (isComplete)
            return;
        isComplete = true;
        Debug.Log("Complete");
        Destroy(gameObject);
        GridManager.instance.InstansiateChessAt(PrefabManager.instance.GetTowerPrefab(ownResource ? ownResource.type : ResourceType.None), location);
    }
    public override void BePickUp(Player player)
    {
        base.BePickUp(player);
        currentResourceCount--;
        if (currentResourceCount == 0)
        {
            if (ownResource)
                ownResource.GetComponentInChildren<Renderer>().enabled = true;
            Destroy(gameObject);
        }
        player.PickUp(ProjectileType.RawMaterial,_color);
    }
}
