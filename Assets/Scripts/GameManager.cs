using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {


	float levelSpeed;

	int curLevel;

	int highScore;

	int score;

	List<Shape> polys;
	int sidesNeeded;

	Ray pickRay;
	RaycastHit hit;

	bool isChecking = false;

	// Use this for initialization
	void Start () {
		polys = new List<Shape> ();
		Time.timeScale = 0.5f;
	
	}



	
	void Update () {

		Time.fixedDeltaTime = 0.02F * Time.timeScale;

		pickRay = Camera.main.ScreenPointToRay (Input.mousePosition);

		if (Input.GetMouseButtonUp (0)) {
			Time.timeScale = 0.75f;
			isChecking = false;

			for (int i = 0; i < polys.Count; i++) {
				
				polys [i].hasBeenSelected = false;
				
				
				
			}
		
			polys.Clear();

	
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
			Time.timeScale = 0.1f;


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

					Destroy (polys [i].gameObject);
			


				}

				polys.Clear ();
				isChecking = false;
			
			}
		}
		else {
			Time.timeScale = 0.75f;
		
		}
	
	}
}