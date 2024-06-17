using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class BarrelSpawn : NetworkBehaviour
{
    public GameObject barrel;

    public float spawnRangeX;
    public float spawnRangeY;

    public int barrelCount;

    [ServerCallback]
    private void Start()
    {
        for (int i = 0; i < barrelCount; i++)
        { 
            GameObject newBarrel = Instantiate(barrel, new Vector3(Random.Range(transform.position.x + -spawnRangeX, transform.position.x + spawnRangeX), Random.Range(-spawnRangeY + transform.position.y, spawnRangeY + transform.position.y), 0f), transform.rotation);
            NetworkServer.Spawn(newBarrel);
            newBarrel.GetComponent<BarrelPickup>().spawner = gameObject;
        }
    }

    [ServerCallback]
    public void SpawnBarrel()
    {
            GameObject newBarrel = Instantiate(barrel, new Vector3(Random.Range(transform.position.x + -spawnRangeX,transform.position.x +  spawnRangeX), Random.Range(-spawnRangeY + transform.position.y, spawnRangeY + transform.position.y), 0f), transform.rotation);
            NetworkServer.Spawn(newBarrel);
            newBarrel.GetComponent<BarrelPickup>().spawner = gameObject;
    }
}