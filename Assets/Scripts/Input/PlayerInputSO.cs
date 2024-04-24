using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

//[CreateAssetMenu(fileName = "PlayerInputSO", menuName = "ScriptableObjects/PlayerInputSO", order = 1)]
public class PlayerInputSO : MonoBehaviour, IPlayerInputProvider
{
    [field:SerializeField]
    public InputActionAsset input { get; private set; }

    public ButtonAction Jump { get { return jumpAction; } }

    public ButtonAction Dash => throw new System.NotImplementedException();

    public ButtonAction Slide {get { return slideAction; } }

    public ButtonAction Primary { get { return primAction; } }

    public ButtonAction Secondary => throw new System.NotImplementedException();


    /* TODO: Switch to using these button actions to switch weapons
    /*public ButtonAction SwitchToPistol { get { return switchToPistol; } }

    public ButtonAction SwitchToShotgun { get { return switchToShotgun; } }*/
    public static InputAction switch2Pistol, switch2Shotgun, switch2Rifle, switch2SMG, placeCharge, reload;

    private ButtonAction jumpAction, dashAction, slideAction, primAction, secAction, switchToPistol, switchToShotgun;

    [SerializeField] private float lookSensitivity;

    public void Awake()
    {
        //input = new PlayerInput();
        
        input.Enable();

        jumpAction = new ButtonAction( input.FindAction("Jump"));
        dashAction = new ButtonAction( input.FindAction("Dash"));
        primAction = new ButtonAction( input.FindAction("Primary"));
        secAction = new ButtonAction(  input.FindAction("Secondary"));
        slideAction = new ButtonAction(input.FindAction("Slide"));


        switchToPistol = new ButtonAction(input. FindAction("SwitchToPistol"));
        switchToShotgun = new ButtonAction(input.FindAction("SwitchToShotgun"));


        // TODO: Switch to using button actions 
        switch2Pistol =  input.FindAction("SwitchToPistol");
        switch2Shotgun = input.FindAction("SwitchToShotgun");
        switch2Rifle =   input.FindAction("SwitchToRifle");
        switch2SMG =     input.FindAction("SwitchToSmg");

        reload =         input.FindAction("Reload");
    }

    public PlayerInputState GetState()
    {
        return new PlayerInputState(input.FindAction("Move").ReadValue<Vector2>(), input.FindAction("Look").ReadValue<Vector2>() * lookSensitivity);
    }
}