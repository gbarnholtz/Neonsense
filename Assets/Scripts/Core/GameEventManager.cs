using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager Instance
    {
        get
        {
            if (instance == null) instance = new GameObject("[Game Event Manager]").AddComponent<GameEventManager>();
            return instance;
        }
    }
    private static GameEventManager instance;

    public static event Action<DamageInstance, Transform> OnHit;
    public static event Action<DamageInstance, Transform> OnKill;
    public void InvokeHit(DamageInstance damage, Transform objectHit) {
        OnHit?.Invoke(damage, objectHit);

    }
    public void InvokeKill(DamageInstance damage, Transform objectKilled)
    {
        OnKill?.Invoke(damage, objectKilled);
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
