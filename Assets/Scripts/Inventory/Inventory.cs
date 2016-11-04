using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Inventory
{
    public class Inventory : MonoBehaviour, IEnumerable<InventoryItem>
    {
        public InventoryUi InventoryUi;
        public LivingEntity.LivingEntity User;
        private InventoryItem[] _items;
        public int SlotCapacity;
        public int SelectedSlot { get; set; }

        public InventoryItem this[int index]
        {
            get
            {
                if (_items[index] == null) return null;
                if (index < 4)
                    _items[index].Count = _items.Skip(4).Where(item => item != null && item.Item == _items[index].Item).Sum(item => item.Count);
                return _items[index];
            }
            set
            {
                _items[index] = value;
                if (value != null && index < 4)
                    _items[index].Count = _items.Skip(4).Where(item => item != null && item.Item == _items[index].Item).Sum(item => item.Count);
            }
        }

        public void Condense()
        {
            for (var none = 4; none < _items.Length; none++)
            {
                if (_items[none] != null) continue;
                for (var filled = none + 1; filled < _items.Length; filled++)
                    if (_items[filled] != null)
                    {
                        SwapSlots(none, filled);
                        break;
                    }
            }
        }

        public void UseItem(int index)
        {
            var forward = transform.forward;
            forward.y += 1;
            var force = transform.forward;
            GameObject itemDrop;
            if (User == null) return;
            var item = _items[index];
            switch (item?.Item)
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
                case Item.Id.Shot1:
                    force.Scale(new Vector3(500, 1, 500));
                    itemDrop = Instantiate(Resources.Load<GameObject>(Item.GetPrefabResource(item.Item)), transform.position + forward, transform.rotation) as GameObject;
                        if (itemDrop != null && itemDrop.GetComponent<Rigidbody>() != null)
                            itemDrop.GetComponent<Rigidbody>().AddForce(force);
                    break;
                case Item.Id.Shot2:
                    force.Scale(new Vector3(1000, 1, 1000));
                    itemDrop = Instantiate(Resources.Load<GameObject>(Item.GetPrefabResource(item.Item)), transform.position + forward, transform.rotation) as GameObject;
                    if (itemDrop != null && itemDrop.GetComponent<Rigidbody>() != null)
                        itemDrop.GetComponent<Rigidbody>().AddForce(force);
                    break;
                case Item.Id.Shot3:
                    force.Scale(new Vector3(1500, 1, 1500));
                    itemDrop = Instantiate(Resources.Load<GameObject>(Item.GetPrefabResource(item.Item)), transform.position + forward, transform.rotation) as GameObject;
                    if (itemDrop != null && itemDrop.GetComponent<Rigidbody>() != null)
                        itemDrop.GetComponent<Rigidbody>().AddForce(force);
                    break;
                case null:
                    throw new NullReferenceException();
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (index < 4)
            {
                //use item from last matching slot
                item = _items.Last(i => i != null && i.Item == _items[index]?.Item);
                item.Count--;
                //delete last matching slot when it runs out
                if (item.Count <= 0)
                    for (var lastMatch = _items.Length - 1; lastMatch >= 0; lastMatch--)
                        if (_items[lastMatch] != null && _items[lastMatch].Item == item.Item)
                        {
                            _items[lastMatch] = null;
                            break;
                        }
                //delete quickitem if there is no matching storage anymore
                if (_items.Skip(4).All(i => i == null || i.Item != _items[index].Item))
                    _items[index] = null;
            }
            else
            {
                item.Count--;
                if (item.Count <= 0)
                    _items[index] = null;
            }
            InventoryUi.UpdateInventory(this);
        }

        public void SwapSlots(int toIndex, int fromIndex)
        {
            var temp = _items[toIndex];
            _items[toIndex] = _items[fromIndex];
            _items[fromIndex] = temp;
        }

        [UsedImplicitly]
        private void Start()
        {
            _items = new InventoryItem[16];
        }

        [UsedImplicitly]
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<Rigidbody>() != null &&
                collision.gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude > 25) return;
                var pickup = collision.gameObject.GetComponent<Pickup>();
            if (pickup == null) return;
            for (var i = 4; i < _items.Length; i++)
                if (_items[i] != null && _items[i].Item == pickup.Item && _items[i].Count < SlotCapacity)
                {
                    _items[i].Count++;
                    Destroy(collision.gameObject);
                    if (InventoryUi != null)
                        InventoryUi.UpdateInventory(this);
                    return;
                }
                else if (_items[i] == null)
                {
                    _items[i] = new InventoryItem(pickup.Item, 1);
                    Destroy(collision.gameObject);
                    if (InventoryUi != null)
                        InventoryUi.UpdateInventory(this);
                    return;
                }
            //TODO: notify inventory full
        }

        public IEnumerator<InventoryItem> GetEnumerator()
        {
            return ((IEnumerable<InventoryItem>) _items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
}
