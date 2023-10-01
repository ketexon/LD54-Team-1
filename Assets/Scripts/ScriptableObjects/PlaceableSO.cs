using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Placeable", menuName = "Placeables/Placeable", order = 0)]
public class PlaceableSO : Tile
{
    public enum PlaceableType
    {
        Land, Tower, Plant
    }

    // public GameObject prefab;
    
    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {   
        tileData.sprite = this.sprite;
        tileData.color = this.color;
        tileData.transform = this.transform;
        tileData.gameObject = this.gameObject;
        tileData.flags = this.flags;
        tileData.colliderType = this.colliderType;
    }
}
