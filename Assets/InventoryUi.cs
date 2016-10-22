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
            inventorySlot.UpdateUi(item);
        }
    }

    private Vector3 _activeInventoryOffset;
    private bool _open;
    public bool Open
    {
        get { return _open; }
        set
        {
            _open = value;
            if (_open)
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
	        Open = !Open;
	    if (Inventory == null)
	        return;
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Inventory.UseItem(0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            Inventory.UseItem(1);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            Inventory.UseItem(2);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            Inventory.UseItem(3);
    }

    public void OnInventoryClick(InventorySlotUi slot)
    {
        if (Inventory == null || slot == null) return;
        Inventory.SelectedSlot = slot.Slot;
        Image previousSelectedImage;
        if (_selectedSlot != null && _selectedSlot.Background != null && (previousSelectedImage = _selectedSlot.Background.GetComponent<Image>()) != null)
        previousSelectedImage.color = new Color(1, 1, 1, 100f/256f); //default color
        Image selectedImage;
        if (slot.Background != null && (selectedImage = slot.Background.GetComponent<Image>()) != null)
            selectedImage.color = new Color(.5f, .5f, 1, 100f / 256f);
        _selectedSlot = slot;
        Inventory.UseItem(_selectedSlot.Slot);
        Debug.Log(string.Format("click: #{0} {1}", slot.Slot, slot.name));
    }

    public void OnInventoryBeginDrag(InventorySlotUi slot)
    {
        if (slot == null) return;
        _cursorItem = Inventory[slot.Slot];
        _cursorSlot = slot;
        //Copy slot
        _cursorObject = (GameObject) Instantiate(slot.gameObject, slot.transform);
        //Delete background and ui script
        Destroy(_cursorObject.GetComponent<InventorySlotUi>().Background);
        Destroy(_cursorObject.GetComponent<InventorySlotUi>());
        //float above all other ui elements
        _cursorObject.transform.parent = transform;
        _cursorObject.transform.SetAsLastSibling();
        //Delete previous item data
        Destroy(slot.PreviewImage);
        if (slot.ItemName != null)
            slot.ItemName.text = "";
        Inventory[slot.Slot] = Item.Id.None;
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

    public void OnInventoryDrop(InventorySlotUi slot)
    {
        if (slot == null) return;
        if (_cursorSlot != null)
        {
            Inventory.SwapSlots(_cursorSlot.Slot, slot.Slot);
            _cursorSlot.UpdateUi(Inventory[_cursorSlot.Slot]);
        }
        Inventory[slot.Slot] = _cursorItem;
        slot.UpdateUi(Inventory[slot.Slot]);
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
