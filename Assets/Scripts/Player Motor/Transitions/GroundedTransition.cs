using System;
using UnityEngine;

public class GroundedTransition : ConditionTransition
{
    [SerializeField] private bool GroundedCondition;

    public override void Initialize(MoveState fromState, MoveState toState) {
        
    }

    protected override bool CheckCondition()
    {
        throw new NotImplementedException();
    }
}

