using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{

    public bool isDead;

    // Note: Look at PlayerMoveState to see how I attach AddListener for all this
    public UnityEvent playerHealthChanged { get; private set; } = new UnityEvent();

    private int _health = 1;
    public int baseHealth;

    public int health
    {
        get { return _health; }
        set
        {
            _health = value; playerHealthChanged?.Invoke();
            if (_health <= 0)
            {
                isDead = true;
                GameManager.Instance.lives--;
            }  
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Take Damage Debug
        if(Input.GetKeyDown(KeyCode.U))
        {
            TakeDamage(1);
        }

    }

    public void TakeDamage(int damageValue)
    {
        health = health - 1;
    }

}