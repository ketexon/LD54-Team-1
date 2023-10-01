using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Placeable", menuName = "Placeables/Placeable", order = 0)]
public class PlaceableSO : ScriptableObject
{
    public enum PlaceableType
    {
        Land, Tower, Plant
    }

    public GameObject prefab;
}
