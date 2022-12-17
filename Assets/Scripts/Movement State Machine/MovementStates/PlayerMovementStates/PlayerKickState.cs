using UnityEngine;

public class PlayerKickState : PlayerBaseState
{
    private readonly int kickHash = Animator.StringToHash("Kick");
    private const float crossFadeDuration = 0.1f;

    public PlayerKickState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        GameManager.Instance.PassSound(stateMachine.kickSFX, false);
        stateMachine.velocity = Vector3.zero;

        stateMachine.animator.CrossFadeInFixedTime(kickHash, crossFadeDuration);
    }


    public override void Tick()
    {

    }

    // It is mandatory to provide overwrites of all abstract methods of an abstract parent class unless the child class is also abstract. That is why we have provided an implementation for the Exit() even though it does nothing here.
    public override void Exit() { }

   


}
