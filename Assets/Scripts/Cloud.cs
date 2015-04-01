using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour {


	Vector3 origPos;
	// Use this for initialization
	void Start () {
	
		origPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		transform.position += new Vector3 (-5.0f * Time.deltaTime, 0, 0);

		if (transform.position.x <= -25) {
			transform.position = origPos + new Vector3(0,Random.Range(-5,5));
			transform.localScale.Scale(new Vector3(Random.Range(0.75f,1.25f),Random.Range(0.75f,1.25f),Random.Range(0.75f,1.25f)));
		}
	
	}
}
