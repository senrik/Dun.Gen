using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGenerator : MonoBehaviour {

    public GameObject menuSystemPrefab;
    private MainMenuController mainMenu;

    private void Awake()
    {
        
    }

    // Use this for initialization
    void Start () {
        if (!GameObject.FindGameObjectWithTag("MenuSystem"))
        {
            mainMenu = Instantiate(menuSystemPrefab).GetComponent<MainMenuController>();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
