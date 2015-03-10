using UnityEngine;
using System.Collections;

public class Shape : MonoBehaviour {



	public int sides;
	public Color color;
	public float Speed = -0.1f;
	float SloMoSpeed;


	public GameObject gameManager;


	public float KillY;

	public bool hasBeenSelected = false;

	Renderer rend;
	bool isSloMo = false;

	public GameObject DestroyEffect;


	Mesh shapeMesh;

	public void SlowDown(bool sloMo){
		isSloMo = sloMo;
	}

	// Use this for initialization

	public virtual void Start () {

		gameManager = GameObject.FindGameObjectWithTag ("manager");
		rend = GetComponentsInChildren<MeshRenderer>()[0];
		rend.material.color = color;
	
	}




	
	
	// Update is called once per frame
	public virtual void Update () {

		SloMoSpeed = Speed / 10.0f;

		if (hasBeenSelected) {
			rend.material.color = new Color (255, 0, 0);
		} else {
			rend.material.color = color;
		}

		if (!isSloMo) {
			transform.position = transform.position + new Vector3 (0, Speed, 0);
		} else {
			transform.position = transform.position + new Vector3 (0, SloMoSpeed, 0);
		}


		if (transform.position.y <= KillY) {
			//Destroy(gameObject);

		}
	
	}
}
