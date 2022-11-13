using UnityEngine;

public class PlayerFallState : PlayerBaseState
{

    private readonly int FallHash = Animator.StringToHash("Fall");
    private const float CrossFadeDuration = 0.1f;

    public PlayerFallState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.velocity.y = 0f;

        stateMachine.animator.CrossFadeInFixedTime(FallHash, CrossFadeDuration);
    }

    public override void Tick()
    {
        ApplyGravity();
        Move();

        if(stateMachine.controller.isGrounded)
        {
            stateMachine.SwitchState(new PlayerMoveState(stateMachine));
        }
    }

    // It is mandatory to provide overwrites of all abstract methods of an abstract parent class unless the child class is also abstract. That is why we have provided an implementation for the Exit() even though it does nothing here.
    public override void Exit() { }


}
