using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class CanvasManager : MonoBehaviour
{

    bool gamePaused;
    public GameObject pauseMenu;
    public GameObject deathMenu;

    public ThirdPersonShooterController playerRef;
    public GameObject[] checkpointRefs;

    // get all checkpoints in map and add each to list

    [Header("Buttons")]
    [Header("MainMenu")]
    public Button startButton;
    public Button quitButton;
    public Button mainMenuButton;
    public Image playerHealthSprite;
    public TextMeshProUGUI playerScoreText;

    public int playerScore = 0;

    [Header("PauseMenu")]
    public Button resumeButton;
    public Button saveButton;
    public Button loadButton;

    [Header("DeathMenu")]
    public Button continueButton;

    [Header("GameOver")]
    public Image GameOverBackground;



    // Start is called before the first frame update
    void Start()
    {

        // Main Menu
        if (startButton)
            startButton.onClick.AddListener(() => StartGame());

        if (quitButton)
            quitButton.onClick.AddListener(() => QuitGame());

        if (mainMenuButton)
            mainMenuButton.onClick.AddListener(() => MainMenu());

        // Pause Menu
        if (resumeButton)
            resumeButton.onClick.AddListener(() => PauseGame());

        if (saveButton)
            saveButton.onClick.AddListener(() => SaveGame());

        if (loadButton)
            loadButton.onClick.AddListener(() => LoadGame());

        // Death Menu
        if (continueButton)
            continueButton.onClick.AddListener(() => Respawn());


        if (playerHealthSprite)
        {
            HealthComponent playerHealthComponent = GameObject.FindObjectOfType<HealthComponent>();
            if (playerHealthComponent)
            {
                playerHealthComponent.playerHealthChanged.AddListener(() => OnHealthChanged());
                //GameObject.FindObjectOfType<HealthComponent>().playerHealthChanged.AddListener(() => OnHealthChanged);
            }
        }

        if(playerScoreText)
        {
            playerScoreText.text = "0";
        }

        
        checkpointRefs = GameObject.FindGameObjectsWithTag("Checkpoint");

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
        if(Input.GetKeyDown(KeyCode.J))
        {
            toggleDeathMenu();
        }

        playerScoreText.text = playerScore.ToString();


    }

    void StartGame()
    {
        SceneManager.LoadScene("Level1");

        playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonShooterController>();

        checkpointRefs = GameObject.FindGameObjectsWithTag("Checkpoint");

        

    }

    void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // ToDo: Handle if player can take more then one hit.
    // Currently dies in one hit so this will work for now.
    void OnHealthChanged()
    {
        playerHealthSprite.enabled = false;
    }

    public void PauseGame()
    {
        gamePaused = !gamePaused;
        pauseMenu.SetActive(gamePaused);

        if (gamePaused)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
        
    }

    // ToDo: Listener to auto call on each character using? Maybe harder to read overall if done that way.
    public void SaveGame()
    {
        playerRef.SaveGamePrepare();

        for (int i = 0; i < checkpointRefs.Length; i++)
        {
            checkpointRefs[i].GetComponent<Checkpoint>().SaveGamePrepare();
            Debug.Log("Saved on " + checkpointRefs[i].name);
        }

        GameManager.Instance.SaveGame();

    }

    public void LoadGame()
    {

        Debug.Log("LoadGame() Canvas Manager");

        playerRef.LoadGameComplete();
        
        for (int i = 0; i < checkpointRefs.Length; i++)
        {
            checkpointRefs[i].GetComponent<Checkpoint>().LoadGameComplete();
            Debug.Log("Load on " + checkpointRefs[i].name);
        }

        playerRef.GetComponent<HealthComponent>().isDead = false;
        GameManager.Instance.LoadGame();

    }

    public void toggleDeathMenu()
    {

        if(!deathMenu.activeSelf)
        {
            deathMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            deathMenu.SetActive(false);
            Time.timeScale = 1;
        }



    }

    public void Respawn()
    {
        toggleDeathMenu();
        GameManager.Instance.Respawn(); 
    }

    // NO idea why this is not working
    public void ToggleGameOverImage(bool isVisible)
    {
            GameOverBackground.enabled = isVisible;

    }

}
