using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupInventory : MonoBehaviour
{
    public GameObject mainInvBar1;
    public GameObject mainInvBar2;
    public GameObject mainInvBar3;
    public GameObject mainInvToolBar;

    public GameObject ruckBar;
    public GameObject toolBar;

    public GameObject inventoryWindow;

    public int slotsPerBar;
    public GameObject slotPrefab;

    public void Awake()
    {
        // Inventory bar 1, which is also the same bar displayed on the ruck bar
        for (int i = 0; i < slotsPerBar; i++)
        {
            Instantiate(slotPrefab, mainInvBar1.transform);
            Instantiate(slotPrefab, ruckBar.transform);
        }

        // Inventory bar 2
        for (int i = 0; i < slotsPerBar; i++)
        {
            Instantiate(slotPrefab, mainInvBar2.transform);
        }

        // Inventory bar 3
        for (int i = 0; i < slotsPerBar; i++)
        {
            Instantiate(slotPrefab, mainInvBar3.transform);
        }

        // Inventory tool bar, which is the same bar displayed on the tool bar
        for (int i = 0; i < slotsPerBar; i++)
        {
            Instantiate(slotPrefab, mainInvToolBar.transform);
            Instantiate(slotPrefab, toolBar.transform);
        }

        inventoryWindow.SetActive(false);
    }
}
