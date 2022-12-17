using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;


public class ThirdPersonShooterController : MonoBehaviour
{

    [SerializeField] private LayerMask shootColliderLayerMask = new LayerMask();
    //[SerializeField] private Transform debugTransform;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private HealthComponent playerHealthComponent;
    private PlayerStateMachine playerAnimState;

    public Transform activeCheckpointPosition;

    [SerializeField] public int hotdogAmmo;


    bool rotateToFace = true;

    Vector3 mouseWorldPosition; // Was inside update but I am playing around with ShootProjectile being outside that.

    

    private void Awake()
    {
        playerHealthComponent = GetComponent<HealthComponent>();
        playerHealthComponent.playerHealthChanged.AddListener(IsPlayerDead);
        playerAnimState = GetComponent<PlayerStateMachine>();
    }

    private void Update()
    {
        
       mouseWorldPosition = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if(Physics.Raycast(ray, out RaycastHit raycastHit, 9999999f, shootColliderLayerMask))
        {
           // debugTransform.position = raycastHit.point; // Debug to see where aiming
            mouseWorldPosition = raycastHit.point;
        }

        // add if isAiming
        if(rotateToFace == true)
        {
            
            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);

        }
    }

    public void ShootProjectile()
    {
        Vector3 aimDirection = (mouseWorldPosition - projectileSpawnPoint.position).normalized;
        Instantiate(projectile, projectileSpawnPoint.position, Quaternion.LookRotation(aimDirection, Vector3.up));

        // Change animation back to idle
    }

    public void IsPlayerDead()
    {
        if(playerHealthComponent.health == 0)
        {
            rotateToFace = false;
        }
    }

    // Called from animation even in death animation
    public void Death()
    {
        GameManager.Instance.Death();
    }

    public void SaveGamePrepare()
    {
        Debug.Log("Save Game Prepare ThirdPersonShooterControrller");

        SaveLoadManager.GameStateData.DataPlayer data = GameManager.StateManager.gameState.player;

        data.lives = GameManager.Instance.lives;
        data.health = playerHealthComponent.health;
        

        activeCheckpointPosition = GameManager.Instance.activeCheckpoint.transform;

        data.activeCheckpointTransform.posX = activeCheckpointPosition.position.x;
        data.activeCheckpointTransform.posY = activeCheckpointPosition.position.y;
        data.activeCheckpointTransform.posZ = activeCheckpointPosition.position.z;

        data.activeCheckpointTransform.rotX = activeCheckpointPosition.localEulerAngles.x;
        data.activeCheckpointTransform.rotY = activeCheckpointPosition.localEulerAngles.y;
        data.activeCheckpointTransform.rotZ = activeCheckpointPosition.localEulerAngles.z;

        data.activeCheckpointTransform.scaleX = activeCheckpointPosition.localScale.x;
        data.activeCheckpointTransform.scaleY = activeCheckpointPosition.localScale.y;
        data.activeCheckpointTransform.scaleZ = activeCheckpointPosition.localScale.z;


        // Don't need player position because spawn on checkpoint
        data.posRotScale.posX = transform.position.x;
        data.posRotScale.posY = transform.position.y;
        data.posRotScale.posZ = transform.position.z;

        data.posRotScale.rotX = transform.localEulerAngles.x;
        data.posRotScale.rotY = transform.localEulerAngles.y;
        data.posRotScale.rotZ = transform.localEulerAngles.z;

        data.posRotScale.scaleX = transform.localScale.x;
        data.posRotScale.scaleY = transform.localScale.y;
        data.posRotScale.scaleZ = transform.localScale.z;
        

    }

    public void LoadGameComplete()
    {
        Debug.Log("LoadGameComplete");
        SaveLoadManager.GameStateData.DataPlayer data = GameManager.StateManager.gameState.player;

        playerHealthComponent.health = data.health;
        GameManager.Instance.lives = data.lives;


       
        //  playerAnimState.TogglePlayerMoveState(); // If this is uncommented it will not move character
        playerAnimState.setCanMove(true);

       
        transform.position = new Vector3(data.activeCheckpointTransform.posX, data.activeCheckpointTransform.posY, data.activeCheckpointTransform.posZ);
        transform.localRotation = Quaternion.Euler(data.activeCheckpointTransform.rotX, data.activeCheckpointTransform.rotY, data.activeCheckpointTransform.rotZ);
        transform.localScale = new Vector3(data.activeCheckpointTransform.scaleX, data.activeCheckpointTransform.scaleY, data.activeCheckpointTransform.scaleZ);
        
    }

}

