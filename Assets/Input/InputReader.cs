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
    public System.Action<Vector2> MouseMoveEvent;
    public System.Action<float> ScrollEvent;
    public System.Action<bool> MMBEvent;
    public System.Action SpawnWaveEvent;

    void OnEnable()
    {
        Controls = new Controls();
        Controls.Gameplay.AddCallbacks(this);
        Controls.Gameplay.Enable();
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
        PointEvent?.Invoke(ctx.ReadValue<Vector2>());
    }

    public void OnMMB(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed)
        {
            MMBEvent?.Invoke(ctx.ReadValueAsButton());
        }
    }

    public void OnMouseMove(InputAction.CallbackContext context)
    {
        MouseMoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnScroll(InputAction.CallbackContext context)
    {
        // divide by 120 b/c one tick of scroll is 120f
        ScrollEvent?.Invoke(context.ReadValue<float>() / 120f);
    }

    public void OnSpawnWave(InputAction.CallbackContext context)
    {
        if (context.performed) { 
            SpawnWaveEvent?.Invoke();
        }
    }
}
