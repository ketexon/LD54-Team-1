using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlantBuff
{
    public float ShotIntervalMult = 1.0f;
    public ShotParams DeltaShotParams = ShotParams.Zero;

    public static PlantBuff operator+(PlantBuff a, PlantBuff b)
    {
        return new()
        {
            ShotIntervalMult = a.ShotIntervalMult * b.ShotIntervalMult,
            DeltaShotParams = new()
            {
                Damage = a.DeltaShotParams.Damage + b.DeltaShotParams.Damage,
                Speed = a.DeltaShotParams.Speed + b.DeltaShotParams.Speed,
                PeirceCap = a.DeltaShotParams.PeirceCap + b.DeltaShotParams.PeirceCap,
                Range = a.DeltaShotParams.Range + b.DeltaShotParams.Range,
            }
        };
    }

    public static PlantBuff operator -(PlantBuff a)
    {
        return new()
        {
            ShotIntervalMult = 1/a.ShotIntervalMult,
            DeltaShotParams = new()
            {
                Damage = a.DeltaShotParams.Damage,
                Speed = a.DeltaShotParams.Speed,
                PeirceCap = a.DeltaShotParams.PeirceCap,
                Range = a.DeltaShotParams.Range,
            }
        };
    }
}

[CreateAssetMenu(fileName = "Plant", menuName = "Placeables/Plant", order = 1)]
public class PlantSO : PlaceableSO
{
    public string Name;
    public int SeedSpriteIndex;
    public PlantBuff Buff;
    public int HarvestCycleLength;
    public DropSO HarvestDrop;
    public GameObject Prefab;
}
