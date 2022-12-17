using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(MeshRenderer))]
public class Checkpoint : MonoBehaviour
{

    [SerializeField] public bool isActive;
    /*{
        get { return isActive; }
        private set
        {
            if(value == true) // using to change materials. could do listener but later
            {
                setActive();
            }
            else
            {
                setInActive();
            }
        }
    }*/

    [SerializeField] public int checkpointID;
    [SerializeField] public int unityInstanceID;
    public ParticleSystem activeCheckpointParticle;

    // All the checkpoints in the current level
    public Checkpoint[] levelCheckpoints { get; private set; } // All the checkpoints in the current level

    private MeshRenderer mr;

    [SerializeField] private Material inActiveMaterial;
    [SerializeField] private Material activeMaterial;
    
    BoxCollider bc;

    private void Awake()
    {
        bc = GetComponent<BoxCollider>();
        mr = GetComponent<MeshRenderer>();
        isActive = false;
        
    }

    private void OnValidate() // Not called in an actual build only called in editor
    {
        // check if number exists in level already 
        levelCheckpoints = FindObjectsOfType<Checkpoint>();


        foreach (Checkpoint checkpoint in levelCheckpoints)
        {
            if (checkpoint != this && checkpoint.checkpointID == this.checkpointID) // check for unique unity ID number that exists
            {
                Debug.LogError("checkpointID on " + gameObject.name + " already exists. Please verify and change number");
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        bc.isTrigger = true;
        mr.material = inActiveMaterial;
        unityInstanceID = GetInstanceID();
        activeCheckpointParticle.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.tag == "Player" && !isActive)
        {
            setActive();
        }
        
    }

    public void setActive()
    {

        levelCheckpoints = FindObjectsOfType<Checkpoint>();

        // Turn all other checkpoints inActive
        foreach(Checkpoint checkpoint in levelCheckpoints)
        {
            if (checkpoint.unityInstanceID != this.unityInstanceID)
            {
                checkpoint.setInActive();
            }
       }

        isActive = true;
        GameManager.Instance.setActiveCheckpoint(this);
        mr.material = activeMaterial;
        activeCheckpointParticle.Play();
    }

    public void setInActive()
    {
        isActive = false;
        mr.material = inActiveMaterial;
        activeCheckpointParticle.Stop();
    }

    

    // Function called when saving game 
    public void SaveGamePrepare()
    {
        Debug.Log("Save Game Prepare Checkpoint");

        // Get Checkpoint Data Object
        SaveLoadManager.GameStateData.DataCheckpoint data = new SaveLoadManager.GameStateData.DataCheckpoint();

        // Fill in data for curent enemy
        data.unityInstanceID = this.unityInstanceID;
        data.checkpointID = this.checkpointID;
        data.isActive = this.isActive;

        data.posRotScale.posX = transform.position.x;
        data.posRotScale.posY = transform.position.y;
        data.posRotScale.posZ = transform.position.z;

        data.posRotScale.rotX = transform.localEulerAngles.x;
        data.posRotScale.rotY = transform.localEulerAngles.y;
        data.posRotScale.rotZ = transform.localEulerAngles.z;

        data.posRotScale.scaleX = transform.localScale.x;
        data.posRotScale.scaleY = transform.localScale.y;
        data.posRotScale.scaleZ = transform.localScale.z;

        // Add enemy to Game State
        GameManager.StateManager.gameState.levelCheckpoints.Add(data);

    }

    // Function called when loading is complete.
    public void LoadGameComplete()
    {

        List<SaveLoadManager.GameStateData.DataCheckpoint> checkpoints = GameManager.StateManager.gameState.levelCheckpoints;

        // Reference to this checkpoint
        SaveLoadManager.GameStateData.DataCheckpoint data = null;

        for (int i = 0; i < checkpoints.Count; i++)
        {
            if (checkpoints[i].unityInstanceID == unityInstanceID)
            {
                // Found checkpoint. Now break from loop
                data = checkpoints[i];
                break;
            }
        }
        
        // If here and no enemy is found, then it was destroyed when saved. So destroy.
        if (data == null)
        {
            Destroy(gameObject);
            return;
        }

        // else load enemy data
        unityInstanceID = data.unityInstanceID;
        checkpointID = data.checkpointID;
        isActive = data.isActive; 


        // Set position // Currently unneeded until randomization is placed in 
        transform.position = new Vector3(data.posRotScale.posX,
            data.posRotScale.posY, data.posRotScale.posZ);

        // Set rotation
        transform.localRotation = Quaternion.Euler(data.posRotScale.rotX,
            data.posRotScale.rotY, data.posRotScale.rotZ);

        // Set scale
        transform.localScale = new Vector3(data.posRotScale.scaleX,
            data.posRotScale.scaleY, data.posRotScale.scaleZ);


    }

    private void Update()
    {
        if(isActive)
        {
            mr.material = activeMaterial;
        }
        else
        {
            mr.material = inActiveMaterial;
        }
    }

}
