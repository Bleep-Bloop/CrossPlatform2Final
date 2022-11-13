using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{

    [Header("Buttons")]
    public Button startButton;
    public Button quitButton;
    public Button mainMenuButton;
    public Image playerHealthSprite;

    // Start is called before the first frame update
    void Start()
    {

        if (startButton)
            startButton.onClick.AddListener(() => StartGame());

        if (quitButton)
            quitButton.onClick.AddListener(() => QuitGame());

        if (mainMenuButton)
            mainMenuButton.onClick.AddListener(() => MainMenu());

        if (playerHealthSprite)
        {
            HealthComponent playerHealthComponent = GameObject.FindObjectOfType<HealthComponent>();
            if (playerHealthComponent)
            {
                Debug.Log("Listener added");
                playerHealthComponent.playerHealthChanged.AddListener(() => OnHealthChanged());
                //GameObject.FindObjectOfType<HealthComponent>().playerHealthChanged.AddListener(() => OnHealthChanged);
            }
        }  

    }

    // Update is called once per frame
    void Update()
    {

    }

    void StartGame()
    {
        SceneManager.LoadScene("Level1");
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

}
