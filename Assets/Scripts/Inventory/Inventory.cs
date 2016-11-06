using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Inventory.Items;
using Assets.Scripts.Io;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using UnityEngine;
using ItemId = Assets.Scripts.Inventory.Items.Item.ItemId;

namespace Assets.Scripts.Inventory {
    public class Inventory : MonoBehaviour, IEnumerable<Item>, ISerializableScript {
        private Item[] _items;
        public InventoryUi InventoryUi;
        public int SlotCapacity;
        public LivingEntity.LivingEntity User;
        public int SelectedSlot { get; set; }

        public Item this[int index] {
            get {
                if (_items[index] == null) return null;
                if (index < 4)
                    _items[index].Count = ItemCount(_items[index].Id);
                return _items[index];
            }
            set {
                _items[index] = value;
                if (value != null && index < 4)
                    _items[index].Count = ItemCount(_items[index].Id);
            }
        }

        public IEnumerator<Item> GetEnumerator() => ((IEnumerable<Item>) _items).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();

        public int ItemCount(ItemId id) =>
            _items.Skip(4)
                .Where(item => item != null && item.Id == id)
                .Sum(item => item.Count);

        //TODO Testing
        public void Condense() {
            for (var none = 4; none < _items.Length; none++) {
                if (_items[none] != null) continue;
                for (var filled = none + 1; filled < _items.Length; filled++)
                    if (_items[filled] != null) {
                        SwapSlots(none, filled);
                        break;
                    }
            }
        }

        public void UseItem(int index) {
            var forward = transform.forward;
            forward.y += 1;
            var force = transform.forward;
            GameObject itemDrop;
            if (User == null) return;
            var item = _items[index];
            if (item == null) return;
            switch (item.Id) {
                case ItemId.Health:
                    User.Health += 10;
                    break;
                case ItemId.Health2:
                    User.Health += 30;
                    break;
                case ItemId.Health3:
                    User.Health = 100;
                    break;
                case ItemId.Shot1:
                    force.Scale(new Vector3(500, 1, 500));
                    itemDrop =
                        Instantiate(Resources.Load<GameObject>(item.PrefabString),
                            transform.position + forward, transform.rotation) as GameObject;
                    if (itemDrop != null && itemDrop.GetComponent<Rigidbody>() != null)
                        itemDrop.GetComponent<Rigidbody>().AddForce(force);
                    break;
                case ItemId.Shot2:
                    force.Scale(new Vector3(1000, 1, 1000));
                    itemDrop =
                        Instantiate(Resources.Load<GameObject>(item.PrefabString),
                            transform.position + forward, transform.rotation) as GameObject;
                    if (itemDrop != null && itemDrop.GetComponent<Rigidbody>() != null)
                        itemDrop.GetComponent<Rigidbody>().AddForce(force);
                    break;
                case ItemId.Shot3:
                    force.Scale(new Vector3(1500, 1, 1500));
                    itemDrop =
                        Instantiate(Resources.Load<GameObject>(item.PrefabString),
                            transform.position + forward, transform.rotation) as GameObject;
                    if (itemDrop != null && itemDrop.GetComponent<Rigidbody>() != null)
                        itemDrop.GetComponent<Rigidbody>().AddForce(force);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (index < 4) {
                //use item from last matching slot
                item = _items.Last(i => i != null && i.Id == _items[index]?.Id);
                item.Count--;
                //delete last matching slot when it runs out
                if (item.Count <= 0)
                    for (var lastMatch = _items.Length - 1; lastMatch >= 0; lastMatch--)
                        if (_items[lastMatch] != null && _items[lastMatch].Id == item.Id) {
                            _items[lastMatch] = null;
                            break;
                        }
                //delete quickitem if there is no matching storage anymore
                if (_items.Skip(4).All(i => i == null || i.Id != _items[index].Id))
                    _items[index] = null;
            }
            else {
                item.Count--;
                if (item.Count <= 0)
                    _items[index] = null;
            }
            InventoryUi.UpdateInventory(this);
        }

        public void SwapSlots(int toIndex, int fromIndex) {
            var temp = _items[toIndex];
            _items[toIndex] = _items[fromIndex];
            _items[fromIndex] = temp;
        }

        [UsedImplicitly]
        private void Start() {
            _items = new Item[16];
        }

        [UsedImplicitly]
        private void OnCollisionEnter(Collision collision) {
            if (collision.gameObject.GetComponent<Rigidbody>() != null &&
                collision.gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude > 25) return;
            var pickup = collision.gameObject.GetComponent<Pickup>();
            if (pickup == null) return;
            for (var i = 4; i < _items.Length; i++)
                if (_items[i] != null && _items[i].Id == pickup.Item && _items[i].Count < SlotCapacity) {
                    _items[i].Count++;
                    Destroy(collision.gameObject);
                    if (InventoryUi != null)
                        InventoryUi.UpdateInventory(this);
                    return;
                }
                else if (_items[i] == null) {
                    _items[i] = Item.FromId(pickup.Item);
                    Destroy(collision.gameObject);
                    if (InventoryUi != null)
                        InventoryUi.UpdateInventory(this);
                    return;
                }
            //TODO: notify inventory full
        }

        public JToken ToJson()
        {
            throw new NotImplementedException();
        }
    }
}