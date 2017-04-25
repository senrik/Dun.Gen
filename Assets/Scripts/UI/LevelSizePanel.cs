using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSizePanel : MonoBehaviour {

    public Text roomNumber;
    public Slider roomSlider;
    private int levelSize;

    private void Update()
    {
        levelSize = (int)roomSlider.value;
    }

    private void LateUpdate()
    {
        roomNumber.text = roomSlider.value.ToString();
    }

    public int LevelSize
    {
        get { return levelSize; }
    }
}
