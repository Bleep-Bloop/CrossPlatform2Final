using UnityEngine.AI;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    protected NavMeshAgent navMeshAgent;
    protected SkinnedMeshRenderer skinnedMeshRenderer;
    protected Animator anim;

    protected int _health;

    public int health
    {
        get { return _health; }
        set
        {
            _health = value;

            if (_health <= 0)
                Death();
        }
    }

    protected void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    protected void Start()
    {
        
    }

    // Update is called once per frame
    protected void Update()
    {
        
    }

    public virtual void Death()
    {
        anim.SetTrigger("Death");
    }

    // Called from animation event in death animations
    public virtual void DestroyMyself()
    {
        Destroy(gameObject);
    }

    public virtual void TakeDamage()
    {
        health -= 1;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            PlayerStateMachine curPlayerStateMachine = collision.gameObject.GetComponent<PlayerStateMachine>();
            if(curPlayerStateMachine)
            {
                if (!curPlayerStateMachine.isChopping && !curPlayerStateMachine.isKicking)
                {
                    HealthComponent curPlayerHealth = collision.gameObject.GetComponent<HealthComponent>();
                    curPlayerHealth.health = 0;
                }
            }
          

            

        }
    }

    private void OnTriggerEnter(Collider other)
    {
       

        if (other.gameObject.tag == "Player")
        {

            PlayerStateMachine curPlayer = other.GetComponentInParent<PlayerStateMachine>();
            
            if(curPlayer)
            {
                if(curPlayer.isKicking)
                {
                    anim.SetTrigger("KickDeath");
                    Debug.Log("Kick Death");
                }
                else if(curPlayer.isChopping)
                {
                    anim.SetTrigger("ChopDeath");
                    Debug.Log("Chop Death");
                }
            }
            
        }
    }


}
