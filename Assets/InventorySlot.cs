//Drag n Drop inventory script
//Timothy Oltjenbruns, 2016
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IDragHandler, IDropHandler, IEndDragHandler, IBeginDragHandler
{
    public int Slot;
    public UI UiScript;
    public GameObject Background;
    public GameObject Preview;

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        if (UiScript != null)
            UiScript.OnInventoryClick(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (UiScript != null)
            UiScript.OnInventoryDrag(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (UiScript != null)
            UiScript.OnInventoryDrop(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (UiScript != null)
            UiScript.OnInventoryEndDrag(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (UiScript != null)
            UiScript.OnInventoryBeginDrag(this);
    }
}
