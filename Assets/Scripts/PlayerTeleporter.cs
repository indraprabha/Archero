using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleporter : MonoBehaviour
{
    public GameObject teleportTarget;
    PlayerControl player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
    }

    void OnTriggerEnter()
    {
        // Change player's position to gate in next room. 
        // XXX: addtional vector added is a hack. To be removed once proper gate disable method is figured
        player.transform.position = teleportTarget.transform.position + new Vector3(5,0,5);
        player.gate = teleportTarget;
        player.EnterNewRoom();
    }
}
