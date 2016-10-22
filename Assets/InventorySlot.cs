//Drag n Drop inventory script
//Timothy Oltjenbruns, 2016
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IDragHandler, IDropHandler, IEndDragHandler, IBeginDragHandler
{
    public int Slot;
    public InventoryUi InventoryUiScript;
    public GameObject Background;
    public GameObject Preview { get; set; }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (InventoryUiScript != null)
            InventoryUiScript.OnInventoryClick(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (InventoryUiScript != null)
            InventoryUiScript.OnInventoryDrag(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (InventoryUiScript != null)
            InventoryUiScript.OnInventoryDrop(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (InventoryUiScript != null)
            InventoryUiScript.OnInventoryEndDrag(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (InventoryUiScript != null)
            InventoryUiScript.OnInventoryBeginDrag(this);
    }
}
