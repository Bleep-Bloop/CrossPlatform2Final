using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{

    private readonly int deathHash = Animator.StringToHash("Death");
    private const float crossFadeDuration = 0.1f;

    public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        // Disabled to stop animations from being played.
        stateMachine.enabled = false;

        stateMachine.canMove = false;
        stateMachine.velocity = Vector3.zero;  
        
        stateMachine.animator.CrossFadeInFixedTime(deathHash, crossFadeDuration);
    }

    public override void Tick()
    {

        

    }

  
    public override void Exit() { }
}
