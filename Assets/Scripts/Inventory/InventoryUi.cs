using System.Linq;
using Assets.Scripts.Inventory.Items;
using Assets.Scripts.Io;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Inventory {
    [RequireComponent(typeof(RectTransform))]
    public class InventoryUi : MonoBehaviour {
        private Vector3 _activeInventoryOffset;
        private Item _cursorItem;
        private GameObject _cursorObject;
        private InventorySlotUi _cursorSlot;
        private bool _open;
        private InventorySlotUi _selectedSlot;
        public Inventory Inventory;
        public Vector3 InventoryOffset;

        public bool Open {
            get { return _open; }
            set {
                _open = value;
                if (_open) {
                    _activeInventoryOffset = InventoryOffset*
                                             (Inventory.Skip(4).Take(6).Count(item => item != null) == 6
                                              //first row of inventory is full
                                              || Inventory.Skip(10).Count(item => item != null) > 0
                                                 //second row contains anything
                                                 ? 1
                                                 : 0.5f);
                    GetComponent<RectTransform>().transform.localPosition += _activeInventoryOffset;
                }
                else
                    GetComponent<RectTransform>().transform.localPosition -= _activeInventoryOffset;
            }
        }

        public void UpdateInventory(Inventory inventory) {
            foreach (var inventorySlot in GetComponentsInChildren<InventorySlotUi>()) {
                var item = inventory[inventorySlot.Slot];
                inventorySlot.UpdateUi(item);
            }
        }

        [UsedImplicitly]
        private void Update() {
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

        public void OnInventoryClick(InventorySlotUi slot) {
            if (Inventory == null || slot == null) return;
            Inventory.SelectedSlot = slot.Slot;
            Image previousSelectedImage;
            if (_selectedSlot != null && _selectedSlot.Background != null &&
                (previousSelectedImage = _selectedSlot.Background.GetComponent<Image>()) != null)
                previousSelectedImage.color = new Color(1, 1, 1, 100f/256f); //default color
            Image selectedImage;
            if (slot.Background != null && (selectedImage = slot.Background.GetComponent<Image>()) != null)
                selectedImage.color = new Color(.5f, .5f, 1, 100f/256f);
            _selectedSlot = slot;
            Inventory.UseItem(_selectedSlot.Slot);
            Debug.Log($"click: #{slot.Slot} {slot.name}");
        }

        public void OnInventoryBeginDrag(InventorySlotUi slot) {
            if (slot == null) return;
            _cursorItem = Inventory[slot.Slot];
            _cursorSlot = slot;
            //Copy slot
            _cursorObject = (GameObject) Instantiate(slot.gameObject, slot.transform);
            //Delete background and ui script
            Destroy(_cursorObject.GetComponent<InventorySlotUi>().Background);
            Destroy(_cursorObject.GetComponent<InventorySlotUi>());
            //float above all other ui elements
            _cursorObject.transform.SetParent(transform);
            _cursorObject.transform.SetAsLastSibling();
            //Delete previous item data
            Destroy(slot.PreviewImage);
            if (slot.ItemName != null)
                slot.ItemName.text = "";
            if (slot.ItemCount != null)
                slot.ItemCount.text = "";
            Inventory[slot.Slot] = null;
            Debug.Log($"begindrag: #{slot.Slot} {slot.name}");
        }

        public void OnInventoryDrag(InventorySlotUi slot) {
            if (slot == null || _cursorObject == null) return;
            var screenPoint = Input.mousePosition;
            screenPoint.z = 1.8f; //distance of the plane from the camera
            _cursorObject.transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
            Debug.Log($"drag: {_cursorObject.transform.position}");
        }

        public void OnInventoryDrop(InventorySlotUi slot) {
            if (slot == null) return;
            if (_cursorSlot != null)
                if (slot.Slot < 4 && _cursorSlot.Slot >= 4) {
                    //Clone item back to storage
                    Inventory[_cursorSlot.Slot] = _cursorItem.DeepCopy();
                    _cursorSlot.UpdateUi(Inventory[_cursorSlot.Slot]);
                }
                else {
                    Inventory.SwapSlots(_cursorSlot.Slot, slot.Slot);
                    _cursorSlot.UpdateUi(Inventory[_cursorSlot.Slot]);
                }
            Inventory[slot.Slot] = _cursorItem;
            slot.UpdateUi(Inventory[slot.Slot]);
            Debug.Log($"drop: #{slot.Slot} {slot.name}");
            _cursorItem = null;
        }

        //Relies on OnDrop (when valid) being called before OnEndDrag
        public void OnInventoryEndDrag(InventorySlotUi slot) {
            if (slot == null) return;
            if (_cursorItem != null) {
                //If quickbar is dropped, drop all items of that type
                if (slot.Slot < 4)
                    for (var i = 4; i < Inventory.Count(); i++)
                        if (Inventory[i] != null && Inventory[i].Id == _cursorItem.Id)
                            Inventory[i] = null;
                var forward = Inventory.transform.forward;
                forward.y += 1;
                var force = forward;
                force.Scale(new Vector3(50, 1, 50));
                for (var i = 0; i < _cursorItem.Count; i++) {
                    var itemDrop =
                        Instantiate(Resources.Load<GameObject>(_cursorItem.PrefabString),
                            Inventory.transform.position + forward, Inventory.transform.rotation) as GameObject;
                    if (itemDrop == null) continue;
                    if (itemDrop.GetComponent<Rigidbody>() != null)
                        itemDrop.GetComponent<Rigidbody>().AddForce(force);
                    if (itemDrop.GetComponent<ProjectilePickup>() != null)
                        //Render projectiles inert
                        itemDrop.GetComponent<ProjectilePickup>().VolatileTime = 0;
                }
                _cursorItem = null;
            }
            _cursorSlot = null;
            DestroyImmediate(_cursorObject);
            UpdateInventory(Inventory);
            Debug.Log($"enddrag: #{slot.Slot} {slot.name}");
        }
    }
}