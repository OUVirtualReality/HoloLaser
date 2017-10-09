using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

	public Color goalColor;

	public Goal(Color color)
	{
		goalColor = color;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// if hit send hit flag to game setup
	}
}
