using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class ShopItem : MonoBehaviour
{
    [SerializeField] DropSettingsSO dropSettings;
    [SerializeField] TMP_Text nameText;
    [SerializeField] CanvasGroup countCanvasGroup;
    [SerializeField] TMP_Text countText;
    [SerializeField] TMP_Text costText;
    [SerializeField] Image sprite;
    [SerializeField] AspectRatioFitter spriteFitter;

    Button button;
    FarmingManager manager = null;
    [SerializeField] PlaceableSO placeable = null;

    public void Initialize(FarmingManager manager, PlaceableSO placeable)
    {
        this.manager = manager;
        this.placeable = placeable;

        nameText.text = placeable.Name;
        ResourceManager.Instance.ResourcesChanged += OnResourcesChanged;
        countCanvasGroup.alpha = placeable is PlantSO ? 1 : 0;
        costText.text = placeable.Cost.ToString(dropSettings);

        sprite.sprite = placeable.ShopSprite;
        float aspect = placeable.ShopSprite.rect.width / placeable.ShopSprite.rect.height;
        spriteFitter.aspectRatio = aspect;

        button.interactable = ResourceManager.Instance.CanAfford(placeable);
    }

    void Awake()
    {
        button = GetComponent<Button>();
    }

    void Start()
    {
        button.onClick.AddListener(SelectShopItem);
    }

    public void OnResourcesChanged()
    {
        if(placeable is PlantSO plant)
        {
            int seeds = ResourceManager.Instance.Seeds.GetValueOrDefault(plant);
            countText.text = seeds.ToString();
            button.interactable = seeds > 0;
        }
        button.interactable = ResourceManager.Instance.CanAfford(placeable);
    }

    public void SelectShopItem()
    {
        manager.SetSelection(placeable);
    }
}
