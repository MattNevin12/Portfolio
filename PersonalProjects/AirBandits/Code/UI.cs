using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public GameObject cargoPanel;
    public GameObject currentOrderPanel;

    public void CloseCargoPanel()
    {
            cargoPanel.SetActive(false);
    }

    public void CloseOrderPanel()
    {
        currentOrderPanel.SetActive(false);
    }
}
