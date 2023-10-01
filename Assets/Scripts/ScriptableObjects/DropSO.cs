using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Drop", menuName = "Drops/Drop", order = 1)]
public class DropSO : ScriptableObject
{
    public int Metal;
    public PlantSO Seed;
    public int Energy;

    public string IndicatorText(DropSettingsSO settings)
    {
        string text = "";
        if (Metal > 0)
        {
            text += $"+{Metal}<sprite index=\"{settings.MetalSpriteIndex}\">";
        }
        if (Seed is PlantSO plant)
        {
            text += text.Length > 0 ? "\n" : "";
            text += $"+<sprite index=\"{plant.SeedSpriteIndex}\">";
        }
        if (Energy > 0)
        {
            text += text.Length > 0 ? "\n" : "";
            text += $"<color=\"blue\">+{Energy}</color><sprite index=\"{settings.EnergySpriteIndex}\">";
        }
        return text;
    }
}
