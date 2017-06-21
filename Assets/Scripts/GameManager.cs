﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	public GameObject player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();

		if (player == null) {
			SceneManager.UnloadSceneAsync("MainWorld");
			SceneManager.LoadScene("MainWorld");
		}
	}
}
