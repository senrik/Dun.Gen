using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour {

    public Text titleText, loadingText;
    public Button newGameButton;
    public Button quitGameButton;
    public LevelSizePanel levelSizePanel;
    public int levelSize;

    private void Update()
    {
        levelSize = levelSizePanel.LevelSize;
    }
}
