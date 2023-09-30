using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Plant", menuName = "Plants/Plant", order = 1)]
public class PlantSO : ScriptableObject
{
    public string Name;
    public int SeedSpriteIndex;
}
