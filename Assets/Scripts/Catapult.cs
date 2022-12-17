using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Lobs a game object towards a marked location every random seconds while person is in zone.
public class Catapult : MonoBehaviour
{

    public Transform target;
    public CannonProjectile cannonProjectile;
    public ParabolaController parabolaController;
    public AudioClip shotSFX;
    public float timeBetweenShots;


    public bool isFiring = false;

    // Angular speed in degrees per sec. Used on rotating the cannon (purely visual no effect)
    float speed = 340;

    public float fireSpeed = 5; // Tims in seconds for how often the cannon fires while targetting player

    private float step;

    private void Awake()
    {
        parabolaController = cannonProjectile.GetComponent<ParabolaController>();
        timeBetweenShots = Random.Range(3, 8);
    }

    private void Start()
    {
        InvokeRepeating("Fire", -1, timeBetweenShots);
        target = null;
    }

    private void Update()
    {

        step = speed * Time.deltaTime;

        if (target)
        {
            RotateCatapult();
        }

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            target = other.gameObject.transform;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            target = null; 
        }
    }

    public void Fire()
    {

        if(target)
        {
            
            isFiring = true;
            if(shotSFX)
            {
                GameManager.Instance.PassSound(shotSFX, false);
                //SoundManager.Instance.Play(shotSFX, false);
            }
            parabolaController.ResetParabolaMidPoint();
            cannonProjectile.LaunchProjectile(target.transform);
        }
        else
        {
            Debug.Log("No Target");
        }
        
    }

    /// <summary>
    /// Rotates the catapult to face the target. 
    /// Purely visual, no affect on targetting or launching.
    /// </summary>
    private void RotateCatapult()
    {
       // transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, step);
        
    }



}