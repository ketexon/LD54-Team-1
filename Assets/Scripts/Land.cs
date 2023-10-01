using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Land : Placeable, IPointerDownHandler
{
    public enum LandStatus
    {
        Infertile,
        Fertile
    }

    [SerializeField] private PlaceableSO infertileSO;
    [SerializeField] private PlaceableSO fertileSO;

    [SerializeField] private Sprite infertileSprite;
    [SerializeField] private Sprite fertileSprite;

    [SerializeField] private LandStatus status;
    public LandStatus GetStatus() => status;

    public override void Die()
    {
        GameController.gameController.RemoveLandTile(this.transform.position);
        // Destroy(gameObject);
    }

    public override bool ValidatePlace(Vector3Int loc)
    {
        // TOOD: somethign with hammers idk
        return !GameController.gameController.HasLand(loc);
    }

    public override void UpdateResources()
    {
        // TOOD: somethign with hammers idk
    }

    public override void Place(Vector3Int loc, PlaceableSO placeable)
    {
        GameController.gameController.SetLand(loc, placeable);
    }

    public void TryFertilize()
    {
        if (energyCost <= ResourceManager.Instance.Energy)
        {
            ResourceManager.Instance.Energy -= energyCost;
            GameController.gameController.SetLand(GameController.gameController.GetGrid().WorldToCell(this.transform.position), fertileSO);
        }
    }

    public void OnPointerDown(PointerEventData e)
    {
        if (status == LandStatus.Infertile) TryFertilize();
    }
}
