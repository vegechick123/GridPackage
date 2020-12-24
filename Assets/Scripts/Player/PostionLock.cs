using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PostionLock : MonoBehaviour
{
    public Transform lockFather;
    Vector3 deltaPos;
    private void Awake()
    {
        deltaPos = lockFather.position - this.transform.position;
    }
    private void Update()
    {
        lockFather.position = this.transform.position + deltaPos;
    }
}

