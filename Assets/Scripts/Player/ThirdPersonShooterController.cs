using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;


public class ThirdPersonShooterController : MonoBehaviour
{

    [SerializeField] private LayerMask shootColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private HealthComponent playerHealthComponent;

    bool rotateToFace = true;

    Vector3 mouseWorldPosition; // Was inside update but I am playing around with ShootProjectile being outside that.

    private void Awake()
    {
        playerHealthComponent = GetComponent<HealthComponent>();
        playerHealthComponent.playerHealthChanged.AddListener(IsPlayerDead);
    }

    private void Update()
    {
        
       mouseWorldPosition = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if(Physics.Raycast(ray, out RaycastHit raycastHit, 9999999f, shootColliderLayerMask))
        {
            debugTransform.position = raycastHit.point; // Debug to see where aiming
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
    public void GameOver()
    {
        GameManager.Instance.GameOver();
    }

}

