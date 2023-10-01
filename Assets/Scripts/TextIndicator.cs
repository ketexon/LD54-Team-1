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

    bool dontDestroy = false;

    void Reset()
    {
        text = GetComponentInChildren<TMP_Text>();
    }

    public void DebugDontDestroy()
    {
        dontDestroy = true;
    }

    // Used by AnimationController
    void DropAnimationFinishEvent()
    {
        if (!dontDestroy)
        {
            Destroy(gameObject);
        }
    }
}
