using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Inventory.Items;
using Assets.Scripts.Io;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

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

        public JToken ToJson(JsonSerializer serializer)
            =>
            new JArray(
                _items.Select(
                    item =>
                        item == null
                            ? null
                            : JObject.FromObject(item.ToSerializable(), serializer)));
        
        public void FromJson(JToken token) {
            var inventory = token as JArray;
            if (inventory == null) {
                Debug.LogError("Serialized inventory was not an array");
                return;
            }
            if (inventory.Count > _items.Length)
                Debug.LogWarning($"Serialized inventory capacity {inventory.Count} exceeds current inventory capacity");
            else if (inventory.Count < _items.Length)
                Debug.LogWarning(
                    $"Serialized inventory capacity {inventory.Count} is less than current inventory capacity");
            _items =
                inventory.Take(_items.Length)
                    .Select(
                        jItem =>
                            jItem == null
                                ? null
                                : JsonConvert.DeserializeObject<Item.Serializable>(jItem.ToString()))
                    .Select(serializable => serializable == null ? null : Item.FromSerializable(serializable))
                    .ToArray();
            InventoryUi.UpdateInventory(this);
        }

        public int ItemCount(Item.ItemId id) =>
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
            var item = _items[index];
            if (User == null || item == null) return;
            if (index < 4)
            {
                //use item from last matching slot
                item = _items.Last(i => i != null && i.Id == _items[index]?.Id);
                item.Count--;
                //delete last matching slot when it runs out
                if (item.Count <= 0)
                    for (var lastMatch = _items.Length - 1; lastMatch >= 0; lastMatch--)
                        if (_items[lastMatch] != null && _items[lastMatch].Id == item.Id)
                        {
                            _items[lastMatch] = null;
                            break;
                        }
                //delete quickitem if there is no matching storage anymore
                if (_items.Skip(4).All(i => i == null || i.Id != _items[index].Id))
                    _items[index] = null;
            }
            else
            {
                item.Count--;
                if (item.Count <= 0)
                    _items[index] = null;
            }
            InventoryUi.UpdateInventory(this);
            var heal = item as Heal;
            if (heal != null)
            {
                User.Health += heal.HealAmount;
                return;
            }
            var shot = item as Shot;
            if (shot == null) return;
            var itemDrop =
                Instantiate(Resources.Load<GameObject>(item.PrefabString),
                    transform.position + transform.forward + new Vector3(0, 1, 0), transform.rotation) as GameObject;
            itemDrop?.GetComponent<ProjectilePickup>()?.Activate(transform.forward);
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
    }
}