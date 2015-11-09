using UnityEngine;
using System.Collections;

public class Ninja : MonoBehaviour {

	Transform tr;
	Renderer rend;
	float fade = 0.0f;
	public float fadeSpeed = 1.0f;

	// Use this for initialization
	void Start () {
		tr = GetComponent<Transform> ();
		rend = GetComponent<Renderer> ();
	}
	
	// Update is called once per fsrame
	void Update () {

		tr.localScale = (new Vector3 (1.0f, 1.0f, 1.0f) * fade);

		fade = Mathf.Sin (Time.deltaTime/50.0f);



		if (rend.material.color.a <= 0) {
			//Destroy(gameObject);
		}

	
	}
}
