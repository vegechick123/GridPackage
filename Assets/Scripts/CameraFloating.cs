using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFloating : MonoBehaviour
{
    public Transform target;
    public Vector3 center;
    public float originRotationY;
    public float scale=1;
    private void Start()
    {
        center = transform.position;
        originRotationY = transform.rotation.eulerAngles.y;
    }
    private void Update()
    {
        Vector3 targetDirection = (target.position - transform.position).normalized;
        transform.forward = targetDirection;
        //transform.rotation=transform.forward
    }
}
