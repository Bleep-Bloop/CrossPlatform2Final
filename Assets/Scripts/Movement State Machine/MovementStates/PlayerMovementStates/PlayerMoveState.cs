using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{

    private readonly int moveSpeedHash = Animator.StringToHash("MoveSpeed");
    private readonly int moveBlendTreeHash = Animator.StringToHash("MoveBlendTree");
    private const float animationDampTime = 0.1f;
    private const float crossFadeDuration = 0.1f;


    public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        // Player constantly pulled down.
        stateMachine.velocity.y = Physics.gravity.y;

        // Crossfade our animator moveBlendTree state in a fixed time. Results in a smooth transition between the animations.
        stateMachine.animator.CrossFadeInFixedTime(moveBlendTreeHash, crossFadeDuration);

        // Register SwitchToJumpState method on the OnJumpPerformed event in our InputReader. Now when we press the button that function will be called.
        stateMachine.inputReader.OnJumpPerformed += SwitchToJumpState;

        stateMachine.inputReader.OnShootPerformed += SwitchToShootState;

        stateMachine.inputReader.OnChopPerformed += SwitchToChopState;

        stateMachine.inputReader.OnKickPerformed += SwitchToKickState;

        // When health is changed call OnPlayerHealthChanged to see if they are dead. Then change to desired state.
        stateMachine.playerHealthComponent.playerHealthChanged.AddListener(OnPlayerHealthChanged);

        // ToDo: Make it so player only rotates to face camera when aiming.

    }

    public override void Tick()
    {

        if(Input.GetKeyDown(KeyCode.C))
        {
            GameManager.Instance.PassSound(stateMachine.taunt1, false);
        }

        // If not grounded switch to fall state.
        if(!stateMachine.controller.isGrounded)
        {
            stateMachine.SwitchState(new PlayerFallState(stateMachine));
        }

        CalculateMoveDirection();
        FaceMoveDirection();
        Move();

        // We use a hash to identify the name of the moveSpeed parameter because integers is much more performant than comparing strings. Although it is possible to pass a string to the method. Overall It wouldn't matter her but this is the more performant way.
        // Also we used the squared magnitude because we're not interested in the actual value; we just need to know if the value is 0 or not, and calculating the magnitude from squared magnitude requires an extra step, teh square root. This is just about squeezing a little bit of performance, saving a few cycles every frame when the tick moethod is executed.
        // If the value is 0, we set the moveSpeed parameter also to 0, otherwise, we set it to 1, effectively transitions in our moveBlendTree from Idle to Run animation. AnimationDampTime & Time.deltaTime make this transition smooth and framerate independent.
        stateMachine.animator.SetFloat(moveSpeedHash, stateMachine.inputReader.moveComposite.sqrMagnitude > 0f ? 1f : 0f, animationDampTime, Time.deltaTime); // ToDo: YES

    }

    public override void Exit() // ToDo: Confused
    {
        if (stateMachine.canMove)
            stateMachine.inputReader.OnJumpPerformed -= SwitchToJumpState;
    }

    private void SwitchToJumpState()
    {
        if (stateMachine.canMove)
            stateMachine.SwitchState(new PlayerJumpState(stateMachine));
    }

    private void SwitchToShootState()
    {
        if (stateMachine.canMove)
            stateMachine.SwitchState(new PlayerShootState(stateMachine));
    }

    private void SwitchToChopState()
    {
        if (stateMachine.canMove)
            stateMachine.SwitchState(new PlayerChopState(stateMachine));
    }

    private void SwitchToKickState()
    {
        if (stateMachine.canMove)
            stateMachine.SwitchState(new PlayerKickState(stateMachine));
    }

    private void SwitchToDeathState()
    {
        stateMachine.SwitchState(new PlayerDeadState(stateMachine));
    }

    private void OnPlayerHealthChanged()
    {
        if(stateMachine.playerHealthComponent.health == 0 && !stateMachine.playerHealthComponent.isDead)
        {
            SwitchToDeathState();
        }
    }

}
