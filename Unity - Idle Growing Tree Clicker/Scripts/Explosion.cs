using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] ParticleSystem levelUpParticle = null;

    public void playParticle()
    {
        levelUpParticle.Play();
    }
}
