using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager inst;
    void Awake() { inst = this;  }

    [SerializeField] AudioClip[] _impacts;
    [SerializeField] AudioSource _source;

    public void PlayImpact(int index)
    {
        _source.PlayOneShot(_impacts[index]);
    }
}
