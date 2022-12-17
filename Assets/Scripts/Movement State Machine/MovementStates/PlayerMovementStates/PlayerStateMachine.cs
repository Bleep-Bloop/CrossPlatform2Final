using UnityEngine;
using System.Collections;

[RequireComponent(typeof(InputReader))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]

public class PlayerStateMachine : StateMachine
{
    
    public Vector3 velocity;
    public float movementSpeed{ get; private set; } = 5f;
    public float jumpForce { get; private set; } = 5f;
    public float lookRotationDampFactor { get; private set; } = 10f;
    public Transform mainCamera { get; private set; }
    public InputReader inputReader { get; private set; }
    public Animator animator { get; private set; }
    public CharacterController controller { get; private set; }
    public HealthComponent playerHealthComponent { get; private set; }

    public bool canMove = true;

    public bool isChopping = false;
    public bool isKicking = false;

    public AudioClip deathSFX;
    public AudioClip chopSFX;
    public AudioClip kickSFX;

    public AudioClip taunt1;
    

    Coroutine jumpForceChange;
    Coroutine moveSpeedChange;




    // put a isChop isKicking check to get death animations
    public BoxCollider meleeHitBox;

    // ToDo: Maybe move this up into StateMachine. Unused currently but could be useful.
    public string GetCurrentPlayingClipName()
    {

        if(animator)
        {
            AnimatorClipInfo[] currentClipInfo;

            currentClipInfo = this.animator.GetCurrentAnimatorClipInfo(0);

            return currentClipInfo[0].clip.name;
        }

        return null;
    }

    // Called at the end of certain animations to return to the MoveBlendTree
    public void ReturnToMoveBlendTree()
    {
        SwitchState(new PlayerMoveState(this));
    }

    private void Awake()
    {

        mainCamera = Camera.main.transform;

        inputReader = GetComponent<InputReader>();
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        playerHealthComponent = GetComponent<HealthComponent>();

    }

    // Start is called before the first frame update
    void Start()
    {
        SwitchState(new PlayerMoveState(this)); 
    }

    public void TogglePlayerMoveState()
    {
        SwitchState(new PlayerMoveState(this)); 
    }

    public void ToggleMeleeHitBox()
    {
        meleeHitBox.enabled = !meleeHitBox.enabled;
    }

    public void SetIsChoppingTrue()
    {
        isChopping = true;
    }
    public void SetIsChoppingFalse()
    {
        isChopping = false;
    }

    public void SetIsKickingTrue()
    {
        isKicking = true;
    }
    public void SetIsKickingFalse()
    {
        isKicking = false;
    }

    public void setCanMove(bool state)
    {
        //this.enabled = true;
        canMove = state;
    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.tag == "Enemy")
        {
            Debug.Log("Hit Enemy");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    public void StartJumpForceChange()
    {
        if (jumpForceChange == null)
        {
            jumpForceChange = StartCoroutine(JumpForceChange());
        }
        else
        {
            StopCoroutine(jumpForceChange);
            jumpForceChange = null;
            jumpForce /= 2;
            jumpForceChange = StartCoroutine(JumpForceChange());
        }
    }

    IEnumerator JumpForceChange()
    {
        jumpForce *= 2;

        yield return new WaitForSeconds(5.0f);

        jumpForce /= 2;
        jumpForceChange = null;
    }

    public void StartMoveSpeedChange()
    {
        if(moveSpeedChange == null)
        {
            moveSpeedChange = StartCoroutine(MoveSpeedChange());
        }
        else
        {
            StopCoroutine(MoveSpeedChange());
            moveSpeedChange = null;
            movementSpeed /= 2;
            moveSpeedChange = StartCoroutine(MoveSpeedChange());
        }
    }

    IEnumerator MoveSpeedChange()
    {
        movementSpeed *= 2;

        yield return new WaitForSeconds(5.0f);

        movementSpeed /= 2;
        moveSpeedChange = null;
    }


}
