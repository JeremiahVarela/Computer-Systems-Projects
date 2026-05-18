using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Optional visuals")]
    public Image highlight;

    [Header("State")]
    public bool isOccupied;
    public InventoryItem currentItem;

    // Extend this for item rules (e.g., by item type/size)
    public bool CanAccept(InventoryItem item)
    {
        // Only accept if the slot is free. Add more rules here if needed.
        return !isOccupied && item != null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        var go = eventData.pointerDrag;
        if (go == null) return;

        var item = go.GetComponent<InventoryItem>();
        if (item == null) return;

        // Accept only if valid
        if (!CanAccept(item)) return;

        // Claim the item: set where it should return to on EndDrag
        item.parentAfterDrag = transform;

        // Reparent now so EndDrag just snaps & finishes cleanly
        item.transform.SetParent(transform, true);
        var rt = item.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.anchoredPosition = Vector2.zero;
            rt.localPosition = Vector3.zero;
        }

        // Mark occupied
        isOccupied = true;
        currentItem = item;
    }

    // Called by an item that starts dragging out of this slot
    public void Clear()
    {
        isOccupied = false;
        currentItem = null;
    }

    // Optional hover visuals
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (highlight != null && !isOccupied) highlight.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (highlight != null) highlight.enabled = false;
    }

}
