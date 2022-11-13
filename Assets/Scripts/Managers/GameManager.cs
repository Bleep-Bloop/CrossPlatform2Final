using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    private void Awake()
    {
        Debug.Log("Awake test");
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoadCursorState;

    }

    // Update is called once per frame
    void Update()
    {
        
        // Just for debugging
        if(Input.GetKey(KeyCode.Q))
        {
            LockCursor();
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
    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    void OnSceneLoadCursorState(Scene scene, LoadSceneMode mode)
    {

        if (scene.name == "MainMenu" || scene.name == "GameOver")
        {
            Cursor.lockState = CursorLockMode.None;
        }
        if (scene.name == "Level1") 
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

}
