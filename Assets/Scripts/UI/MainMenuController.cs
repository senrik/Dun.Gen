using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    public MainMenuPanel mainPanel;
    public PauseMenuController pauseMenu;
    public Button confirmButton, backButton;
    public Image fadeImage;
    public GameObject hud;
    public GameObject gc_prefab;
    private GameController _game;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

	// Use this for initialization
	void Start () {
        if(!_game)
        {
            if (GameObject.FindGameObjectWithTag("GameController"))
            {
                _game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
                BindGameMethods();
            }
            else
            {
                Instantiate(gc_prefab);
                _game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
                BindGameMethods();
            }
        }
	}

    void BindGameMethods()
    {
        /* Bind Main Menu Buttons */
        mainPanel.newGameButton.onClick.AddListener(delegate { GetComponent<Animator>().SetBool("sceneReady", false); _game.LevelSize = mainPanel.levelSize; _game.LoadMe("playScene"); });
        mainPanel.quitGameButton.onClick.AddListener(delegate { GetComponent<Animator>().SetTrigger("QuitGame"); });
        /**/
        /* Bind Confirmation Panel Buttons */
        backButton.onClick.AddListener(delegate { GetComponent<Animator>().SetTrigger("Deny"); });
        confirmButton.onClick.AddListener(delegate { _game.QuitGame(); });
        /* Bind Pause Menu Buttons */
        pauseMenu.resumeButton.onClick.AddListener(delegate { _game.PauseGame(false); });
        pauseMenu.mainMenuButton.onClick.AddListener(delegate { GetComponent<Animator>().SetBool("sceneReady", false); _game.LoadMe("mainMenu"); });
        pauseMenu.quitButton.onClick.AddListener(delegate { _game.QuitGame(); });
    }

	void ActiveMainMenu(bool h)
    {
        if (h)
        {
            // Show the main menu
            if (mainPanel.GetComponent<CanvasGroup>().alpha < 1)
            {
                mainPanel.GetComponent<CanvasGroup>().alpha = 1;
                mainPanel.GetComponent<CanvasGroup>().interactable = true;
                mainPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
        }
        else
        {
            // Hide the main menu
            if (mainPanel.GetComponent<CanvasGroup>().alpha > 0)
            {
                mainPanel.GetComponent<CanvasGroup>().alpha = 0;
                mainPanel.GetComponent<CanvasGroup>().interactable = false;
                mainPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }
       
    }

    void ActivePauseMenu(bool b)
    {
        if (b)
        {
            if(pauseMenu.GetComponent<CanvasGroup>().alpha < 1)
            {
                pauseMenu.GetComponent<CanvasGroup>().alpha = 1;
                pauseMenu.GetComponent<CanvasGroup>().interactable = true;
                pauseMenu.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
        }
        else
        {
            if (pauseMenu.GetComponent<CanvasGroup>().alpha > 0)
            {
                pauseMenu.GetComponent<CanvasGroup>().alpha = 0;
                pauseMenu.GetComponent<CanvasGroup>().interactable = false;
                pauseMenu.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }
    }
	// Update is called once per frame
	void Update () {

    }

    void LateUpdate()
    {
        if (_game)
        {
            switch (_game.State)
            {
                case GameController.GameState.Active:
                    if (SceneManager.GetActiveScene().buildIndex > 0)
                    {
                        if (mainPanel.GetComponent<CanvasGroup>().alpha > 0)
                        {
                            ActiveMainMenu(false);
                        }
                        if (pauseMenu.GetComponent<CanvasGroup>().alpha > 0)
                        {
                            ActivePauseMenu(false);
                        }

                        if(hud.GetComponent<CanvasGroup>().alpha < 1)
                        {
                            hud.GetComponent<CanvasGroup>().alpha = 1;
                        }
                    }
                    else
                    {
                        if (mainPanel.GetComponent<CanvasGroup>().alpha < 1)
                        {
                            ActiveMainMenu(true);
                        }

                        if (hud.GetComponent<CanvasGroup>().alpha > 0)
                        {
                            hud.GetComponent<CanvasGroup>().alpha = 0;
                        }
                    }
                    break;
                case GameController.GameState.Loading:
                    break;
                case GameController.GameState.Paused:
                    if (_game.SceneReady)
                    {
                        GetComponent<Animator>().SetBool("sceneReady", true);
                    }
                    if (SceneManager.GetActiveScene().buildIndex > 0)
                    {
                        if (_game.GamePaused)
                        {
                            if (pauseMenu.GetComponent<CanvasGroup>().alpha < 1)
                            {
                                ActivePauseMenu(true);
                            }
                        }
                    }
                    break;
            }
        }
    }

    public bool ScreenClear
    {
        get { return (fadeImage.GetComponent<CanvasGroup>().alpha > 0.9f) ? false : true; }
    }
}
