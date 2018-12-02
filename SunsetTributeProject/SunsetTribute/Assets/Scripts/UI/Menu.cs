using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Menu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("StartP1") || Input.GetButtonDown("StartP2"))
        {
            SceneManager.LoadScene("Demo_Level_2");
        }
	}
}
