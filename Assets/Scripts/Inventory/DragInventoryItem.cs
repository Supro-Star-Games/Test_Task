
using UnityEngine;
using UnityEngine.EventSystems;


public class DragInventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector2 mouseOffset;
    private Vector2 originalPosition;
    private InventorySlot originalSlot;
    private EquipmentSlot originalESlot;

    public RectTransform MRectTransform { get; set; }
    public InventorySlot InventorySlot { get; set; }
    
    public virtual void Initialize()
    {
        MRectTransform = GetComponent<RectTransform>();
        originalPosition = InventorySlot.SRectTransform.anchoredPosition;
        originalSlot = InventorySlot;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit.collider.TryGetComponent(out InventorySlot slot))
        {
            originalSlot = slot;
            originalPosition = originalSlot.SRectTransform.anchoredPosition;
        }
        else if (hit.collider.TryGetComponent(out EquipmentSlot eslot))
        {
            originalESlot = eslot;
            originalPosition = eslot.SRectTransform.anchoredPosition;
        }

        mouseOffset = MRectTransform.anchoredPosition - eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        MRectTransform.anchoredPosition = eventData.position + mouseOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction);
        ;
        if (hits.Length == 0)
        {
            MRectTransform.anchoredPosition = originalPosition;
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
                        MRectTransform.anchoredPosition = originalPosition;
                    }
                    else
                    {
                        MRectTransform.anchoredPosition = hit.collider.GetComponent<RectTransform>().anchoredPosition;
                        originalPosition = slot.SRectTransform.anchoredPosition;
                        originalSlot.IsOccupied = false;
                        slot.IsOccupied = true;
                        InventorySlot = slot;
                        if (originalESlot != null)
                        {
                            originalESlot.RemoveItem();
                            originalESlot = null;
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
                        MRectTransform.anchoredPosition = originalPosition;
                    }
                }
                else
                {
                    MRectTransform.anchoredPosition = originalPosition;
                }
            }
        }
    }

    public void SwapPlaces(EquipmentSlot equipmentSlot, ClothInventoryItem clothItem)
    {
        
        if (equipmentSlot.CurrentItem != null)
        {
            clothItem.MRectTransform.anchoredPosition = equipmentSlot.SRectTransform.anchoredPosition;
            equipmentSlot.CurrentItem.MRectTransform.anchoredPosition = originalPosition;
        }
        else
        {
            clothItem.MRectTransform.anchoredPosition = equipmentSlot.SRectTransform.anchoredPosition;
        }

        originalSlot.IsOccupied = false;
    }
}