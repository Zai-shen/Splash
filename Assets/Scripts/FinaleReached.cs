using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinaleReached : MonoBehaviour
{
    public ParticleSystem fireWorksPE;
    private AudioSource fanfareAudio;

    private void Start()
    {
        fanfareAudio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            fanfareAudio.Play();
            fireWorksPE.Play();
        }
    }
}
