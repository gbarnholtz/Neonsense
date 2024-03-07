using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputChain : SerializedMonoBehaviour, IInputProvider
{
    [OdinSerialize] private IPlayerInputProvider inputProvider;
    [OdinSerialize] private List<IInputModifier> inputChain;

    public ButtonAction OnJump => inputProvider.OnJump;

    public ButtonAction OnSprint => inputProvider.OnSprint;

    public List<ButtonAction> Abilities => inputProvider.Abilities;

    private void Awake()
    {
        GameStateManager.Instance.Player = transform;
    }

    public InputState GetState()
    {
        InputState input = new InputState(inputProvider.GetState());
        for (int i = 0; i < inputChain.Count; i++)
        {
            input = inputChain[i].ModifyInput(input);
        }

        return input;
    }

    //TODO: REMOVE THIS SHIT 
    private void OnDisable()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}