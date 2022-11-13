using UnityEngine;

// ToDo: Stop player from rotating while chopping. Override facemovedirection()?
public class PlayerChopState : PlayerBaseState
{

    private readonly int chopHash = Animator.StringToHash("Chop");
    private const float crossFadeDuration = 0.1f;

    public PlayerChopState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.velocity = Vector3.zero;

        stateMachine.animator.CrossFadeInFixedTime(chopHash, crossFadeDuration);
    }

    public override void Tick()
    {
       
    }

    // It is mandatory to provide overwrites of all abstract methods of an abstract parent class unless the child class is also abstract. That is why we have provided an implementation for the Exit() even though it does nothing here.
    public override void Exit() { }

    

}
