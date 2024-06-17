using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindInteractable : MonoBehaviour
{
    public Material red;
    public Material green;

    private bool isNewTile;

    public List<int> interactables = new List<int>();

    private bool listEmpty = true;

    public void Update()
    {
        if (interactables.Count > 0)
            listEmpty = false;
        else
            listEmpty = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "interactable")
        {
            interactables.Add(other.GetComponent<TileData>().ID);
            isNewTile = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "interactable")
        {
            other.transform.GetChild(0).gameObject.SetActive(false);
            interactables.Remove(other.GetComponent<TileData>().ID);
            isNewTile = true;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (!listEmpty)
        {
            if (isNewTile && other.GetComponent<TileData>().ID == interactables[0])
            {
                other.transform.GetChild(0).gameObject.SetActive(true);
                isNewTile = false;
            }
        }
    }
}
