using UnityEngine;
using System.Collections;

public class Shape : MonoBehaviour {



	int sides;
	Color color;

	public float KillY;

	Mesh shapeMesh;

	// Use this for initialization

	void Start () {
	
	}
	
	// Update is called once per frame
	public virtual void Update () {
		if (transform.position.y <= KillY) {
			Destroy(gameObject);

		}
	
	}
}
