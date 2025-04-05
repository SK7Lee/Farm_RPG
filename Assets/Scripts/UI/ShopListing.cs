using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ShopListing : MonoBehaviour
{
    public Image itemThumbnail;
    public Text nameText;
    public Text costText;
    
    ItemData itemData;

    public void Display(ItemData itemData)
    {
        this.itemData = itemData;
        itemThumbnail.sprite = itemData.thumbnail;
        nameText.text = itemData.name;
        costText.text = PlayerStats.CURRENCY + itemData.cost.ToString();
    }
}
