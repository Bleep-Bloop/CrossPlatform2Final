using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{

    // Note: Look at PlayerMoveState to see how I attach AddListener for all this
    public UnityEvent playerHealthChanged { get; private set; } = new UnityEvent();

    private int _health = 1;

    public int health
    {
        get { return _health; }
        set
        {
            _health = value; playerHealthChanged?.Invoke(); 
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Debug death
        if(Input.GetKeyDown(KeyCode.U))
        {
            health = 0;
        }

    }
}
