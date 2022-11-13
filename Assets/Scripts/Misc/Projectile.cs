using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{

    private Rigidbody projectileRigidBody;
    [SerializeField] private float speed = 25f;

    private void Awake()
    {
        projectileRigidBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        projectileRigidBody.velocity = transform.forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("On Collision Enter");

        if (collision.gameObject.tag == "Enemy")
        {
            Enemy curEnemy = collision.gameObject.GetComponent<Enemy>(); // Unsure if will work because child class of enemy but guess we will see.
            curEnemy.TakeDamage();
            Debug.Log("Damage Dealt");
        }

       //    Destroy(gameObject);
    }


}
