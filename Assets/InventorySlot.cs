using Assets;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IDragHandler, IDropHandler
{
    public int Slot;
    public UI uiScript;
    public GameObject Background;
    private Item _item;
    private GameObject _itemGameObject;
    public Item Item
    {
        get {return _item;}
        set
        {
            if (_itemGameObject != null)
                DestroyImmediate(_itemGameObject);
            _item = value;
            if (_item.UiSprite == null || Background == null) return;
            _itemGameObject = Instantiate(Background, transform) as GameObject;
            if (_itemGameObject == null) return;
            _itemGameObject.transform.localScale = new Vector3(.8f, .8f, .8f);
            _itemGameObject.transform.SetAsLastSibling();
            var image = _itemGameObject.GetComponent<Image>();
            if (image == null) return;
            image.sprite = _item.UiSprite;
            image.color = Color.white;
            image.type = Image.Type.Simple;
        }
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        if (uiScript != null)
            Item = uiScript.OnInventoryClick(Slot);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (uiScript != null)
            Item = uiScript.OnInventoryDrag(Slot);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (uiScript != null)
            Item = uiScript.OnInventoryDrop(Slot);
    }
}
