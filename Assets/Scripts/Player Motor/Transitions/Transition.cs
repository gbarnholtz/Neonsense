using System;
using UnityEngine;

public abstract class Transition
{
    [HideInInspector] public Action Callback;
    public virtual void Initialize(MoveState fromState, MoveState toState) { }
    public virtual void Enter() { }
    public virtual void Exit() { }   
}

public abstract class ActionTransition : Transition {

}

public abstract class ConditionTransition:Transition {
    protected abstract bool CheckCondition();
    public void Update()
    {
        if (!CheckCondition() && Callback != null) Callback.Invoke();
    }
}
