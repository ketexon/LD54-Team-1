using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantDatabase", menuName = "Plants/Plant Database", order = 0)]
public class PlaceableDatabaseSO : ScriptableObject
{
    public List<PlaceableSO> Placeables = new();
}
