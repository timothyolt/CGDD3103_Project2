//Drag n Drop inventory script
//Timothy Oltjenbruns, 2016

using System.Collections;
using System.Collections.Generic;
using Assets;
using UnityEngine;

public class Inventory : MonoBehaviour, IEnumerable<Item.Id>
{
    public InventoryUi InventoryUi;
    public LivingEntity User;
    private Item.Id[] _slots;
    public int SelectedSlot { get; set; }

    public Item.Id this[int index]
    {
        get { return _slots[index]; }
        set { _slots[index] = value; }
    }

    public void Condense()
    {
        for (var none = 4; none < _slots.Length; none++)
        {
            if (_slots[none] != Item.Id.None) continue;
            for (var filled = none + 1; filled < _slots.Length; filled++)
                if (_slots[filled] != Item.Id.None)
                {
                    SwapSlots(none, filled);
                    break;
                }
        }
    }

    public void UseItem(int index)
    {
        if (User == null) return;
        switch (_slots[index])
        {
            case Item.Id.Health:
                User.Health += 10;
                break;
            case Item.Id.Health2:
                User.Health += 30;
                break;
            case Item.Id.Health3:
                User.Health = 100;
                break;
        }
        _slots[index] = Item.Id.None;
        InventoryUi.UpdateInventory(this);
    }

    public void SwapSlots(int toIndex, int fromIndex)
    {
        var temp = _slots[toIndex];
        _slots[toIndex] = _slots[fromIndex];
        _slots[fromIndex] = temp;
    }

    // Use this for initialization
    void Start()
    {
        _slots = new Item.Id[16];
    }

    void OnCollisionEnter(Collision collision)
    {
        var pickup = collision.gameObject.GetComponent<Pickup>();
        if (pickup == null) return;
        for (var i = 4; i < _slots.Length; i++)
            if (_slots[i] == Item.Id.None)
            {
                _slots[i] = pickup.Item;
                Destroy(collision.gameObject);
                if (InventoryUi != null)
                    InventoryUi.UpdateInventory(this);
                return;
            }
        //TODO: notify inventory full
    }

    public IEnumerator<Item.Id> GetEnumerator()
    {
        return ((IEnumerable<Item.Id>) _slots).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _slots.GetEnumerator();
    }
}
