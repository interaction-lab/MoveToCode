using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighFive : MonoBehaviour {
    public Animator anim;
    private AudioSource source;

    void Start() {
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    void OnCollisionExit(Collision collisionInfo) {
        anim.SetTrigger("HighFiveEnd");
        source.Play();
    }
}
