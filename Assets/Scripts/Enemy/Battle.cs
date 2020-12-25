using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Battle:MonoBehaviour
{
    private void FixedUpdate()
    {
        foreach (var tran in GetAllChilds())
        {
            if (tran.childCount != 0 || tran.GetComponent<EnemyBorn>().isEnd == false)
                return;
        }
        Debug.Log("Win!");
    }
    Transform[] GetAllChilds()
    {
        Queue<Transform> transforms = new Queue<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            transforms.Enqueue(transform.GetChild(i));
        }
        return transforms.ToArray();
    }
}

