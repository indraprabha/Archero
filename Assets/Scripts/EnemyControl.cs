using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : CharacterControl
{
    Animator anim;
    Rigidbody characterRigidbody;

    // Awake is called at beginning irrespective 
    // of script enabled or not
    void Awake()
    {
        anim = GetComponent<Animator>();
        characterRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TrackPlayer();
        Shoot();
    }

    void TrackPlayer()
    {

    }

    void Shoot()
    {

    }
}
