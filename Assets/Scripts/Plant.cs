using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Placeable
{
    [SerializeField] DropSettingsSO dropSettings;

    public PlantSO Stats => (PlantSO)associatedSO;

    int wavesUntilHarvest;

    override protected void Awake()
    {
        base.Awake();
        wavesUntilHarvest = Stats.HarvestCycleLength;

        Instantiate(Stats.SpritePrefab, transform.position, Quaternion.identity, transform);
    }

    override protected void Start()
    {
        base.Start();
        GameController.gameController.WaveEndEvent += OnWaveEnd;
    }

    void OnDestroy()
    {
        GameController.gameController.WaveEndEvent -= OnWaveEnd;
    }

    void OnWaveEnd()
    {
        wavesUntilHarvest -= 1;
        if(wavesUntilHarvest == 0)
        {
            Debug.Log("Harvest");
            var indicatorGO = Instantiate(textIndicatorPrefab, transform.position, Quaternion.identity);
            var indicator = indicatorGO.GetComponent<TextIndicator>();
            indicator.Text = Stats.HarvestDrop.IndicatorText(dropSettings);
            indicator.DebugDontDestroy();
            ResourceManager.Instance.AddDrop(Stats.HarvestDrop);
            wavesUntilHarvest += Stats.HarvestCycleLength;
        }
    }
    
    public override void Die()
    {

    }
    
    public override bool ValidatePlace(Vector3Int loc)
    {
        // FIXME: something w/ seeds idk
        return base.ValidatePlace(loc) 
            && GameController.gameController.HasFertileLand(loc)
            && !GameController.gameController.HasBuilding(loc);
    }

    public override void Place(Vector3Int loc, PlaceableSO placeable)
    {

    }
}
