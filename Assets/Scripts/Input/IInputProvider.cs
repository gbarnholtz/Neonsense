using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IInputProvider: IMoveActionProvider, IAbilityActionProvider
{
    public InputState GetState();
}

public interface IMoveActionProvider
{
    public ButtonAction OnJump { get; }
    public ButtonAction OnSprint { get; }
}

public interface IUIActionProvider
{
    public ButtonAction OnOverlay { get; }
    public ButtonAction OnPause { get; }
}

public interface IAbilityActionProvider
{
    public List<ButtonAction> Abilities { get; }
}


public interface IInputModifier
{
    public InputState ModifyInput(InputState input);
}

public class ButtonAction {
    public event Action started;
    public event Action ended;

    public ButtonAction(InputAction inputAction) {
        inputAction.started += _ => started?.Invoke();
        inputAction.canceled += _ => ended?.Invoke();
    }

    public ButtonAction() {}

    public void InvokeStart() => started?.Invoke(); 
    public void InvokeEnd() => ended?.Invoke();

    public void InvokeComplete() {
        started?.Invoke();
        ended?.Invoke();
    }
}

[Serializable]
public struct InputState
{
    public Vector3 moveDirection;
    public bool shouldLookAtAim;
    public Vector3 aimPoint;
    public Vector3 lookEulers;

    public InputState(Vector2 moveInput, Vector2 lookInput)
    {
        moveDirection = moveInput;
        lookEulers = new Vector3(-lookInput.y, lookInput.x, 0);
        aimPoint = Vector3.zero;
        shouldLookAtAim = false;
    }
    public InputState(PlayerInputState playerInputState)
    {
        moveDirection = new Vector3(playerInputState.moveInput.x, 0, playerInputState.moveInput.y);
        lookEulers = new Vector3(-playerInputState.lookInput.y, playerInputState.lookInput.x, 0);
        aimPoint = Vector3.zero;
        shouldLookAtAim = false;
    }
}
