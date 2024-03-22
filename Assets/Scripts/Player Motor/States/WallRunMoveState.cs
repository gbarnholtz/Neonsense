using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunMoveState : MoveState
{
    public override bool ShouldApplyGravity => true ;



    public override void MovePlayer()
    {
        throw new System.NotImplementedException();
    }

    public override void Update()
    {
        if(psm.IsGrounded) { psm.ChangeState(psm.WalkState); }
    }
}
