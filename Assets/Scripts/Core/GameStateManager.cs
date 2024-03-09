using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get {

            if (instance == null) instance = new GameObject("[Game State Manager]").AddComponent<GameStateManager>();
            return instance;
        }
    }
    private static GameStateManager instance;

    public Transform Player;
    public Camera Camera;
    [SerializeField] private float timeElapsed = 0;

    // private float gameDifficulty = 1;
    [SerializeField] public int Tokens;
    //In seconds
    [SerializeField] private float tokenTimeInterval = 5f;
    [SerializeField] private int tokensPerMinute = 500;
    [SerializeField] private float tokenTimer = 0;
    [SerializeField] private int MAX_TOKEN_COUNT = 1000;

    private Scene HUD;
    public event Action<int> OnTokenAdd;

    private void Awake()
    {
        if (instance == null) instance = this;
        Camera = Camera.main;
    }

    private void OnEnable()
    {
        UpdateTicker.Subscribe(IncrementTime);
    }

    //Changed this to fit tokens but needs to be changed later
    private void IncrementTime()
    {     
        timeElapsed += Time.deltaTime;
        if (tokenTimer < tokenTimeInterval)
        {
            tokenTimer += Time.deltaTime;
        }
        else
        {
            tokenTimer = 0;
            AddTokens();
            OnTokenAdd?.Invoke(Tokens);
        }
    }

    private void OnDisable()
    {
        UpdateTicker.Unsubscribe(IncrementTime);
        instance = null;
        OnTokenAdd = null;
    }

    //Can change for the logic sometime later once we get the formula
    private void AddTokens()
    {
        if(Tokens + tokensPerMinute <= MAX_TOKEN_COUNT) Tokens += tokensPerMinute;
    }

    public float GetTime()
    {
        return timeElapsed;
    }

    
}
