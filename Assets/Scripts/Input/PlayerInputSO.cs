using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

[CreateAssetMenu(fileName = "PlayerInputSO", menuName = "ScriptableObjects/PlayerInputSO", order = 1)]
public class PlayerInputSO : ScriptableObject, IPlayerInputProvider
{
    public PlayerInput input { get; private set; }

    public ButtonAction Jump { get { return jumpAction; } }

    public ButtonAction Dash => throw new System.NotImplementedException();

    public ButtonAction Slide {get { return slideAction; } }

    public ButtonAction Primary { get { return primAction; } }

    public ButtonAction Secondary => throw new System.NotImplementedException();


    /* TODO: Switch to using these button actions to switch weapons
    /*public ButtonAction SwitchToPistol { get { return switchToPistol; } }

    public ButtonAction SwitchToShotgun { get { return switchToShotgun; } }*/
    public static InputAction switch2Pistol, switch2Shotgun;

    private ButtonAction jumpAction, dashAction, slideAction, primAction, secAction, switchToPistol, switchToShotgun;

    [SerializeField] private float lookSensitivity;

    public void OnEnable()
    {
        input = new PlayerInput();
        input.Enable();

        jumpAction = new ButtonAction(input.Game.Jump);
        dashAction = new ButtonAction(input.Game.Dash);
        primAction = new ButtonAction(input.Game.Primary);
        secAction = new ButtonAction(input.Game.Secondary);
        slideAction = new ButtonAction(input.Game.Slide);


        switchToPistol = new ButtonAction(input.Game.SwitchToPistol);
        switchToShotgun = new ButtonAction(input.Game.SwitchToShotgun);


        // TODO: Switch to using button actions 
        switch2Pistol = input.Game.SwitchToPistol;
        switch2Shotgun = input.Game.SwitchToShotgun;
    }

    public PlayerInputState GetState()
    {
        return new PlayerInputState(input.Game.Move.ReadValue<Vector2>(), input.Game.Look.ReadValue<Vector2>() * lookSensitivity);
    }
}