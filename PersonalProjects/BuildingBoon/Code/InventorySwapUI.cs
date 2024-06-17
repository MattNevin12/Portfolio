using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySwapUI : MonoBehaviour
{
    [SerializeField]
    private GameObject toolInventory;
    [SerializeField]
    private GameObject genInventory;

    public int barChildNum;
    public int iconChildNum;

    public Vector2 activeSize;
    public Vector2 backgroundSize;

    public Vector3 activeLocalPos;
    public Vector3 backgroundLocalPos;

    public float swapSpeed;

    private GameObject activeInv;
    private GameObject backgroundInv;

    private bool swapping;

    private void Start()
    {
        SetInventoryUI(InventoryBehavior.toolActive);
    }

    private void Update()
    {
        if (swapping && activeInv != null && backgroundInv != null)
        {
            AnimateSwap(activeInv, backgroundInv);
        }
    }

    private void SetInventoryUI(bool toolActive)
    {
        if (toolActive)
        {
            activeInv = toolInventory;
            backgroundInv = genInventory;
        }
        else
        {
            activeInv = genInventory;
            backgroundInv = toolInventory;
        }

        activeInv.transform.localPosition = new Vector3(activeLocalPos.x, activeLocalPos.y, 0);
        backgroundInv.transform.localPosition = new Vector3(backgroundLocalPos.x, backgroundLocalPos.y, 0);

        ConfigureIcons(activeInv, backgroundInv);
    }

    void ConfigureIcons(GameObject newActive, GameObject newBackground)
    {
        newActive.transform.SetAsLastSibling();
        newActive.transform.GetChild(iconChildNum).gameObject.GetComponent<RectTransform>().sizeDelta = activeSize;
        newActive.transform.GetChild(barChildNum).gameObject.SetActive(true);

        newBackground.transform.GetChild(iconChildNum).gameObject.GetComponent<RectTransform>().sizeDelta = backgroundSize;
        newBackground.transform.GetChild(barChildNum).gameObject.SetActive(false);
    }

    void SwitchActiveIcon(bool tActive)
    {
        if (tActive)
        {
            activeInv = genInventory;
            backgroundInv = toolInventory;
        }
        else
        {
            activeInv = toolInventory;
            backgroundInv = genInventory;
        }
    }

    void AnimateSwap(GameObject newActive, GameObject newBackground)
    {
        newActive.transform.localPosition = new Vector3(Mathf.MoveTowards(newActive.transform.localPosition.x, activeLocalPos.x, swapSpeed * Time.deltaTime),
                                                Mathf.MoveTowards(newActive.transform.localPosition.y, activeLocalPos.y, swapSpeed * Time.deltaTime),
                                                0);

        newBackground.transform.localPosition = new Vector3(Mathf.MoveTowards(newBackground.transform.localPosition.x, backgroundLocalPos.x, swapSpeed * Time.deltaTime),
                                                    Mathf.MoveTowards(newBackground.transform.localPosition.y, backgroundLocalPos.y, swapSpeed * Time.deltaTime),
                                                    0);

        if (newActive.transform.localPosition == activeLocalPos && newBackground.transform.localPosition == backgroundLocalPos)
        {
            ConfigureIcons(newActive, newBackground);

            if (newActive == toolInventory)
                InventoryBehavior.toolActive = true;
            else
                InventoryBehavior.toolActive = false;

            swapping = false;
        }
    }

    public void SwapClockwise()
    {
        SwitchActiveIcon(InventoryBehavior.toolActive);
        ConfigureIcons(activeInv, backgroundInv);
        swapping = true;
    }

    public void SwapCounterClockwise()
    {
        SwitchActiveIcon(InventoryBehavior.toolActive);
        swapping = true;
    }
}
