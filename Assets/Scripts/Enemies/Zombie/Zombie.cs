using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{

    public GameObject player;

    new void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    new private void Start()
    {
        navMeshAgent.SetDestination(player.transform.position);
    }

    // Update is called once per frame
    new void Update()
    {
        if(isDead == true)
        {
            navMeshAgent.isStopped = true;
        }
        else
        {
            navMeshAgent.SetDestination(player.transform.position);
        }
        
    }



}
