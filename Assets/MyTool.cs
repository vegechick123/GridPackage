using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTool : MonoBehaviour
{
    public Material material;
    public string TargetName;
    [ContextMenu("Set")]
    public void Set()
    {
        for(int i=0;i<transform.childCount;i++)
        {
            if (transform.GetChild(i).name == TargetName)
                transform.GetChild(i).GetChild(0).GetChild(0).gameObject.GetComponent<MeshRenderer>().material = material;
        }
    }
    public Material tree;
    [ContextMenu("SetTree")]
    public void SetTree()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Material[] materials = transform.GetChild(i).GetComponent<MeshRenderer>().materials;
           for (int j=0;j<materials.Length;j++)
            {
                if (materials[j].name == "Tree (Instance)")
                    materials[j] = tree;
            }
        }
    }

}
