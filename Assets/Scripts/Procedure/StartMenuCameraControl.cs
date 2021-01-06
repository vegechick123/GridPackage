using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuCameraControl : MonoBehaviour
{
    public Animator targetAnimator;
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Player>())
        {
            targetAnimator.SetTrigger("Enter");
        }
    }
}
