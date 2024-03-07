using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerInputProvider : IMoveActionProvider, IAttackActionProvider
{
    public PlayerInputState GetState();
}

public struct PlayerInputState
{
    public Vector2 moveInput, lookInput;
    public PlayerInputState(Vector2 moveInput, Vector2 lookInput)
    {
        this.moveInput = moveInput;
        this.lookInput = lookInput;
    }
}
