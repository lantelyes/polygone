using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public class GameManager2 : MonoBehaviour {


	public GameObject StreakBar;
	public GameObject levelIndicator;



	Renderer streakBarRend;

	bool isStreak = false;

	
	public Texture2D emptyProgressBar;
	public Texture2D fullProgressBar; 

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
	int topStreak;

	float decayTime = 2.0f;


	float oldDelay;

	public int highScore;
	public int numPolysDestroyed = 0;
	
	public float fade = 0.0f;
	bool fading = false;

	public int score;
	public int currentLevel = 0;
	public int streaksNeeded = 5;
	public int currentStreak;

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
	

	void OnGUI() {

		if (isNinja) {


		
			

		}

	}

	void StoreHighscore(int newHighscore)
	{
		int oldHighscore = PlayerPrefs.GetInt("highscore", 0);    
		if(newHighscore > oldHighscore)
			PlayerPrefs.SetInt("highscore", newHighscore);
		PlayerPrefs.Save ();
	}

	// Use this for initialization
	void Start () {

		highScore =  PlayerPrefs.GetInt ("hs");
		streakBarRend = StreakBar.GetComponent<Renderer> ();

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



		StreakBar.transform.localScale = Vector3.Lerp (new Vector3 (1.0f + (currentStreak), 1.0f, 1.0f), new Vector3 (1.0f, 1.0f, 1.0f),streakExpireTime/decayTime);

		if (currentStreak != 0) {
			streakBarRend.material.color = Color.Lerp (Color.red, levelSkyColors [currentLevel], streakExpireTime / decayTime);
		} else {
			streakBarRend.material.color = levelSkyColors [currentLevel];
		}
		music.pitch = 1;// + audioOffset;
		explosionSound.pitch = 1 + audioOffset;

	

		if (gameOver) {

			StoreHighscore(score * 100);




			Application.LoadLevel("gameover");



		}



		if (!isNinja) {
			if(currentStreak > 0) 
			{
				streakExpireTime += Time.deltaTime;
			}
		}

		if (streakExpireTime >= decayTime) {

			streakExpireTime = 0.0f;
			currentStreak = 0;
			audioOffset = 0.0f;
		}


		if (currentStreak == streakTiers [currentTier]) {
	
			currentTier++;
		


		}

		if (currentStreak == 0) {
			scoreMultiplier = 1.0f;
		}

		if (polyGones >= polyGonesNeeded[currentLevel] && !gameOver) {

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

					//shapes [i].Respawn();
				
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


		

		scoreTextMesh.text = (score * 100.0f * scoreMultiplier).ToString();
		multiTextMesh.text = "Level: " + (currentLevel + 1);
			


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

		if (Input.touchCount == 0 && !Input.GetMouseButton(0) ){

			for(int i = 0; i< shapes.Count; i++) {
				if(shapes[i] != null) {
					shapes[i].hasBeenSelected = false;
					shapes[i].isFirst = false;
				}

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
				polys.Clear();
			}
		}


		if (isChecking && !isNinja && !gameOver) {



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

			}

		

			if (polys.Count == sidesNeeded ) {

				if(currentStreak == streakTiers[1]){
					isNinja = true;
					NinjaPopup();
					
				}
				
				if(currentStreak == streakTiers[2]){
					
					
					for(int i = 0; i< shapes.Count ; i++) {
						Instantiate(shapes[i].DestroyEffect, shapes[i].gameObject.transform.position + new Vector3(0,0,5), shapes[i].gameObject.transform.rotation);
						
					
						shapes[i].Respawn();
						Destroy (shapes [i].gameObject);
					
						
						
					}

					shapes.Clear();
					isStreak = true;
					currentStreak=0;
					explosionSound.Play();
					
				}
				if(currentStreak == streakTiers[0]){
//		fh
//					for(int k = 0; k < oldPositions.Count; k++) {
//						Collider[] toExplode = Physics.OverlapSphere(oldPositions[k],4.0f);
//						for(int i =0 ; i < toExplode.Length; i++) {
//							if(toExplode[i].gameObject.tag == "shape") {
//								Instantiate(toExplode[i].gameObject.GetComponent<Shape>().DestroyEffect, toExplode[i].gameObject.transform.position + new Vector3(0,0,5), Quaternion.identity );
//								toExplode[i].gameObject.GetComponent<Shape>().Respawn();
//								Destroy (toExplode[i].gameObject);
//							}
//						}
//					}
//					isStreak = true;
				}
				numPolysDestroyed += polys.Count - 1;

			

				score += polys[0].sides;


				if(!isStreak) {
					for (int i = 0; i < polys.Count; i++) {

						polys[i].transform.rotation = Quaternion.identity;

						oldPositions.Add(polys[i].transform.position);
						destroyEffects.Add(polys[i].DestroyEffect);


						Instantiate(polys[i].DestroyEffect, polys[i].gameObject.transform.position + new Vector3(0,0,5), polys[i].gameObject.transform.rotation);
					

						polys [i].Respawn();
						Destroy (polys [i].gameObject);
					
						explosionSound.Play();    

					}
				}
				polyGones++;


		
				audioOffset += 0.1f/(1+currentStreak);
				currentStreak +=1 ;
				streakExpireTime = 0.0f;
				if(topStreak < 	currentStreak) {
					topStreak = currentStreak;
				}

				polys.Clear ();
				isChecking = false;


				isStreak = false;
			}
		}
		else {

			for(int i = 0; i< shapes.Count; i++) {
				if(shapes[i] != null) {
					shapes[i].SendMessage("SlowDown",false);
					shapes[i].hasBeenSelected = false;

				}

			}

			
		}
	
	}
}