using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : CharacterControl
{

    GameObject player;
    Animator anim;
    Rigidbody characterRigidbody;

    // Awake is called at beginning irrespective 
    // of script enabled or not
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        characterRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        TrackPlayer();
        Shoot();
    }

    void TrackPlayer()
    {
        // Get offset
        Vector3 lookVector = player.transform.position - transform.position;

        // Ignore y axis
        lookVector.y = transform.position.y;

        // Turn to look at player
        Quaternion targetRotation = Quaternion.LookRotation(lookVector);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
    }

    void Shoot()
    {

    }
}
