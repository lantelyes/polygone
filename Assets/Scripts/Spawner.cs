using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {


	public GameObject shapeToSpawn;
	public int delay = 5;
	public bool spawning = false;



	// Use this for initialization
	void Start () {

		Invoke("Spawn", delay);


	
	}


	void Spawn() {

		if(spawning) {

			Instantiate (shapeToSpawn, transform.position, transform.rotation);
			Invoke("Spawn", delay);
	
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
