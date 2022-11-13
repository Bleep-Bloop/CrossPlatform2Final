using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BooStateMachine : StateMachine
{
    public Vector3 velocity;
    public Animator animator { get; private set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SwitchState(new BooMoveState(this));
    }

}
