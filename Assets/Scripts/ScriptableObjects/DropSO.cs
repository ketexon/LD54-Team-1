using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Drop", menuName = "Drops/Drop", order = 1)]
public class DropSO : ScriptableObject
{
    public int Metal;
    public PlantSO Seed;
    public int Energy;
}
