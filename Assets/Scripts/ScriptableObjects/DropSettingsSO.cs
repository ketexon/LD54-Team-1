using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropPool
{
    public float Weight;
    public List<DropSO> Drops;

    public DropSO GetRandomDrop()
    {
        return Drops.Count == 0 ? null : Drops[Random.Range(0, Drops.Count)];
    }
}

[CreateAssetMenu(fileName = "DropSettings", menuName = "Drops/DropSettings", order = 0)]
public class DropSettingsSO : ScriptableObject
{
    public int MetalSpriteIndex;
    public int EnergySpriteIndex;
    public List<DropPool> Pools;

    // cached value (we assume Pools is immutable after enable)
    float? netWeight_ = null;
    public float NetWeight { 
        get
        {
            if (netWeight_ != null) return netWeight_.Value;
            netWeight_ = 0;
            foreach(DropPool pool in Pools)
            {
                netWeight_ += pool.Weight;
            }
            return NetWeight;
        }
    }


    public DropPool GetRandomDropPool()
    {
        float val = Random.Range(0, NetWeight);
        foreach (DropPool pool in Pools)
        {
            val -= pool.Weight;
            if (val <= 0) return pool;
        }
        return null;
    }

    /// <summary>
    /// Gets random drop from drop pools, weighted by drop pool weight. Drop can be null.
    /// </summary>
    /// <seealso cref="DropPool.GetRandomDrop"/>
    /// <seealso cref="GetRandomDropPool"/>
    public DropSO GetRandomDrop()
    {
        DropPool pool = GetRandomDropPool();
        if (pool != null) return pool.GetRandomDrop();
        return null;
    }
}
