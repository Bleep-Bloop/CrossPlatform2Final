using UnityEngine;

// ToDo: Set up shooting and death Make enemy base class
public class BooEnemy : Enemy
{

    public GameObject player;

    public Material ghostMaterial;
    public Material transparentGhostMaterial;

    private BooMechanic booMechanic;

    new void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player");
        booMechanic = GetComponent<BooMechanic>();
    }

    // Start is called before the first frame update
    new void Start()
    {

    }

    // Update is called once per frame
    new void Update()
    {
        if(booMechanic.isSpotted)
        {
            skinnedMeshRenderer.material = transparentGhostMaterial;
            navMeshAgent.isStopped = true;
            anim.SetBool("isSpotted", true);
        }
        else
        {
            skinnedMeshRenderer.material = ghostMaterial;
            navMeshAgent.isStopped = false;
            anim.SetBool("isSpotted", false);
            navMeshAgent.SetDestination(player.transform.position);
        }
    }
}
