//Drag n Drop inventory script
//Timothy Oltjenbruns, 2016

using System.Linq;
using Assets;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class InventoryUi : MonoBehaviour
{
    public Vector3 InventoryOffset;
    public Inventory Inventory;
    private InventorySlotUi _selectedSlot;
    private Item.Id _cursorItem;
    private InventorySlotUi _cursorSlot;
    private GameObject _cursorObject;

    public void UpdateInventory(Inventory inventory)
    {
        foreach (var inventorySlot in GetComponentsInChildren<InventorySlotUi>())
        {
            var item = inventory[inventorySlot.Slot];
            if (item != Item.Id.None)
                UpdateInventoryPreview(inventorySlot, item);
        }
    }

    private Vector3 _activeInventoryOffset;
    private bool _paused;
    public bool Paused
    {
        get { return _paused; }
        set
        {
            _paused = value;
            if (_paused)
            {
                _activeInventoryOffset = InventoryOffset * 
                    (  Inventory.Skip(4).Take(6).Count(item => item != Item.Id.None) == 6 //first row of inventory is full
                    || Inventory.Skip(10).Count(item => item != Item.Id.None) > 0 ? 1 : 0.5f); //second row contains anything
                GetComponent<RectTransform>().transform.localPosition += _activeInventoryOffset;
            }
            else
                GetComponent<RectTransform>().transform.localPosition -= _activeInventoryOffset;
        }
    }
    
	void Update ()
	{
	    if (Input.GetKeyDown(KeyCode.E))
	        Paused = !Paused;
	}

    public void OnInventoryClick(InventorySlotUi slot)
    {
        if (slot == null) return;
        Inventory.SelectedSlot = slot.Slot;
        Image previousSelectedImage;
        if (_selectedSlot != null && _selectedSlot.Background != null && (previousSelectedImage = _selectedSlot.Background.GetComponent<Image>()) != null)
        previousSelectedImage.color = new Color(1, 1, 1, 100f/256f); //default color
        Image selectedImage;
        if (slot.Background != null && (selectedImage = slot.Background.GetComponent<Image>()) != null)
            selectedImage.color = new Color(.5f, .5f, 1, 100f / 256f);
        _selectedSlot = slot;
        Debug.Log(string.Format("click: #{0} {1}", slot.Slot, slot.name));
    }

    public void OnInventoryBeginDrag(InventorySlotUi slot)
    {
        if (slot == null) return;
        _cursorItem = Inventory[slot.Slot];
        _cursorSlot = slot;
        _cursorObject = slot.Preview;
        //float above all other ui elements
        _cursorObject.transform.parent = transform;
        _cursorObject.transform.SetAsLastSibling();
        slot.Preview = null;
        Inventory[slot.Slot] = Item.Id.None;
        //DestroyImmediate(slot.Preview);
        Debug.Log(string.Format("begindrag: #{0} {1}", slot.Slot, slot.name));
    }

    public void OnInventoryDrag(InventorySlotUi slot)
    {
        if (slot == null || _cursorObject == null) return;
        var screenPoint = Input.mousePosition;
        screenPoint.z = 1.8f; //distance of the plane from the camera
        _cursorObject.transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
        Debug.Log("drag: " + _cursorObject.transform.position);
    }

    private static void UpdateInventoryPreview(InventorySlotUi slot, Item.Id item)
    {
        if (slot == null || item == Item.Id.None) return;
        Destroy(slot.Preview);

        if (slot.ItemName != null)
            slot.ItemName.text = Item.GetName(item);

        slot.Preview = Instantiate(slot.Background, slot.transform) as GameObject;
        if (slot.Preview == null) return;
        //Scale sprite to fit inside background
        slot.Preview.transform.localScale = new Vector3(.8f, .8f, .8f);
        //slot.Preview.transform.SetAsLastSibling();
        var image = slot.Preview.GetComponent<Image>();
        if (image == null) return;
        image.sprite = Resources.Load<Sprite>(Item.GetSpriteResource(item));
        //Reset the color, the base object will be colorized
        image.color = Color.white;
        //The base object is sliced, reset this
        image.type = Image.Type.Simple;
        //Disable raycasting to prevent the drop event from misfiring
        image.raycastTarget = false;
    }

    public void OnInventoryDrop(InventorySlotUi slot)
    {
        if (slot == null) return;
        if (_cursorSlot != null)
        {
            Inventory.SwapSlots(_cursorSlot.Slot, slot.Slot);
            UpdateInventoryPreview(_cursorSlot, Inventory[_cursorSlot.Slot]);
        }
        Inventory[slot.Slot] = _cursorItem;
        UpdateInventoryPreview(slot, Inventory[slot.Slot]);
        Debug.Log(string.Format("drop: #{0} {1}", slot.Slot, slot.name));
        _cursorItem = Item.Id.None;
    }

    //Relies on OnDrop (when valid) being called before OnEndDrag
    public void OnInventoryEndDrag(InventorySlotUi slot)
    {
        if (slot == null) return;
        if (_cursorItem != Item.Id.None)
        {
            var forward = Inventory.transform.forward;
            forward.y += 1;
            var itemDrop = Instantiate(Resources.Load<GameObject>(Item.GetPrefabResource(_cursorItem)), Inventory.transform.position + forward, Inventory.transform.rotation) as GameObject;
            forward.Scale(new Vector3(50, 1, 50));
            if (itemDrop != null && itemDrop.GetComponent<Rigidbody>() != null)
                itemDrop.GetComponent<Rigidbody>().AddForce(forward);
            _cursorItem = Item.Id.None;
        }
        _cursorSlot = null;
        DestroyImmediate(_cursorObject);
        Debug.Log(string.Format("enddrag: #{0} {1}", slot.Slot, slot.name));
    }
}
