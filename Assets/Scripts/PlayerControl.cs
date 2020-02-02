using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : CharacterControl
{
    public Material hit;
    public Material notHit;
    public GameObject gate;
    public Slider healthSlider;
    public float timeBetweenShots = 0.5f;
    public float firingRange = 10f;
    public int enemyLayerMask = 10;
    float timer;

    ParticleSystem gunParticles;
    Light gunLight;
    float effectsDisplayTime = 0.2f;

    private bool _isWalking;
    GameObject CurrentRoom;
    Vector3 movement;
    Animator anim;
    Rigidbody characterRigidbody;
    List<EnemyControl> enemies = new List<EnemyControl>();

    // Awake is called at beginning irrespective 
    // of script enabled or not
    private void Awake()
    {
        _isWalking = false;
        anim = GetComponent<Animator>();
        characterRigidbody = GetComponent<Rigidbody>();

        // Get the Player Gun object to enable/disable firing shots animation
        GameObject gun = transform.Find("Gun").gameObject;
        gunParticles = gun.GetComponent<ParticleSystem>();
        gunLight = gun.GetComponent<Light>();
        EnterNewRoom();
    }

    // On entering new room, ensure teleported gate to next room is disabled 
    // until enemies are killed. Also collect enemy list in the room.
    public void EnterNewRoom()
    {
        gate.GetComponent<PlayerTeleporter>().enabled = false;
        CurrentRoom = gate.transform.parent.gameObject;
        enemies.AddRange(CurrentRoom.GetComponentsInChildren<EnemyControl>());
    }

    private void Update()
    {

        if (isDead())
        {
            Debug.Log("Player Dead! GAME OVER..");
            DisableEffects();
            return;
        }
        
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;

        // Reset skin back to normal
        SetSkinMaterial(notHit);

        // Attack only if player is not moving and enough time lapsed since last attack
        if (!_isWalking && timer >= timeBetweenShots)
        {
            Debug.Log("Time elapsed" + timer+" >= "+timeBetweenShots);
            Attack();
        }

        // Check whether timer has exceeded time to display shooting effects
        if (timer >= timeBetweenShots * effectsDisplayTime)
        {
            // ... disable the effects.
            DisableEffects();
        }

    }

    // FixedUpdate is called on every physics update 
    void FixedUpdate()
    {
        if(isDead())
        {
            Debug.Log("Player Dead! GAME OVER..");
            return;
        }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        _isWalking = IsWalking(h,v);
        if (_isWalking)
        {
            SetWalking(_isWalking);
            Move(h, v);
            Turn();
        }
        SetWalking(false);
    }

    // Move Character as per user input
    void Move(float h, float v)
    {
        // Set X and Z axes according to horizontal and vertical inputs 
        movement.Set(h, 0f, v);

        // Normalize movement in both axes with desired speed
        movement = movement.normalized * speed * Time.deltaTime;
        characterRigidbody.MovePosition(transform.position + movement);
    }

    // Turn Character towards the direction of user input
    void Turn ()
    {
        // Ignore Y axis, so that player is always along floor
        movement.y = transform.position.y;

        // Determine direction of rotation 
        Quaternion targetRotation = Quaternion.LookRotation(transform.position - movement);

        // Smoothly rotate from current lookrotation to new one at given speed
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
    }

    void SetWalking(bool isWalking)
    {
        // Set animator's IsWalking parameter if either 
        // horizontal or vertical axis input received
        anim.SetBool("IsWalking", isWalking);
    }

    public override void Attack()
    {
        // Reset timer
        timer = 0f;

        RaycastHit hit;
        int enemiesAlive = 0;

        //Attack Enemies in Range round-robin
        foreach (EnemyControl enemy in enemies)
        {
            // Enable the light.
            gunLight.enabled = true;

            // Stop the particles from playing if they were, then start the particles.
            gunParticles.Play();

            if (!enemy.isDead())
            {
                enemiesAlive++;

                // Turn towards enemy
                Quaternion targetRotation = Quaternion.LookRotation(enemy.transform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed);

                // And attack
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, firingRange))
                {
                    if (hit.collider.name.Equals(enemy.name))
                    {
                        Debug.Log("Hit enemy: " + hit.collider.name + " at a distance: " + hit.distance);
                        enemy.FaceAttack();
                    }
                        
                }
            }
        }

        // If all enemies are dead inside current room, enable teleporter gate to next room
        if(enemiesAlive == 0)
        {
            EnableTeleportGate();
        }
    }

    public void FaceAttack()
    {
        SetSkinMaterial(hit);
        characterHealth -= attackPower;

        // Set the health bar's value to the current health.
        healthSlider.value = characterHealth;
        Debug.Log("Player hit! health reduced to " + characterHealth + "hP ");

        if (characterHealth <= 0)
        {
            SetSkinMaterial(hit);
            anim.SetTrigger("Dead");
        }
    }

    public void DisableEffects()
    {
        // Disable the light.
        gunLight.enabled = false;
        gunParticles.Stop();
    }

    bool IsWalking(float h, float v)
    {
        return h != 0f || v != 0f;
    }

    // Teleporter Gate to be shown to Player after enemies are killed
    void EnableTeleportGate()
    {
        Renderer gateway = gate.GetComponent<Renderer>();
        gateway.enabled = true;
        gate.GetComponent<PlayerTeleporter>().enabled = true;
    }
}
