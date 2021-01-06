using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFloating : MonoBehaviour
{
    public Transform target;
    private Vector3 center { get { return transform.position; } }
    private Vector3 originEuler;
    private Vector2 originFoward;
    private Vector3 targetEuler;
    private float currentY { get { return transform.rotation.eulerAngles.y; } }
    public float scale = 1;
    public float maxDegreePerSecond=1f;
    public float fade=0.5f;
    public float floatScale=0.1f;
    public float floatSpeed = 0.1f;
    public Vector3 floatTime;//= new Vector3(Random.Range(0,2*Mathf.PI), Random.Range(0, 2 * Mathf.PI), Random.Range(0, 2 * Mathf.PI));
    private void Start()
    {
        originEuler = transform.rotation.eulerAngles;
        originFoward = new Vector3(transform.forward.x, transform.forward.z).normalized;
        floatTime = new Vector3(Random.Range(0, 2 * Mathf.PI), Random.Range(0, 2 * Mathf.PI), Random.Range(0, 2 * Mathf.PI));
    }
    private void Update()
    {
        floatTime.x += Time.deltaTime+Random.Range(0, Time.deltaTime);
        floatTime.y += Time.deltaTime + Random.Range(0, Time.deltaTime);
        floatTime.z += Time.deltaTime + Random.Range(0, Time.deltaTime);

        //transform.position = floatScale*new Vector3(Mathf.Sin(floatTime.x), Mathf.Sin(floatTime.y), Mathf.Sin(floatTime.z)) *Mathf.Sin(floatSpeed*Time.time)+center;
        SetTargetEuler();

        float next = Mathf.Lerp(currentY, targetEuler.y ,fade);

        transform.rotation = Quaternion.Euler(new Vector3(originEuler.x,next, originEuler.z));

    }
    void SetTargetEuler()
    {
        Vector3 targetDirection = (target.position - transform.position);

        Vector2 targetForward = new Vector2(targetDirection.x, targetDirection.z).normalized;

        float angle = Vector2.SignedAngle(targetForward, originFoward);

        float targetAngle = originEuler.y + angle;

        angle = Mathf.Lerp(originEuler.y, targetAngle, scale);

        targetEuler = new Vector3(originEuler.x, angle, originEuler.z);
    }
}
