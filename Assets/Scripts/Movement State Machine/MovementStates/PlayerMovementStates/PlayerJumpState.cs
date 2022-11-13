using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{

    private readonly int jumpHash = Animator.StringToHash("Jump");
    private const float crossFadeDuration = 0.1f;

    public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.velocity = new Vector3(stateMachine.velocity.x, stateMachine.jumpForce, stateMachine.velocity.z);

        stateMachine.animator.CrossFadeInFixedTime(jumpHash, crossFadeDuration);
    }

    public override void Tick()
    {
        ApplyGravity();

        if (stateMachine.velocity.y <= 0f)
        {
            stateMachine.SwitchState(new PlayerFallState(stateMachine));
        }

        FaceMoveDirection();
        Move();

    }

    // It is mandatory to provide overwrites of all abstract methods of an abstract parent class unless the child class is also abstract. That is why we have provided an implementation for the Exit() even though it does nothing here.
    public override void Exit() { }

}
