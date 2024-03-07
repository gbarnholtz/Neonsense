using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Progressive : SerializedMonoBehaviour, IProgressiveRead
{
    public event Action<float, float> OnChange;

    [SerializeField] private float initial;

    private float max;
    [ShowInInspector]
    public float Max
    {
        get { return max; }
        set {
            max = value;
            OnChange?.Invoke(Current, Max);
        }
    }

    private float current;
    [ShowInInspector]
    public virtual float Current
    {
        get { return current; }
        set { 
            current = Mathf.Clamp(value, 0, Max); 
            OnChange?.Invoke(Current, Max);
        }
    }

    public float Ratio => Current / Max;

    public void ResetCurrent()
    {
        Current = Max;
    }

    public virtual void Awake() {
        Max = initial;
        ResetCurrent();
    }
    
}

public interface IProgressiveRead
{
    float Max { get; }
    public float Current { get; }

    public float Ratio { get; }

    public event Action<float, float> OnChange;
}