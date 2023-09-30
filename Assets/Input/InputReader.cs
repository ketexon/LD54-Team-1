using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Input Reader", fileName = "InputReader")]
public class InputReader : ScriptableObject, Controls.IGameplayActions
{
    public Controls Controls { get; private set; }

    public System.Action<bool> ClickEvent;
    public System.Action<Vector2> PointEvent;

    void OnEnable()
    {
        Controls = new Controls();
    }

    public void OnClick(InputAction.CallbackContext ctx)
    {
        // only call when started or ended
        if(!ctx.performed)
        {
            ClickEvent?.Invoke(ctx.ReadValueAsButton());
        }
    }

    public void OnPoint(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed)
        {
            PointEvent?.Invoke(ctx.ReadValue<Vector2>());
        }
    }
}
