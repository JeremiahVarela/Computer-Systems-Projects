using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Optional (auto-filled if left empty)")]
    public Image image;                 // will auto-get if null
    public Transform dragLayerOverride; // optional: a dedicated layer under your UI canvas for dragging

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas rootCanvas;

    // where to return if drop is invalid
    public Transform parentAfterDrag;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup   = GetComponent<CanvasGroup>();

        // Try to auto-fill references to avoid NullRefs
        if (image == null) image = GetComponent<Image>();
        rootCanvas = GetComponentInParent<Canvas>();
        if (rootCanvas == null)
        {
            Debug.LogError("[InventoryItem] No Canvas found in parents. UI drag requires the item to be under a Canvas.");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent; // remember where we came from

        // Let pointer go "through" this item so drop targets can receive raycasts
        canvasGroup.blocksRaycasts = false;

        // Move to a safe top-level for correct draw order while dragging
        Transform targetParent = dragLayerOverride != null
            ? dragLayerOverride
            : (rootCanvas != null ? rootCanvas.transform : transform.root);

        transform.SetParent(targetParent, true);
        transform.SetAsLastSibling();

        // If using layouts, ignore while dragging so it doesn't fight you
        var le = GetComponent<LayoutElement>();
        if (le != null) le.ignoreLayout = true;

        // Optional: scale slightly or add visual feedback if you want
        // rectTransform.localScale = Vector3.one * 1.02f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Re-enable raycasts so this item can be clicked again
        canvasGroup.blocksRaycasts = true;

        // Safety: if OnBeginDrag didn’t run (e.g., component disabled mid-drag),
        // fall back to current parent to avoid NullRef.
        if (parentAfterDrag == null)
        {
            parentAfterDrag = transform.parent;
        }

        // Snap back to the last valid parent (if no IDropHandler changed it)
        transform.SetParent(parentAfterDrag, true);

        // If the parent uses a layout group, reset local position for a clean snap
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.localPosition    = Vector3.zero;

        var le = GetComponent<LayoutElement>();
        if (le != null) le.ignoreLayout = false;

        // rectTransform.localScale = Vector3.one;
    }
}
