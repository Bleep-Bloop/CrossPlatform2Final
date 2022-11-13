using UnityEngine;

public class BooMoveState : BooBaseState
{

    private readonly int moveSpeedHash = Animator.StringToHash("MoveSpeed");
    private readonly int moveBlendTreeHash = Animator.StringToHash("MoveBlendTree");
    private const float animationDampTime = 0.1f;
    private const float crossFadeDuration = 0.1f;

    public BooMoveState(BooStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        // Enemy constantly pulled down.
        stateMachine.velocity.y = Physics.gravity.y;

        // Crossfade our animator moveBlendTree state in a fixed time. Results in a smooth transition between the animations.
        stateMachine.animator.CrossFadeInFixedTime(moveBlendTreeHash, crossFadeDuration);

    }

    public override void Tick()
    {
        
    }

    public override void Exit()
    {
        
    }
}
