using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class BarrelPickup : NetworkBehaviour
{
    //The spawner object that spawned this barrel
    public GameObject spawner;

    [ServerCallback]
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<playerBehavior>().myGold += 10;
            DestroyBarrel();
        }
    }

    [Server]
    public void DestroyBarrel()
    {
        spawner.GetComponent<BarrelSpawn>().SpawnBarrel();
        NetworkServer.Destroy(gameObject);

    }
}
