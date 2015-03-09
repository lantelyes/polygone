using UnityEngine;
using System.Collections;

public class Shape : MonoBehaviour {



	public int sides;
	Color color;

	public GameObject gameManager;


	public float KillY;

	public bool hasBeenSelected = false;

	Renderer rend;

	public GameObject DestroyEffect;


	Mesh shapeMesh;

	// Use this for initialization

	public virtual void Start () {

		gameManager = GameObject.FindGameObjectWithTag ("manager");
		rend = GetComponentsInChildren<MeshRenderer>()[0];
	
	}


	public virtual void OnDestroy() {
		Instantiate(DestroyEffect, transform.position, transform.rotation);

	}

	
	
	// Update is called once per frame
	public virtual void Update () {

		if (hasBeenSelected) {
			rend.material.color = new Color (255, 0, 0);
		} else {
			rend.material.color = new Color (0, 255, 0);
		}


		if (transform.position.y <= KillY) {
			//Destroy(gameObject);

		}
	
	}
}
