using UnityEngine;

public abstract class BooBaseState : State
{

    protected readonly BooStateMachine stateMachine;

    protected BooBaseState(BooStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

}
