using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantDatabase", menuName = "Plants/Plant Database", order = 0)]
public class PlantDatabaseSO : ScriptableObject
{
    public List<PlantSO> Plants = new();
}
