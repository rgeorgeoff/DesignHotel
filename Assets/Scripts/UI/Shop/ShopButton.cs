using UnityEngine;
using System.Collections;
using Models;
using UI.Shop;
using UnityEngine.UI;

//These 
public class ShopButton : MonoBehaviour
{
    public Button buttonComponent;
    public Text nameLabel;
    public Image iconImage;
    public Text priceText;
    public delegate void ClickAction(PlaceableDefaultInfo placeableDefaultInfo);
    public event ClickAction OnClicked;
    
    private string itemPath;
    private ShopScrollUI scrollList;
    private PlaceableDefaultInfo placeableDefaultInfo;
    //private Color c = new Color(0.39f, 0.64f, 1f);

    // Use this for initialization
    void Start()
    {
        buttonComponent.onClick.AddListener(HandleClick);
    }

    public void Init(PlaceableDefaultInfo placeableDefaultInfo)
    {
        nameLabel.text = placeableDefaultInfo.Name;
        iconImage.sprite = placeableDefaultInfo.ShopSprite;
        this.placeableDefaultInfo = placeableDefaultInfo;
    }

    private void HandleClick()
    {
        OnClicked.Invoke(placeableDefaultInfo);
    }

    //some other button functions 
}