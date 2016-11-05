using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Inventory {
    public class InventorySlotUi : MonoBehaviour, IPointerClickHandler, IDragHandler, IDropHandler, IEndDragHandler,
        IBeginDragHandler {
        public GameObject Background;
        public InventoryUi InventoryUiScript;
        public Text ItemCount;
        public Text ItemName;
        public int Slot;
        public GameObject PreviewImage { get; set; }

        public void OnBeginDrag(PointerEventData eventData) {
            if (InventoryUiScript != null)
                InventoryUiScript.OnInventoryBeginDrag(this);
        }

        public void OnDrag(PointerEventData eventData) {
            if (InventoryUiScript != null)
                InventoryUiScript.OnInventoryDrag(this);
        }

        public void OnDrop(PointerEventData eventData) {
            if (InventoryUiScript != null)
                InventoryUiScript.OnInventoryDrop(this);
        }

        public void OnEndDrag(PointerEventData eventData) {
            if (InventoryUiScript != null)
                InventoryUiScript.OnInventoryEndDrag(this);
        }

        //Forward events to parent
        public void OnPointerClick(PointerEventData eventData) {
            if (InventoryUiScript != null)
                InventoryUiScript.OnInventoryClick(this);
        }

        public void UpdateUi(InventoryItem item) {
            Destroy(PreviewImage);
            if (ItemName != null)
                ItemName.text = "";
            if (ItemCount != null)
                ItemCount.text = "";
            if (item == null) return;

            PreviewImage = Instantiate(Background, transform) as GameObject;
            if (PreviewImage == null) return;
            //Scale sprite to fit inside background
            PreviewImage.transform.localScale = new Vector3(.8f, .8f, .8f);
            var image = PreviewImage.GetComponent<Image>();
            if (image == null) return;
            image.sprite = Resources.Load<Sprite>(Item.GetSpriteResource(item.Id));
            //Reset the color, the base object will be colorized
            image.color = Color.white;
            //The base object is sliced, reset this
            image.type = Image.Type.Simple;
            //Disable raycasting to prevent the drop event from misfiring
            image.raycastTarget = false;
            if (ItemName == null) return;
            //Mind the render order
            ItemName.transform.SetAsLastSibling();
            ItemName.text = Item.GetName(item.Id);
            if (ItemCount == null) return;
            ItemCount.transform.SetAsLastSibling();
            ItemCount.text = item.Count.ToString();
        }
    }
}