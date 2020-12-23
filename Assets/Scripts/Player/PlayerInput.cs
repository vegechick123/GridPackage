using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    #region KeyWord
    public string KeyA;
    public string KeyB;
    public string KeyC;
    public string KeyD;
    public string KeyE;
    public string KeyF;
    #endregion

    public float Dup;
    public float Dright;
    public float Dis;

    void Start()
    {
        Dup = 0;
        Dright = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Dup = Input.GetAxis("Horizontal");
        Dright = Input.GetAxis("Vertical");
        Dis = Distance(Dup, Dright);
    }
    private float Distance(float x, float y)
    {
        return Mathf.Sqrt(x * x + y * y);
    }
}
