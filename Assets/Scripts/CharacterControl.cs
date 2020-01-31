using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public float speed = 5f;
    public int characterHealth = 100;
    public int arrowsPerShot = 1;
    public int attackPower = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Attack()
    {
        // Shoot at target character
    }

    public bool isDead()
    {
        return characterHealth <= 0;
    }

    protected void SetSkinMaterial(Material mat)
    {
        GameObject tank = transform.Find("Model").gameObject;
        tank.GetComponent<Renderer>().material = mat;
    }
}
