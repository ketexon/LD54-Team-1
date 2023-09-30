using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Animator))]
public class TextIndicator : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    public string Text { set { text.text = value; } }

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
}
