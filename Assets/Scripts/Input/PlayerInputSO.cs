using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInputSO", menuName = "ScriptableObjects/PlayerInputSO", order = 1)]
public class PlayerInputSO : ScriptableObject, IPlayerInputProvider
{
    public PlayerInput input { get; private set; }

    public ButtonAction Jump { get { return jumpAction; } }

    public ButtonAction Dash => throw new System.NotImplementedException();

    public ButtonAction Slide => throw new System.NotImplementedException();

    public ButtonAction Primary => throw new System.NotImplementedException();

    public ButtonAction Secondary => throw new System.NotImplementedException();

    private ButtonAction jumpAction, dashAction, freeMouseAction, primAction, secAction;

    [SerializeField] private float lookSensitivity;

    public void OnEnable()
    {
        input = new PlayerInput();
        input.Enable();

        jumpAction = new ButtonAction(input.Game.Jump);
        dashAction = new ButtonAction(input.Game.Dash);
        primAction = new ButtonAction(input.Game.Primary);
        secAction = new ButtonAction(input.Game.Secondary);
    }

    public PlayerInputState GetState()
    {
        return new PlayerInputState(input.Game.Move.ReadValue<Vector2>(), input.Game.Look.ReadValue<Vector2>() * lookSensitivity);
    }
}