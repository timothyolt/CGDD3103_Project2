//Drag n Drop inventory script
//Timothy Oltjenbruns, 2016
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IDragHandler, IDropHandler, IEndDragHandler, IBeginDragHandler
{
    public int Slot;
    public UiHandler UiHandlerScript;
    public GameObject Background;
    public GameObject Preview { get; set; }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (UiHandlerScript != null)
            UiHandlerScript.OnInventoryClick(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (UiHandlerScript != null)
            UiHandlerScript.OnInventoryDrag(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (UiHandlerScript != null)
            UiHandlerScript.OnInventoryDrop(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (UiHandlerScript != null)
            UiHandlerScript.OnInventoryEndDrag(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (UiHandlerScript != null)
            UiHandlerScript.OnInventoryBeginDrag(this);
    }
}
