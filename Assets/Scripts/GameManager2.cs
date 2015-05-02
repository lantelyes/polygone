﻿using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public class GameManager2 : MonoBehaviour {


	int[] highScores = new int[5];

	
	public Texture2D emptyProgressBar; // Set this in inspector.
	public Texture2D fullProgressBar; // Set this in inspector.


	public GameObject scoreManager;
	List<Vector3> oldPositions;
	List<GameObject> destroyEffects;

	Collider[] toExplode;

	float ninjaProgress = 1.0f;
	

	public List<float> levelSpeeds;
	public List<AudioSource> connectSounds;

	public AudioSource explosionSound;
	public AudioSource music;

	public Rigidbody ninjaPopup;


	float audioOffset = 0.0f;

	public List<Color> levelBuildingColors; 
	public List<Color> levelSkyColors; 


	bool isNinja = false;
	int currentTier = 0;
	float t = 0.0f;
	public float fadeDuration = 1.0f;

	public List<int> streakTiers; 
	public List<Rigidbody> levelPopups; 
	public List<Rigidbody> popups;

	float scoreMultiplier = 1.0f; 

	public GameObject ScoreText;
	public GameObject MultiText;
	GameObject SkyObject;
	Renderer skyRend;
	public GameObject buildingObject;
	Renderer buildingRend;
	TextMesh scoreTextMesh;
	TextMesh multiTextMesh;
	bool gameOver = false;

	public Shader lineShader;
	int polyGones = 0;

	float levelSpeed;
	public GameObject slashObject;

	GameObject slash;


	float oldDelay;

	public int highScore;
	public int numPolysDestroyed = 0;
	
	public float fade = 0.0f;
	bool fading = false;

	public int score;
	public int currentLevel = 0;
	public int streaksNeeded = 5;
	public int currentStreak;
	public int topStreak;
	public float streakExpireTime = 0.0f;
	public List<int> polyGonesNeeded;
	int numLevels = 4;

	List<Shape> polys;
	int sidesNeeded;


	Ray pickRay;
	RaycastHit hit;


	public Color c1 = Color.yellow;
	public Color c2 = Color.red;

	public bool isChecking = false;

	public int lengthOfLineRenderer = 6;

	public Renderer rend;
	
	public List<Shape> shapes;

	int r = 0;

	void CleanUp() {
		for (int i = 0; i< shapes.Count; i++) {
			if(shapes[i] == null) {
				shapes.RemoveAt(i);
			}
		}

	}

	void StoreScores(int score) {

		for (int i = 0; i<highScores.Length; i++){

			string highScoreKey = "HighScore"+(i+1).ToString();
			highScore = PlayerPrefs.GetInt(highScoreKey,0);

			if(score>highScore){
				int temp = highScore;
				PlayerPrefs.SetInt(highScoreKey,score);
					score = temp;
			}
		}



	}


	void OnGUI() {

		if (isNinja) {
					
			GUI.DrawTexture (new Rect (Screen.width / 4, Screen.height * 0.9f, Screen.width / 2, Screen.height / 12), emptyProgressBar);
			GUI.DrawTexture (new Rect (Screen.width / 4, Screen.height * 0.9f, (Screen.width / 2) * ninjaProgress, Screen.height / 12), fullProgressBar);

		}

	}

	void StoreHighscore(int newHighscore)
	{
		int oldHighscore = PlayerPrefs.GetInt("highscore", 0);    
		if(newHighscore > oldHighscore)
			PlayerPrefs.SetInt("highscore", newHighscore);
	}

	// Use this for initialization
	void Start () {


		slash = (GameObject)Instantiate(slashObject,Camera.main.transform.position,Quaternion.identity);

		shapes = new List<Shape> ();

		oldPositions = new List<Vector3>();
	 	destroyEffects = new List<GameObject>();


		PopupLevel (0);
		LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.material = new Material(lineShader);
		lineRenderer.SetColors(c1, c2);
		lineRenderer.SetWidth(0.2F, 0.2F);



		polys = new List<Shape> ();

	
		scoreTextMesh = ScoreText.GetComponent<TextMesh> ();

		multiTextMesh = MultiText.GetComponent<TextMesh> ();

		buildingRend = buildingObject.GetComponent<Renderer> ();

		scoreManager = GameObject.FindGameObjectWithTag("ScoreManager");


	
	}


	public void Reset() {


		for (int i = 0; i < polys.Count; i++) {
			
			polys [i].hasBeenSelected = false;
		}
		
		
		
		polys.Clear ();
		isChecking = false;


	

	}

	void NinjaPopup() {
		int rand = Random.Range (0, 4);
		Rigidbody clone;
		clone = Instantiate (ninjaPopup, Camera.main.transform.position + new Vector3 (0, 3.0f, 7.0f), Quaternion.Euler (90.0f, 180.0f, 0.0f)) as Rigidbody;
	}


	void Popup() {
		int rand = Random.Range (0, 4);
		Rigidbody clone;
		clone = Instantiate (popups[rand], Camera.main.transform.position + new Vector3(Random.Range(-5,5),Random.Range(-5,5),10.0f), Quaternion.Euler(90.0f,180.0f,0.0f)) as Rigidbody;
		clone.AddTorque (new Vector3 (0.0f, 0.0f, 50.0f * Random.Range (-1, 1)));
	}
	void PopupLevel(int levelNum) {

		Rigidbody clone;
		if (currentLevel != numLevels) {
			clone = Instantiate (levelPopups [levelNum], Camera.main.transform.position + new Vector3 (0, 3.0f, 7.0f), Quaternion.Euler (90.0f, 180.0f, 0.0f)) as Rigidbody;
		}
	} 


	bool isAdjacent(Shape s1, Shape s2) {


		if(s1.hasBeenSelected && s2.hasBeenSelected) { 
			return false; 
		}

		float dist = Vector3.Distance (s1.transform.position, s2.transform.position);

		if (dist < 3.25f) {
			return true;
		} else {
			return false;
		}

	}


	
	void Update () {

		CleanUp ();


		music.pitch = 1 + audioOffset;

	

		if (gameOver) {

			StoreHighscore(score);

			Application.LoadLevel("new_menu");
		}



		if (!isNinja) {
			if(currentStreak > 0) 
			{
				streakExpireTime += Time.deltaTime;
			}
		}

		if (streakExpireTime >= 3.0f) {

			streakExpireTime = 0.0f;
			currentStreak = 0;
			audioOffset = 0.0f;
		}


		if (currentStreak == streakTiers [currentTier]) {
	
		
			scoreMultiplier += 1.0f;
			currentTier++;
		


		}

		if (currentStreak == 0) {
			scoreMultiplier = 1.0f;
		}

		if (polyGones >= polyGonesNeeded[currentLevel]) {

			if(currentLevel == 4) {
				gameOver = true;
			}
			if (currentLevel != levelPopups.Count) {
				fading = true;
				PopupLevel(currentLevel+1);
			}
		
			polyGones=0;


			for (int i = 0; i < shapes.Count; i++) {
				if(shapes[i] != null) {
					shapes [i].Respawn();
					Destroy (shapes [i].gameObject);
				}
			}
			 

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
			if (currentLevel != numLevels) {
				Color newCameraColor = Color.Lerp(levelSkyColors[currentLevel],levelSkyColors[currentLevel+1],t);
				Color newBuildingColor = Color.Lerp(levelBuildingColors[currentLevel],levelBuildingColors[currentLevel+1],t);
				Camera.main.backgroundColor = newCameraColor;
				buildingRend.material.color =  newBuildingColor;
			}

		}


		

		scoreTextMesh.text = (score * 1000.0f * scoreMultiplier).ToString();
		multiTextMesh.text = (scoreMultiplier) + "x";
			


		LineRenderer lineRenderer = GetComponent<LineRenderer>();


		pickRay = Camera.main.ScreenPointToRay (Input.mousePosition);

		
		lineRenderer.SetVertexCount (polys.Count);
		while(r < polys.Count) {
			if(!isNinja) {
				lineRenderer.SetPosition(r,polys[r].gameObject.transform.position);
				r++;
			}
		}
		r = 0;
		
		if (Input.GetMouseButtonUp (0) && !isNinja) {
			isChecking = false;

			for (int i = 0; i < polys.Count; i++) {
				
				polys [i].hasBeenSelected = false;
							
				
			}
		
			polys.Clear();
			r=0;


	
		}





		if(Input.GetMouseButtonDown(0) && !isChecking && !isNinja) {
			hit = new RaycastHit ();
			
			if (Physics.Raycast (pickRay, out hit, 10000.0f)) {
				
				Shape poly = (Shape)hit.collider.gameObject.GetComponent<Shape>();
			
				sidesNeeded = poly.sides;

				poly.hasBeenSelected = true;
				poly.isFirst = true;

				polys.Add(poly);

				isChecking = true;

				connectSounds[0].Play();
				
			}
		
		


		}

		if(isNinja){

			ninjaProgress -= Time.deltaTime/3.0f;

			slash.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,1.0f));
	
			t += Time.deltaTime/3.0f;
	
			hit = new RaycastHit ();
			if (Physics.Raycast (pickRay, out hit, 10000.0f)) {
			
				Shape poly = (Shape)hit.collider.gameObject.GetComponent<Shape> ();

				if(!poly.justAlive) {
					
					Instantiate(poly.DestroyEffect, poly.gameObject.transform.position + new Vector3(0,0,5), poly.gameObject.transform.rotation);
					score += poly.sides;
					poly.Respawn();
					Destroy (poly.gameObject);

				}
				
			}
			if(t>=1) {
				isNinja = false;
				ninjaProgress = 1.0f;
				t=0;
			}
		}


		if (isChecking && !isNinja) {



			for(int i = 0; i< shapes.Count; i++) {
				if(shapes[i] != null) {
					shapes[i].SendMessage("SlowDown",true);
				}
			}
	


			hit = new RaycastHit ();
			if (Physics.Raycast (pickRay, out hit, 10000.0f)) {
				Shape poly = (Shape)hit.collider.gameObject.GetComponent<Shape> ();
				if (!poly.hasBeenSelected && polys.Count != sidesNeeded) {
					if (poly.sides == sidesNeeded) {
						//if(isAdjacent(polys[polys.Count-1],poly)){

							poly.hasBeenSelected = true;
							polys.Add (poly);
							connectSounds[polys.Count - 1].Play();
						//}


					}
				}
				if(polys.Count == sidesNeeded && poly.isFirst) {
					poly.hasBeenSelected = true;
					polys.Add (poly);
					
				}
			}

		

			if (polys.Count == sidesNeeded) {

				oldPositions.Clear();
				destroyEffects.Clear();

			
				numPolysDestroyed += polys.Count - 1;

			

				score += polys[0].sides;


				
				for (int i = 0; i < polys.Count; i++) {

					polys[i].transform.rotation = Quaternion.identity;

					oldPositions.Add(polys[i].transform.position);
					destroyEffects.Add(polys[i].DestroyEffect);


					Instantiate(polys[i].DestroyEffect, polys[i].gameObject.transform.position + new Vector3(0,0,5), polys[i].gameObject.transform.rotation);
				
					polys [i].Respawn();
					Destroy (polys [i].gameObject);
					explosionSound.Play();    

				}
				polyGones++;


		
				//audioOffset += 0.04f/(1+currentStreak);
				currentStreak +=1 ;
				streakExpireTime = 0.0f;
				if(topStreak < 	currentStreak) {
					topStreak = currentStreak;
				}

				polys.Clear ();
				isChecking = false;

				if(currentStreak == streakTiers[0]){
					isNinja = true;
					NinjaPopup();

				}

				if(currentStreak == streakTiers[1]){
					Popup();
					for(int k = 0; k < oldPositions.Count; k++) {
							toExplode = Physics.OverlapSphere(oldPositions[k],4.0f);
							for(int i =0 ; i < toExplode.Length; i++) {
								if(toExplode[i].gameObject.tag == "shape") {
									Instantiate(toExplode[i].gameObject.GetComponent<Shape>().DestroyEffect, toExplode[i].gameObject.transform.position + new Vector3(0,0,5), Quaternion.identity );
									Shape temp = (Shape)toExplode[i].gameObject.GetComponent<Shape>();
									//temp.Respawn();
									//Destroy (toExplode[i].gameObject);
								}
							}
					}
					currentStreak = 0;
					ninjaProgress = 1.0f;
				}
			
			}
		}
		else {

			for(int i = 0; i< shapes.Count; i++) {
				if(shapes[i] != null) {
					shapes[i].SendMessage("SlowDown",false);
				}

			}

			
		}
	
	}
}