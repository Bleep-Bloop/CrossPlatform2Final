using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonProjectile : MonoBehaviour
{

    protected float Animation;
    protected ParabolaController parabolaController;
    public Transform targetLocation;
    public ParticleSystem explosionParticles;
    public AudioClip explosionSFX;


    private void Awake()
    {
        parabolaController = GetComponent<ParabolaController>();
        explosionParticles = GetComponentInChildren<ParticleSystem>();
    }

    private void Start()
    {
        parabolaController.Autostart = false;
        parabolaController.Speed = 20;
        

    }

    // Update is called once per frame
    void Update()
    {
        
        Animation += Time.deltaTime;

        Animation = Animation % 5f;

       
        
       

        if(GameObject.FindGameObjectWithTag("Catapult").GetComponent<Catapult>().isFiring == true && this.parabolaController.Animation == false)
        {
            Debug.Log("Blam");
            GameManager.Instance.PassSound(explosionSFX, false);        
            this.explosionParticles.Play();
            GameObject.FindGameObjectWithTag("Catapult").GetComponent<Catapult>().isFiring = false;
            Debug.Log("isFiring False");
        }



    }

    public void LaunchProjectile(Transform impactLocation)
    {

        if(parabolaController.Animation == false)
        {
            GameObject.FindGameObjectWithTag("Catapult").GetComponent<Catapult>().isFiring = true;
            targetLocation.transform.position = impactLocation.position;
            parabolaController.ResetParabolaMidPoint();
            parabolaController.FollowParabola();
        }

       
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("ToDo: Player Death. Ragdoll?");
        }

        
    }


    


}