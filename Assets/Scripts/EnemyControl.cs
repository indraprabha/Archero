using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : CharacterControl
{
    public Material hit;
    public Material notHit;
    public float timeBetweenShots = 0.5f;
    float timer;

    PlayerControl player;
    Animator anim;

    // Awake is called at beginning irrespective 
    // of script enabled or not
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;

        SetSkinMaterial(notHit);

        if(timer >= timeBetweenShots && !player.isDead())
        {
            Attack();
        }
    }

    public override void Attack()
    {
        // Reset timer
        timer = 0f;

        // Get offset
        Vector3 lookVector = player.transform.position - transform.position;

        // Ignore y axis
        lookVector.y = transform.position.y;

        // Turn to look at player
        Quaternion targetRotation = Quaternion.LookRotation(lookVector);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);

        RaycastHit hit;
        if (!player.isDead())
        {
            Debug.Log("Player alive! Attacking..");
            if (Physics.Raycast(transform.position, transform.TransformDirection(player.transform.position), out hit, Mathf.Infinity))
            {
                player.FaceAttack();
            }
        }
    }

    public void FaceAttack()
    {
        SetSkinMaterial(hit);

        characterHealth -= attackPower;

        if (characterHealth <= 0)
        {
            anim.SetTrigger("Dead");
        }
    }

}
