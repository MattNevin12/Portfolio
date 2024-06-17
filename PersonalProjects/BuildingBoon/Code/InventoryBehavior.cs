using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBehavior : MonoBehaviour
{
    public GameObject InventoryWindow;
    public GameObject ruckInventoryBar;
    public GameObject toolInventoryBar;

    public static bool toolActive = true;

    public void OpenInventory()
    {
        InventoryWindow.SetActive(!InventoryWindow.activeInHierarchy);
        ruckInventoryBar.SetActive(!ruckInventoryBar.activeInHierarchy);
        toolInventoryBar.SetActive(!toolInventoryBar.activeInHierarchy);
    }
}
