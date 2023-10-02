using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    [SerializeField] int startingMetal;
    [SerializeField] int startingEnergy;
    [SerializeField] List<PlantSO> startingSeeds;
    [SerializeField] List<TMP_Text> uiText;

    public System.Action ResourcesChanged;
    public System.Action<int> MetalChanged;
    public System.Action<int> EnergyChanged;
    public System.Action<PlantSO, int> SeedCountChanged;

    int metal_ = 0;
    public int Metal
    {
        get => metal_;
        set
        {
            var old = Metal;
            metal_ = value;
            if(old != Metal)
            {
                MetalChanged?.Invoke(Metal);
                ResourcesChanged?.Invoke();
            }
        }
    }

    int energy_ = 0;
    public int Energy
    {
        get => energy_;
        set
        {
            var old = Energy;
            energy_ = value;
            if (old != Energy)
            {
                EnergyChanged?.Invoke(Energy);
                ResourcesChanged?.Invoke();
            }
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

        Metal = startingMetal;
        Energy = startingEnergy;
        foreach(var seed in startingSeeds)
        {
            AddSeed(seed);
        }
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
        seeds_[seed] = seeds_.GetValueOrDefault(seed) + count;
        uiText[seed.SeedSpriteIndex].text = seeds_[seed].ToString();
        SeedCountChanged?.Invoke(seed, Seeds[seed]);
        ResourcesChanged?.Invoke();
    }

    public void UseSeed(PlantSO seed, int count = 1)
    {
        AddSeed(seed, -count);
    }

    public bool CanAfford(PlaceableSO placeable)
    {
        var cost = placeable.Cost;
        return Metal >= cost.Metal && Energy >= cost.Energy && (
            !(placeable is PlantSO plant) || Seeds.GetValueOrDefault(plant) > 0
        );
    }

    public void Buy(PlaceableSO placeable)
    {
        Metal -= placeable.Cost.Metal;
        Energy -= placeable.Cost.Energy;
        if(placeable is PlantSO plant)
        {
            UseSeed(plant);
        }
    }
}
