using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Placeable
{
    [SerializeField] public PlantSO Stats;
    [SerializeField] DropSettingsSO dropSettings;

    int wavesUntilHarvest;

    override protected void Awake()
    {
        base.Awake();
        wavesUntilHarvest = Stats.HarvestCycleLength;
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
}
