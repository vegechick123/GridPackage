using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Manager<SoundManager>
{
    public AudioClip[] pigmentSound;
    public AudioClip[] shootSound;
    public AudioClip GetRandomPigmentSound()
    {
        return pigmentSound[Random.Range(0, pigmentSound.Length)];
    }
    public AudioClip GetRandomShootSound()
    {
        return shootSound[Random.Range(0, shootSound.Length)];
    }
}
