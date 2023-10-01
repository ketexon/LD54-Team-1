using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    public System.Action<int> MetalChanged;
    public System.Action<int> EnergyChanged;
    public System.Action<PlantSO, int> SeedCountChanged;

    int metal_ = 0;
    public int Metal
    {
        get => metal_;
        set
        {
            metal_ = value;
            MetalChanged?.Invoke(Metal);
        }
    }

    int energy_ = 0;
    public int Energy
    {
        get => energy_;
        set
        {
            energy_ = value;
            EnergyChanged?.Invoke(Energy);
        }
    }

    Dictionary<PlantSO, int> seeds_ = new();
    public IReadOnlyDictionary<PlantSO, int> Seeds => seeds_;

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddDrop(DropSO drop)
    {
        Energy += drop.Energy;
        Metal += drop.Metal;
        if(drop.Seed != null)
        {
            AddSeed(drop.Seed);
        }
    }

    public void AddSeed(PlantSO seed, int count = 1)
    {
        seeds_[seed] += count;
        SeedCountChanged?.Invoke(seed, Seeds[seed]);
    }

    public void UseSeed(PlantSO seed, int count = 1)
    {
        AddSeed(seed, -count);
    }
}
