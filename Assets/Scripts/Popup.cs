using UnityEngine;
using System.Collections;

public class Popup : MonoBehaviour {

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

		tr.localScale += (new Vector3 (5.0f, 5.0f, 5.0f) * fadeSpeed *  Time.deltaTime / 5.0f);

		fade += Time.deltaTime * fadeSpeed / 5.0f;

		rend.material.color = (Color.Lerp(Color.grey,Color.clear,fade));

		if (rend.material.color.a <= 0) {
			Destroy(gameObject);
		}

	
	}
}
