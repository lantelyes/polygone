using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		transform.position += new Vector3 (-10.0f * Time.deltaTime, 0, 0);
	
	}
}
