using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum Pickups
    {
        Life,
        Speed,
        Jump,
        Coin
    }

    public Pickups currentPickup;
    public AudioClip pickupSound; // ToDo Implement


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            
            PlayerStateMachine curPlayerStateMachine = other.gameObject.GetComponent<PlayerStateMachine>();
            

            switch (currentPickup)
            {
                case Pickups.Life:
                    //GameManager.instance.lives++; // ToDo Implement
                    Debug.Log("Life Powerup");
                    break;
                case Pickups.Speed:
                    curPlayerStateMachine.StartMoveSpeedChange();
                    Debug.Log("Speed Powerup");
                    break;
                case Pickups.Jump:
                    curPlayerStateMachine.StartJumpForceChange(); 
                    Debug.Log("Jump Powerup");
                    break;
                case Pickups.Coin:
                    GameManager.Instance.canvasManager.playerScore++;
                    GameManager.Instance.soundManager.Play(pickupSound, false);
                    break;
            }

            //sfxManager.Play(pickupSound, false); // ToDo Implement

            Destroy(gameObject);
        }
    }
}
