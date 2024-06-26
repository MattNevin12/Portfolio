﻿using UnityEngine;
using Mirror;
using System.Collections.Generic;

/*
	Visibility Guide: https://mirror-networking.com/docs/Guides/Visibility.html
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkBehaviour.html
*/

public class PlayerAirplaneObserver : NetworkVisibility
{
    /// <summary>
    /// Callback used by the visibility system to determine if an observer (player) can see this object.
    /// <para>If this function returns true, the network connection will be added as an observer.</para>
    /// </summary>
    /// <param name="conn">Network connection of a player.</param>
    /// <returns>True if the player can see this object.</returns>
    public override bool OnCheckObserver(NetworkConnection conn)
    {
        return true;
    }
    
    /// <summary>
    /// Callback used by the visibility system to (re)construct the set of observers that can see this object.
    /// <para>Implementations of this callback should add network connections of players that can see this object to the observers set.</para>
    /// </summary>
    /// <param name="observers">The new set of observers for this object.</param>
    /// <param name="initialize">True if the set of observers is being built for the first time.</param>
    public override void OnRebuildObservers(HashSet<NetworkConnection> observers, bool initialize) { }
}
