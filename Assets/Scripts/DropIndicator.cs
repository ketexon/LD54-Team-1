using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Animator))]
public class DropIndicator : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] int metalSpriteIndex;
    [SerializeField] int energySpriteIndex;

    public DropSO Drop;

    Animator animator;

    void Reset()
    {
        text = GetComponentInChildren<TMP_Text>();
    }

    // Used by AnimationController
    void DropAnimationFinishEvent()
    {
        Destroy(gameObject);
    }

    void Start()
    {
        string text = "";
        if (Drop.Metal > 0)
        {
            text += $"+{Drop.Metal}<sprite index=\"{metalSpriteIndex}\">";
        }
        if(Drop.Seed is PlantSO plant)
        {
            text += text.Length > 0 ? "\n" : "";
            text += $"+<sprite index=\"{plant.SeedSpriteIndex}\">";
        }
        if (Drop.Energy > 0)
        {
            text += text.Length > 0 ? "\n" : "";
            text += $"+{Drop.Energy}<sprite index=\"{energySpriteIndex}\">";
        }

        this.text.text = text;
    }
}
