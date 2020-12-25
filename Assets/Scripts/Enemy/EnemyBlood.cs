using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlood : MonoBehaviour
{
    private Transform lockCamera;
    private Enemy enemy;
    /*—————定时消失——————*/
    public float DisableTime;
    private float time;
    /*——————————————*/
    private void Awake()
    {
        lockCamera = Camera.main.transform;
        //this.gameObject.SetActive(false);
        time = 0;

        enemy = this.transform.parent.GetComponent<Enemy>();
    }
    // Update is called once per frame
    void Update()
    {
        //this.transform.forward = -lockCamera.forward;
        this.transform.forward = enemy.transform.forward;
        this.transform.localScale = new Vector3(((float)enemy.CurrentHealth / (float)enemy.health )* 0.8f, 0.06f, 0.01f);
        time += Time.deltaTime;
        if (time >= DisableTime)
        {
            time = 0;
            this.gameObject.SetActive(false);
        }
    }
}