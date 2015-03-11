using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	


	GameObject ScoreText;
	TextMesh scoreTextMesh;

	float levelSpeed;

	float oldDelay;

	int curLevel;

	public int highScore;

	public int score;

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
		lineRenderer.material = new Material(Shader.Find("Particles/Multiply"));
		lineRenderer.SetColors(c1, c2);
		lineRenderer.SetWidth(0.2F, 0.2F);


		polys = new List<Shape> ();

		ScoreText = GameObject.FindGameObjectWithTag ("scoretext");
		scoreTextMesh = ScoreText.GetComponent<TextMesh> ();


	
	}



	
	void Update () {

		shapes = GameObject.FindObjectsOfType<Shape> ();

		LineRenderer lineRenderer = GetComponent<LineRenderer>();


		pickRay = Camera.main.ScreenPointToRay (Input.mousePosition);

		
		lineRenderer.SetVertexCount (polys.Count);
		while(r < polys.Count) {
			lineRenderer.SetPosition(r,polys[r].gameObject.transform.position);
			r++;
		}
		r = 0;
		
		if (Input.GetMouseButtonUp (0)) {
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

			for(int i = 0; i< shapes.Length; i++) {
				shapes[i].SendMessage("SlowDown",false);


			}

			
		}
	
	}
}