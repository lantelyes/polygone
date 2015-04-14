﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public Rigidbody projectile;

	public List<float> levelSpeeds;

	public List<Color> levelBuildingColors; 
	public List<Color> levelSkyColors; 

	float t = 0.0f;
	public float fadeDuration = 1.0f;

	public List<int> streakTiers; 
	int currentTier = 0;

	float scoreMultiplier = 1.0f; 

	GameObject ScoreText;
	GameObject SkyObject;
	Renderer skyRend;
	public GameObject buildingObject;
	Renderer buildingRend;
	TextMesh scoreTextMesh;
	bool gameOver = false;

	public Shader lineShader;
	int polyGones = 0;

	float levelSpeed;


	float oldDelay;

	public int highScore;
	public int numPolysDestroyed = 0;


	float fadeSpeed = 1.0f;
	float start = 0.0f;
	float end = 1.0f;
	public float fade = 0.0f;
	bool fading = false;

	public int score;
	public int currentLevel = 0;
	public int streaksNeeded = 5;
	public int currentStreak;
	public int topStreak;
	public float streakExpireTime = 3.0f;
	public List<int> polyGonesNeeded;

	List<Shape> polys;
	int sidesNeeded;


	Ray pickRay;
	RaycastHit hit;

	public Color c1 = Color.yellow;
	public Color c2 = Color.red;

	public bool isChecking = false;

	public int lengthOfLineRenderer = 6;

	public Renderer rend;

	public Shape[] shapes;

	int r = 0;

	// Use this for initialization
	void Start () {
		LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.material = new Material(lineShader);
		lineRenderer.SetColors(c1, c2);
		lineRenderer.SetWidth(0.2F, 0.2F);



		polys = new List<Shape> ();

		ScoreText = GameObject.FindGameObjectWithTag ("scoretext");
		scoreTextMesh = ScoreText.GetComponent<TextMesh> ();

		buildingRend = buildingObject.GetComponent<Renderer> ();


	
	}


	public void Reset() {


		for (int i = 0; i < polys.Count; i++) {
			
			polys [i].hasBeenSelected = false;
		}
		
		
		
		polys.Clear ();
		isChecking = false;


	

	}


	
	void Update () {


		if (Input.GetButtonDown ("Fire1")) {
			//Rigidbody clone;
			//clone = Instantiate (projectile, Camera.main.transform.position + new Vector3(0.0f,0.0f,50.0f), Quaternion.Euler(90.0f,180.0f,0.0f)) as Rigidbody;
			//clone.velocity = transform.TransformDirection (Vector3.forward * -30);
		}

		if (currentLevel == 4) {

		}




		streakExpireTime -= Time.deltaTime;

		if (streakExpireTime < 0) {
			streakExpireTime = 3.0f;
			currentStreak = 0;
		}


		if (currentStreak == streakTiers [currentTier]) {
			print ("Streak");
		
			scoreMultiplier += 1.0f;

			currentTier++;
		


		}

		if (currentStreak == 0) {
			scoreMultiplier = 1.0f;
		}

		if (polyGones >= polyGonesNeeded[currentLevel]) {
			fading = true;
			polyGones=0;
		}

		if (fading) {
			if(t < 1) {
				t += Time.deltaTime/fadeDuration;
			}
			if(t >= 1) {
				fading = false;
				currentLevel++;
				numPolysDestroyed = 0;
				t = 0;
			}
			Color newCameraColor = Color.Lerp(levelSkyColors[currentLevel],levelSkyColors[currentLevel+1],t);
			Color newBuildingColor = Color.Lerp(levelBuildingColors[currentLevel],levelBuildingColors[currentLevel+1],t);
			Camera.main.backgroundColor = newCameraColor;
			buildingRend.material.color =  newBuildingColor;

		}
	

		
		
		
		//scoreTextMesh.text = "Score: " + score * 3000 + "\nStreak: " + currentStreak; 
		scoreTextMesh.text = "Score: " + (score * 3000 * scoreMultiplier)+ "\nPolys: " + numPolysDestroyed + "\nStreak: " + currentStreak + "\nMultiplier: " + scoreMultiplier + "x";
			

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
				poly.isFirst = true;

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
				if (!poly.hasBeenSelected && polys.Count != sidesNeeded) {
					if (poly.sides == sidesNeeded) {

						poly.hasBeenSelected = true;
						polys.Add (poly);

					}
				}
				if(polys.Count == sidesNeeded && poly.isFirst) {
					poly.hasBeenSelected = true;
					polys.Add (poly);
					
				}
			}

		

			if (polys.Count == sidesNeeded) {

				List<Vector3> oldPositions = new List<Vector3>();
				List<GameObject> destroyEffects = new List<GameObject>();

				score += polys[0].sides;
				numPolysDestroyed += polys.Count - 1;

	
				
				for (int i = 0; i < polys.Count; i++) {

					polys[i].transform.rotation = Quaternion.identity;

					oldPositions.Add(polys[i].transform.position);
					destroyEffects.Add(polys[i].DestroyEffect);


					Instantiate(polys[i].DestroyEffect, polys[i].gameObject.transform.position + new Vector3(0,0,5), polys[i].gameObject.transform.rotation);
				

					Destroy (polys [i].gameObject);

				}

				polyGones++;

		
			
				currentStreak +=1;
				streakExpireTime = 3.0f;
				if(topStreak < 	currentStreak) {
					topStreak = currentStreak;
				}

				polys.Clear ();
				isChecking = false;

				if(currentStreak == streakTiers[0]){
					for(int k = 0; k < oldPositions.Count; k++) {
							Collider[] toExplode = Physics.OverlapSphere(oldPositions[k],4.0f);
							for(int i =0 ; i < toExplode.Length; i++) {
								Instantiate(toExplode[i].gameObject.GetComponent<Shape>().DestroyEffect, toExplode[i].gameObject.transform.position + new Vector3(0,0,5), Quaternion.identity );
								Destroy (toExplode[i].gameObject);
							}
					}
				}
			
			}
		}
		else {

			for(int i = 0; i< shapes.Length; i++) {
				shapes[i].SendMessage("SlowDown",false);

			}

			
		}
	
	}
}