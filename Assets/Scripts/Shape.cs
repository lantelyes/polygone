using UnityEngine;
using System.Collections;

public class Shape : MonoBehaviour {



	public int sides;
	Color color;

	public GameObject gameManager;


	public float KillY;

	public bool hasBeenSelected = false;



	Mesh shapeMesh;

	// Use this for initialization

	void Start () {

		gameManager = GameObject.FindGameObjectWithTag ("manager");

	
	}


	public virtual void OnMouseOver(){

			
	}

	
	
	// Update is called once per frame
	public virtual void Update () {


		if (transform.position.y <= KillY) {
			//Destroy(gameObject);

		}
	
	}
}
