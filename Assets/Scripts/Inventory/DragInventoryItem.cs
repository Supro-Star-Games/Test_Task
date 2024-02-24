using UnityEngine;
using UnityEngine.EventSystems;


public class DragInventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector2 _mouseOffset;
    private Vector2 _originalPosition;
    private InventorySlot _originalSlot;
    private EquipmentSlot _originalESlot;

    private RectTransform MRectTransform { get; set; }
    public InventorySlot InventorySlot { get; set; }

    public virtual void Initialize()
    {
        MRectTransform = GetComponent<RectTransform>();
        _originalPosition = InventorySlot.SRectTransform.anchoredPosition;
        _originalSlot = InventorySlot;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit.collider.TryGetComponent(out InventorySlot slot))
        {
            _originalSlot = slot;
            _originalPosition = _originalSlot.SRectTransform.anchoredPosition;
        }
        else if (hit.collider.TryGetComponent(out EquipmentSlot eslot))
        {
            _originalESlot = eslot;
            _originalPosition = eslot.SRectTransform.anchoredPosition;
        }

        _mouseOffset = MRectTransform.anchoredPosition - eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        MRectTransform.anchoredPosition = eventData.position + _mouseOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction);
        ;
        if (hits.Length == 0)
        {
            MRectTransform.anchoredPosition = _originalPosition;
            return;
        }

        foreach (var hit in hits)
        {
            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent(out InventorySlot slot))
                {
                    if (slot.IsOccupied)
                    {
                        MRectTransform.anchoredPosition = _originalPosition;
                    }
                    else
                    {
                        MRectTransform.anchoredPosition = hit.collider.GetComponent<RectTransform>().anchoredPosition;
                        _originalPosition = slot.SRectTransform.anchoredPosition;
                        _originalSlot.IsOccupied = false;
                        slot.IsOccupied = true;
                        InventorySlot = slot;
                        if (_originalESlot != null)
                        {
                            _originalESlot.RemoveItem();
                            _originalESlot = null;
                        }
                    }
                }

                if (hit.collider.TryGetComponent(out EquipmentSlot equipmentSlot) && MRectTransform.TryGetComponent(out ClothInventoryItem clothItem))
                {
                    if (equipmentSlot.SlotType == clothItem.DefendType)
                    {
                        SwapPlaces(equipmentSlot, clothItem);
                        equipmentSlot.ChangeItem(clothItem);
                    }
                    else
                    {
                        MRectTransform.anchoredPosition = _originalPosition;
                    }
                }
                else
                {
                    MRectTransform.anchoredPosition = _originalPosition;
                }
            }
        }
    }

    public void SwapPlaces(EquipmentSlot equipmentSlot, ClothInventoryItem clothItem)
    {
        if (equipmentSlot.CurrentItem != null)
        {
            clothItem.MRectTransform.anchoredPosition = equipmentSlot.SRectTransform.anchoredPosition;
            equipmentSlot.CurrentItem.MRectTransform.anchoredPosition = _originalPosition;
            (_originalPosition, equipmentSlot.CurrentItem._originalPosition) = (equipmentSlot.CurrentItem._originalPosition, _originalPosition);
        }
        else
        {
            clothItem.MRectTransform.anchoredPosition = equipmentSlot.SRectTransform.anchoredPosition;
            _originalSlot.IsOccupied = false;
        }
    }
}