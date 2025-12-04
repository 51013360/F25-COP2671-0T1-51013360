using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text countText;

    public void SetItem(Sprite itemIcon, int count)
    {
        icon.sprite = itemIcon;
        icon.enabled = true;

        countText.text = count > 1 ? count.ToString() : "";
    }

    public void Clear()
    {
        icon.sprite = null;
        icon.enabled = false;
        countText.text = "";
    }
}