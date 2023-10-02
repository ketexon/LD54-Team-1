using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class PlaceableCost
{
    public int Energy;
    public int Metal;

    public string ToString(DropSettingsSO dropSettings)
    {
        StringBuilder sb = new();
        if(Energy > 0)
        {
            sb.AppendFormat("{0}<sprite={1}>", Energy, dropSettings.EnergySpriteIndex);
        }
        if (Metal > 0)
        {
            sb.AppendFormat("{0}<sprite={1}>", Metal, dropSettings.MetalSpriteIndex);
        }
        return sb.ToString();
    }
}

[CreateAssetMenu(fileName = "Placeable", menuName = "Placeables/Placeable", order = 0)]
public class PlaceableSO : Tile
{
    [SerializeField] public string Name;
    [SerializeField] public PlaceableCost Cost;
    [SerializeField] public Sprite ShopSprite;
    [SerializeField] public GameObject SpritePrefab;

    public enum PlaceableType
    {
        Land, Tower, Plant
    }

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
