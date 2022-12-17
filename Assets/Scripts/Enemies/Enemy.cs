using UnityEngine.AI;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    protected NavMeshAgent navMeshAgent;
    protected SkinnedMeshRenderer skinnedMeshRenderer;
    protected Animator anim;
    protected BoxCollider bc;
    [SerializeField] protected int dropRate = 100; // Percentage chance of a power up being dropped upon death.
    [SerializeField] protected PowerUp powerUpPrefab;
    public AudioClip deathNoise;

    protected int _health;

    public int health
    {
        get { return _health; }
        set
        {
            _health = value;

            if (_health <= 0)
            {
                //Death();
            }
        }
    }

    protected bool isDead = false;

    protected void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        anim = GetComponent<Animator>();
        bc = GetComponent<BoxCollider>();

        // Ensuring the dropRate percentage is between 0-100% 
        if (dropRate > 100)
            dropRate = 100;
        else if (dropRate < 0)
            dropRate = 0;
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

        if (collision.gameObject.tag == "Projectile")
        {
            Death(DeathType.Projectile);
        }
        else if (collision.gameObject.tag == "Player")
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
       
        if (other.gameObject.tag == "MeleeHitbox")
        {
            PlayerStateMachine curPlayer = other.GetComponentInParent<PlayerStateMachine>();
            
            if(curPlayer)
            {
                if(curPlayer.isKicking)
                {
                    Death(DeathType.Kick);
                }
                else if(curPlayer.isChopping)
                {
                    Death(DeathType.Kick);
                }
            }    
        }
    }

    public virtual void Death(DeathType deathType)
    {

        switch (deathType)
        {
            case DeathType.Chop:
                anim.SetTrigger("ChopDeath");
                break;
            case DeathType.Kick:
                anim.SetTrigger("KickDeath");
                break;
            case DeathType.Projectile:
                anim.SetTrigger("ProjectileDeath");
                break;
            case DeathType.Death:
                break;
            default:
                anim.SetTrigger("Death"); // ToDo: Double check this is active.
                break;
        }

        Invoke("DropPowerUp", 3.5f);
        bc.enabled = false;
        isDead = true;
        
    }

    // Spawns a pickup. Called from animation event.
    void DropPowerUp()
    {

        if(powerUpPrefab)
        {
            if (Random.Range(0, 100) <= dropRate)
            {
                Instantiate(powerUpPrefab, this.transform.position, this.transform.rotation); 
            }
            else
            {
                Debug.Log("Bad Luck - No Drop");
            }
        }
    }


    public void PlayDeadSound()
    {
        GameManager.Instance.soundManager.Play(deathNoise, false);
    }

}


public enum DeathType
{
    Chop,
    Kick,
    Projectile,
    Death, 
}