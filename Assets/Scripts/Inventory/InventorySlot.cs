using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public RectTransform SRectTransform { get; private set; }
    public bool IsOccupied { get; set; }

    private void Awake()
    {
        SRectTransform = GetComponent<RectTransform>();
    }
}