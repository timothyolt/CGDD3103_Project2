using System;
using Assets;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class UI : MonoBehaviour
{
    public Vector3 InventoryOffset;

    private Item[] _inventory;
    private int _selected;
    private Item _cursor;

	// Use this for initialization
	void Start () {
	    _inventory = new Item[16];
	}

    private bool _paused;
    public bool Paused
    {
        get { return _paused; }
        set
        {
            _paused = value;
            if (_paused)
                GetComponent<RectTransform>().transform.localPosition += InventoryOffset;
            else
                GetComponent<RectTransform>().transform.localPosition -= InventoryOffset;
        }
    }
    
	// Update is called once per frame
	void Update ()
	{
	    if (Input.GetKeyDown(KeyCode.E))
	        Paused = !Paused;
	}

    public Item OnInventoryClick(int slot)
    {
        _selected = slot;
        _inventory[slot] = Item.Health;
        return Item.Health;
    }
    public Item OnInventoryDrag(int slot)
    {
        if (!_cursor.Equals(Item.None)) return _inventory[slot];
        _cursor = _inventory[slot];
        _inventory[slot] = Item.None;
        return Item.None;
    }

    public Item OnInventoryDrop(int slot)
    {
        _inventory[slot] = _cursor;
        _cursor = Item.None;
        return _inventory[slot];
    }
}
