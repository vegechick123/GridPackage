using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    #region KeyWord
    public KeyCode KeyPickUp;
    public KeyCode KeyPutDown;
    public KeyCode KeyC;
    public KeyCode KeyD;
    public KeyCode KeyE;
    public KeyCode KeyF;
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
        Input.GetKeyDown(KeyPickUp);
        return Mathf.Sqrt(x * x + y * y);
    }
}
