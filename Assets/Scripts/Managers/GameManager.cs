using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

// Require component SaveLoadManager
public class GameManager : Singleton<GameManager>
{

    public GameObject player;
    public Checkpoint activeCheckpoint;

    public AudioClip curLevelMusic;
    public AudioSource audioSource;

    public SoundManager soundManager;
    public CanvasManager canvasManager;


    public static SaveLoadManager StateManager
    {
        get
        {
            if (!statemanager)
                statemanager = Instance.GetComponent<SaveLoadManager>();

            return statemanager;
        }
    }

    

    // Internal reference to Saveload Game Manager
    private static SaveLoadManager statemanager = null;

    // Should load from save game state on level load, or just restart level from defaults
    private static bool bShouldLoad = false;

    // Lives
    [SerializeField] private int _lives = 3;

    public int lives
    {
        get { return _lives; }
        set
        {

            if (_lives <= 0)
            {
                Debug.Log("GAME OVER");
                GameOver();
            }   

            _lives = value;

           


            if (_lives > value)
            {
                Debug.Log("DEATH");
                Death();
            }
                

            


            if (_lives > maxLives)
                _lives = maxLives;

            

            //OnLifeValueChanged.Invoke(_lives);
        }

    }

    public int maxLives = 3;



    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        player = GameObject.FindGameObjectWithTag("Player");
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    // Start is called before the first frame update
    void Start()
    {
        
        if(bShouldLoad)
        { 
            StateManager.Load(Application.persistentDataPath + "/SaveGame.xml");

            // Reset load flag
            bShouldLoad = false;
        }
        
        
        SceneManager.sceneLoaded += OnSceneLoadCursorState;

   


    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        soundManager = FindObjectOfType<SoundManager>();
        if(!soundManager)
        {
            Debug.LogWarning("Level does not have a sound manager");
        }
        canvasManager = FindObjectOfType<CanvasManager>();
        if (!canvasManager)
        {
            Debug.LogWarning("Level does not have a canvas manager");
        }
    }

    public void PassSound(AudioClip clip, bool isMusic)
    {
        soundManager.Play(clip, isMusic);
    }

    // Update is called once per frame
    void Update()
    {
        
        // Just for debugging
        if(Input.GetKey(KeyCode.Q))
        {
            LockCursor();
        }
        if (Input.GetKey(KeyCode.Z))
        {
            SceneManager.LoadScene("GameOver");
        }

    }

    // Debug function
    private void LockCursor()
    {

        if(Cursor.lockState == CursorLockMode.None)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;

        // Cursor.lockState = CursorLockMode.Confined; // Lock to window

    }

    // Go to game over screen
    public void Death()
    {

        GameManager.Instance.canvasManager.toggleDeathMenu();
        
    }

    public void GameOver()
    {
        Debug.Log("Load GameOver Scene");
        SceneManager.LoadScene("GameOver");
        GameManager.Instance.canvasManager.ToggleGameOverImage(true);
    }

    // Respawn character at active checkpoint
    public void Respawn()
    {
        int zeroplz = 0;

        canvasManager.playerScore = 0;
        canvasManager.playerScoreText.text = "0";
        //GameManager.Instance.LockCursor();

        Debug.Log("respawn");
        player.GetComponent<HealthComponent>().health = player.GetComponent<HealthComponent>().baseHealth;
        player.GetComponent<PlayerStateMachine>().enabled = true;
        player.GetComponent<PlayerStateMachine>().setCanMove(true);
          

     
        player.transform.position = new Vector3(activeCheckpoint.transform.position.x, activeCheckpoint.transform.position.y, activeCheckpoint.transform.position.z);
        player.transform.localRotation = Quaternion.Euler(activeCheckpoint.transform.rotation.x, activeCheckpoint.transform.rotation.y, activeCheckpoint.transform.rotation.z);
        player.transform.localScale = new Vector3(activeCheckpoint.transform.localScale.x, activeCheckpoint.transform.localScale.y, activeCheckpoint.transform.localScale.z);

        Invoke("LetPlayerWalk", .1f);
        

    }

    void LetPlayerWalk()
    {
        player.GetComponent<PlayerStateMachine>().TogglePlayerMoveState();
    }

    void OnSceneLoadCursorState(Scene scene, LoadSceneMode mode)
    {

        if (scene.name == "MainMenu" || scene.name == "GameOver")
        {
            Cursor.lockState = CursorLockMode.None;
        }
        if (scene.name == "Level1") 
        {
            canvasManager.playerScore = 0;
         //   Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void SaveGame()
    {
        // Print the path where the XML is saved
        Debug.Log(Application.persistentDataPath);

        // Call save game functionality
        StateManager.Save(Application.persistentDataPath + "/SaveGame.xml");
    }

    public void LoadGame()
    {
        bShouldLoad = true;

        StateManager.Load(Application.persistentDataPath + "/SaveGame.xml");

        // We could restart level if needed I think we do
    }

    public void setActiveCheckpoint(Checkpoint checkpoint)
    {
        activeCheckpoint = checkpoint;
    }

}
