using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : Placeable
{
    public enum LandStatus
    {
        Infertile,
        Fertile
    }

    [SerializeField] private Sprite infertileSprite;
    [SerializeField] private Sprite fertileSprite;

    public LandStatus status;

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
}
