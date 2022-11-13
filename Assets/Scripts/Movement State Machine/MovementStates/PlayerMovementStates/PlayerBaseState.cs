using UnityEngine;

public abstract class PlayerBaseState : State
{

    protected readonly PlayerStateMachine stateMachine;

    protected PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    /// <summary>
    /// Calculate the direction of player movement based on the orientation of the camera and input values from InputReader.MoveComposite
    /// </summary>
    protected void CalculateMoveDirection()
    {
        Vector3 cameraForward = new(stateMachine.mainCamera.forward.x, 0, stateMachine.mainCamera.forward.z);
        Vector3 cameraRight = new(stateMachine.mainCamera.right.x, 0, stateMachine.mainCamera.right.z);

        Vector3 moveDirection = cameraForward.normalized * stateMachine.inputReader.moveComposite.y + cameraRight.normalized * stateMachine.inputReader.moveComposite.x;

        stateMachine.velocity.x = moveDirection.x * stateMachine.movementSpeed;
        stateMachine.velocity.z = moveDirection.z * stateMachine.movementSpeed;

    }

    /// <summary>
    /// Rotate the player so it's always facing the direction of movement (direction from velocity with the y value zeroed to prevent being tilted up or down.
    /// </summary>
    protected void FaceMoveDirection()
    {
        Vector3 faceDirection = new(stateMachine.velocity.x, 0f, stateMachine.velocity.z);

        if (faceDirection == Vector3.zero)
            return;

        stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, Quaternion.LookRotation(faceDirection), stateMachine.lookRotationDampFactor * Time.deltaTime);
    }

    /// <summary>
    /// Constantly pull player to ground. We are going to call this method in PlayerMoveState to keep the player grounded.
    /// ToDo: Move this to be handled directly inside player class (for future playing with gravity).
    /// </summary>
    protected void ApplyGravity()
    {
        if(stateMachine.velocity.y > Physics.gravity.y)
        {
            stateMachine.velocity.y += Physics.gravity.y * Time.deltaTime;
        }
    }
    
    protected void Move()
    {
        stateMachine.controller.Move(stateMachine.velocity * Time.deltaTime);
    }

}
