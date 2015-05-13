using UnityEngine;
using System.Collections;

public class StreakBar : MonoBehaviour {

	Transform tr;
	Renderer rend;

	// Use this for initialization
	void Start () {


		tr = GetComponent<Transform> ();
		rend = GetComponent<Renderer> ();
	
	}
	
	// Update is called once per frame
	void Update () {

		tr.Rotate(2.0f,0.0f,0.0f);
	
	}
}
