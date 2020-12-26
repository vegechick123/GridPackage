using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingQuad : MonoBehaviour
{
    /// <summary>
    /// 在指定位置创造该颜色的VFX
    /// </summary>
    /// <param name="location"></param>
    /// <param name="color"></param>
    public static void Create(Vector2 location,Color color)
    {
        Vector3 position = new Vector3(location.x,GridManager.instance.chessOffset+0.55f+Random.Range(0,0.05f), location.y);

        var main = Instantiate(PrefabManager.instance.paintingParticle, position, PrefabManager.instance.paintingParticle.transform.rotation).GetComponent<ParticleSystem>().main;

        main.startColor = color;
        PaintingQuad target=Instantiate(PrefabManager.instance.paintingQuadPrefab,position, PrefabManager.instance.paintingQuadPrefab.transform.rotation).GetComponent<PaintingQuad>();
        target.color = color;
    }
    public float beginDissolveTime=2f;
    public float endDissolveTime = 5f;
    public float currentTime = 0f;
    public float initDissolve = 0.3f;
    public Color color;
    private Material material;
    private void Start()
    {

        material = GetComponent<MeshRenderer>().material;
        material.SetColor("BaseColor",color);
        Vector2Int location=new Vector2Int(Random.Range(0, 4), Random.Range(0, 4));
        material.SetVector("PaintCenter",new Vector4(location.x, location.y, 0,0));
    }
    private void Update()
    {
        currentTime += Time.deltaTime;
        float current = Mathf.Lerp(initDissolve, 1, (currentTime - beginDissolveTime) / (endDissolveTime - beginDissolveTime));
        material.SetFloat("Thresold",current);
        if (currentTime > endDissolveTime)
            Destroy(gameObject);
    }
    

}
