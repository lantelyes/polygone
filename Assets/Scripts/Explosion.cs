using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	public float lifetime;

	// Use this for initialization
	void Start () {

		Destroy (gameObject, lifetime);

	
	}

	void OnApplicationQuit() {
		Destroy (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}
