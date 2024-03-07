using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInputSO", menuName = "ScriptableObjects/PlayerInputSO", order = 1)]
public class PlayerInputSO : ScriptableObject, IPlayerInputProvider
{
    public PlayerInput input { get; private set; }
    public List<ButtonAction> Abilities => abilityActions;
    private List<ButtonAction> abilityActions;

    public ButtonAction OnJump { get { return jumpAction; } }
    public ButtonAction OnSprint { get { return sprintAction; } }
    public ButtonAction OnFreeMouse { get { return freeMouseAction; } }
    private ButtonAction jumpAction, sprintAction, freeMouseAction;

    [SerializeField] private float lookSensitivity;

    public void OnEnable()
    {
        input = new PlayerInput();
        input.Enable();

        jumpAction = new ButtonAction(input.Game.Jump);
        sprintAction = new ButtonAction(input.Game.Sprint);
        freeMouseAction = new ButtonAction(input.Game.FreeMouse);
        ButtonAction abilityPrimary = new ButtonAction(input.Game.Ability1);
        ButtonAction abilitySecondary = new ButtonAction(input.Game.Ability2);
        ButtonAction abilityUtility = new ButtonAction(input.Game.Ability3);
        ButtonAction abilitySpecial = new ButtonAction(input.Game.Ability4);
        abilityActions = new List<ButtonAction> { abilityPrimary, abilitySecondary, abilityUtility, abilitySpecial }; 
    }

    public PlayerInputState GetState()
    {
        return new PlayerInputState(input.Game.Move.ReadValue<Vector2>(), input.Game.Look.ReadValue<Vector2>() * lookSensitivity);
    }
}