using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class parkingSpotNum : NetworkBehaviour
{
    public int parkingSpot;

    [SyncVar]
    public bool isTaken;

    public void Start()
    {
        parkingSpot = transform.GetSiblingIndex();
    }
}
