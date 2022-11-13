using UnityEngine;

public class PlayerShootState : PlayerBaseState
{

    private readonly int shootHash = Animator.StringToHash("Shoot");
    private const float crossFadeDuration = 0.1f;
   

    public PlayerShootState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.velocity = Vector3.zero;

        stateMachine.animator.CrossFadeInFixedTime(shootHash, crossFadeDuration);
    }

    public override void Tick()
    {
       
    }


    public override void Exit()
    {

    }
}
