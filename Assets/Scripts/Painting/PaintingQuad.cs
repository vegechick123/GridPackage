using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingQuad : MonoBehaviour
{
    public float beginDissolveTime=2f;
    public float endDissolveTime = 5f;
    public float currentTime = 0f;
    public Color color;
    public Material material;
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
        if (currentTime > endDissolveTime)
            Destroy(gameObject);

    }
    

}
