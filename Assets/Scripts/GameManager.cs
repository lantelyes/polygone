using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {


	float timeScaleFactor = 0.001f;


	float levelSpeed;

	int curLevel;

	int highScore;

	int score;

	List<Shape> polys;
	int sidesNeeded;

	Ray pickRay;
	RaycastHit hit;

	public Color c1 = Color.yellow;
	public Color c2 = Color.red;

	bool isChecking = false;

	public int lengthOfLineRenderer = 6;

	public Renderer rend;

	public Shape[] shapes;

	int r = 0;

	// Use this for initialization
	void Start () {
		LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		lineRenderer.SetColors(c1, c2);
		lineRenderer.SetWidth(0.2F, 0.2F);


		polys = new List<Shape> ();
		Time.timeScale = 0.5f;
	
	}



	
	void Update () {

		shapes = GameObject.FindObjectsOfType<Shape> ();

		LineRenderer lineRenderer = GetComponent<LineRenderer>();

		//Time.fixedDeltaTime = 0.02F * Time.timeScale;

		pickRay = Camera.main.ScreenPointToRay (Input.mousePosition);

		
		lineRenderer.SetVertexCount (polys.Count);
		while(r < polys.Count) {
			lineRenderer.SetPosition(r,polys[r].gameObject.transform.position);
			r++;
		}
		r = 0;
		
		if (Input.GetMouseButtonUp (0)) {
			Time.timeScale = 0.75f + timeScaleFactor;
			isChecking = false;

			for (int i = 0; i < polys.Count; i++) {
				
				polys [i].hasBeenSelected = false;
				
				
				
			}
		
			polys.Clear();
			r=0;


	
		}


		if(Input.GetMouseButtonDown(0) && !isChecking) {
			hit = new RaycastHit ();
			
			if (Physics.Raycast (pickRay, out hit, 10000.0f)) {
				
				Shape poly = (Shape)hit.collider.gameObject.GetComponent<Shape>();
			
				sidesNeeded = poly.sides;

				poly.hasBeenSelected = true;

				polys.Add(poly);
				isChecking = true;
				
			}
		
		


		}


		if (isChecking) {

	
			for(int i = 0; i< shapes.Length; i++) {
				shapes[i].SendMessage("SlowDown",true);
			}
			//Time.timeScale = 0.1f + timeScaleFactor;


			hit = new RaycastHit ();
			if (Physics.Raycast (pickRay, out hit, 10000.0f)) {
				Shape poly = (Shape)hit.collider.gameObject.GetComponent<Shape> ();
				if (!poly.hasBeenSelected) {
					if (poly.sides == sidesNeeded) {

						poly.hasBeenSelected = true;
						polys.Add (poly);

					}
				}
			}
			if (polys.Count == sidesNeeded) {

				for (int i = 0; i < polys.Count; i++) {

					Instantiate(polys[i].DestroyEffect, polys[i].gameObject.transform.position, polys[i].gameObject.transform.rotation);

					Destroy (polys [i].gameObject);
			


				}

				polys.Clear ();
				isChecking = false;
			
			}
		}
		else {
			//Time.timeScale = 0.75f + timeScaleFactor;
			for(int i = 0; i< shapes.Length; i++) {
				shapes[i].SendMessage("SlowDown",false);
			}
			
		}
	
	}
}