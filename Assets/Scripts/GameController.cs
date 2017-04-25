using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public GameObject levelGenPrefab;
    public GameObject player, levelGenerator;
    private bool loadScene, sceneReady, pauseGame;
    private MainMenuController menu;
    private int levelSize;

    public enum GameState
    {
        Paused,
        Active,
        Loading
    };

    private GameState state;
    private string sceneToLoad;
    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        state = GameState.Loading;
        sceneReady = true;
        loadScene = false;
        pauseGame = false;
    }
	// Use this for initialization
	void Start () {
        //Debug.Log("Scene: " + SceneManager.GetActiveScene().name + " start.");
        if (!menu)
        {
            if (GameObject.FindGameObjectWithTag("MenuSystem"))
            {
                menu = GameObject.FindGameObjectWithTag("MenuSystem").GetComponent<MainMenuController>();
            }
        }
        if (!player)
        {
            if (GameObject.FindGameObjectWithTag("Player"))
            {
                player = GameObject.FindGameObjectWithTag("Player");
                player.SetActive(false);
            }
        }

        if (GameObject.FindGameObjectWithTag("LevelGenerator"))
        {
            levelGenerator = GameObject.FindGameObjectWithTag("LevelGenerator");
        }

        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }

        loadScene = false;
        sceneReady = true;
        pauseGame = false;
    }

    #region Private Methods
    
    public void LoadMe(string s)
    {
        loadScene = true;
        sceneToLoad = s;
    }

    public void PauseGame(bool p)
    {
        if (p)
        {
            //Debug.Log("PauseGame called");
            if (Time.timeScale > 0)
            {
                Time.timeScale = 0;
            }
            /* If there are any objects to disable or put to sleep, put those calls here. */
            if (Cursor.lockState != CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            player.SetActive(false);
        }
        else
        {
            //Debug.Log("ResumeGame called.");
            if (Time.timeScale < 1)
            {
                Time.timeScale = 1;
            }
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                Debug.Log("Locking the cursor.");
                Cursor.lockState = CursorLockMode.Locked;
            }

            pauseGame = false;
            player.SetActive(true);
            state = GameState.Active;
        }
        
    }
    private void StartLoadScene(string s)
    {
        if (!menu.ScreenClear)
        {
            StartCoroutine(AsynchronousLoad(s));
        }
    }
    public void QuitGame()
    {
        Debug.Log("QuitGame called.");
        Application.Quit();
    }
    #endregion

    #region Coroutines
    IEnumerator AsynchronousLoad(string scene)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(scene);
        ao.allowSceneActivation = false;
        sceneReady = false;
        state = GameState.Loading;
        if (player)
        {
            if (player.activeSelf)
            {
                player.SetActive(false);
            }
        }
        while (!ao.isDone)
        {
            float progress = Mathf.Clamp01(ao.progress / 0.9f);
            //Debug.Log("Loading Progress: " + (progress * 100) + "%");

            if (ao.progress == 0.9f)
            {
                ao.allowSceneActivation = true;
            }

            yield return null;
        }
        Start();
    }
    #endregion

    // Update is called once per frame
    void Update () {
        switch (state)
        {
            case GameState.Active:
                // if the game is flagged to load a scene
                if (loadScene)
                {
                    StartLoadScene(sceneToLoad);
                }
                else
                {
                    if (player)
                    {
                        if (!player.activeSelf)
                        {
                            player.SetActive(true);
                        }
                    }
                }
                if (Input.GetButtonDown("Pause"))
                {
                    state = GameState.Paused;
                    pauseGame = true;
                }
                break;
            case GameState.Loading:
                // If the current scene is not the main menu scene
                if (SceneManager.GetActiveScene().buildIndex > 0)
                {
                    if (levelGenerator)
                    {
                        if (levelGenerator.GetComponent<LevelGenerator>().LevelBuilt && sceneReady)
                        {
                            state = GameState.Paused;
                        }

                    }
                    else
                    {
                        levelGenerator = Instantiate(levelGenPrefab);
                    }
                }
                else
                {
                    if (sceneReady)
                    {
                        state = GameState.Paused;
                    }
                }
                
                break;
            case GameState.Paused:
                // If the current scene is not the main menu scene
                if (SceneManager.GetActiveScene().buildIndex > 0)
                {
                    // If the screen is clear 
                    if (menu.ScreenClear)
                    {
                        // The game is flagged to be paused
                        if (pauseGame)
                        {
                            // Pause the game
                            PauseGame(true);
                            // If the player presses the pause button
                            if (Input.GetButtonDown("Pause"))
                            {
                                // Flag to resume the game
                                PauseGame(false);
                            }
                            // If the game is supposed to load a scene
                            if (loadScene)
                            {
                                // Unpause the game
                                PauseGame(false);
                                // Load the specified scene
                                StartLoadScene(sceneToLoad);
                            }
                        }
                        // If the game is not flagged to be paused (this is to catch the initial load-in from another scene)
                        else
                        {
                            // Set the state to active
                            state = GameState.Active;
                        }

                    }


                }
                // If it is the main menu scene
                else
                {
                    // If the screen is clear
                    if (menu.ScreenClear)
                    {
                        // Set the state to active
                        state = GameState.Active;
                    }
                }
                break;
            
        }

        Debug.Log("Current game state: " + state.ToString());
	}

    public bool SceneReady
    {
        get { return sceneReady; }
    }

    public bool GamePaused
    {
        get { return pauseGame; }
    }

    public GameState State
    {
        get { return state; }
    }

    public int LevelSize
    {
        get { return levelSize; }
        set { levelSize = value; }
    }

}
