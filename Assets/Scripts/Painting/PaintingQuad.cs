using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingQuad : MonoBehaviour
{
    static void Create(Vector2 location)
    {
        Vector3 position = new Vector3(location.x, location.y, GridManager.instance.floorOffset);
        Instantiate(PrefabManager.instance.paintingQuadPrefab,position,Quaternion.identity);
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
