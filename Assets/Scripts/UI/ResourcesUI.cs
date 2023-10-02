using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourcesUI : MonoBehaviour
{
    [SerializeField] DropSettingsSO dropSettings;
    [SerializeField] TMP_Text energyText;
    [SerializeField] TMP_Text metalText;

    string EnergyText(int energy) => $"{energy}<sprite={dropSettings.EnergySpriteIndex}>";
    string MetalText(int metal) => $"{metal}<sprite={dropSettings.MetalSpriteIndex}>";

    void Reset()
    {
        foreach(var text in GetComponentsInChildren<TMP_Text>())
        {
            var name = text.gameObject.name.ToLower();
            if (name.Contains("metal"))
            {
                metalText = text;
            }
            else if (name.Contains("energy"))
            {
                energyText = text;
            }
        }
    }

    void Start()
    {
        ResourceManager.Instance.EnergyChanged += DrawEnergyText;
        ResourceManager.Instance.MetalChanged += DrawMetalText;

        // The above events wont be called for initial values
        DrawEnergyText(ResourceManager.Instance.Energy);
        DrawMetalText(ResourceManager.Instance.Metal);
    }

    void DrawEnergyText(int energy)
    {
        energyText.text = EnergyText(energy);
    }

    void DrawMetalText(int metal)
    {
        metalText.text = MetalText(metal);
    }
}
