using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public IReceiveable target;
    protected virtual void Shoot(IReceiveable target)
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            if (target != null)
            {
                target.Receive(this);
            }
        }
    }
}
