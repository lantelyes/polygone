using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	



	public Material level12;
	public Material level23;
	public Material level34;
	public Material level45;

	public List<Material> levelsMats; 


	GameObject ScoreText;
	GameObject SkyObject;
	Renderer skyRend;
	TextMesh scoreTextMesh;

	public Shader lineShader;

	float levelSpeed;


	float oldDelay;

	public int highScore;


	float fadeSpeed = 10.0f;
	float start = 0.0f;
	float end = 1.0f;
	public float fade = 0.0f;
	bool fading = false;

	public int score;
	public int currentLevel = 1;
	public int streaksNeeded = 1;
	public int currentStreak;
	public int topStreak;
	public float streakExpireTime = 3.0f;

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

		levelsMats = new List<Material>();
		levelsMats.Add (level12);
		levelsMats.Add (level23);
		levelsMats.Add (level34);
		levelsMats.Add (level45);

		polys = new List<Shape> ();

		ScoreText = GameObject.FindGameObjectWithTag ("scoretext");
		scoreTextMesh = ScoreText.GetComponent<TextMesh> ();


		SkyObject = GameObject.FindGameObjectWithTag ("sky");
		skyRend = SkyObject.GetComponent<Renderer> ();


	
	}


	public void Reset() {


		for (int i = 0; i < polys.Count; i++) {
			
			polys [i].hasBeenSelected = false;
		}
		
		
		
		polys.Clear ();
		isChecking = false;


	

	}


	
	void Update () {




		streakExpireTime -= Time.deltaTime;

		if (streakExpireTime < 0) {
			streakExpireTime = 3.0f;
			currentStreak = 0;
		}

		if (currentStreak == streaksNeeded) {
			fading = true;

		}

		if (fading) {
			fade = Mathf.Clamp (fade + Time.deltaTime * 10.0f, 0, 1);
			skyRend.material.SetFloat("_Blend",Mathf.Lerp(0.0f,1.0f,fade));
		
		
		}

		if (fade == 1.0f) {
			currentLevel++;
			skyRend.material = levelsMats [currentLevel-1];
			skyRend.material.SetFloat("_Blend",0.0f);
			print("new level");
			fade = 0.0f;
			fading = false;
			currentStreak = 0;
		}



			
		scoreTextMesh.text = "Score: " + score * 3000 + "\nStreak: " + currentStreak; 

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

		

			if (polys.Count == sidesNeeded + 1 ) {


				score += polys[0].sides;
				
				for (int i = 0; i < polys.Count; i++) {

					Instantiate(polys[i].DestroyEffect, polys[i].gameObject.transform.position, polys[i].gameObject.transform.rotation);

				

					Destroy (polys [i].gameObject);
		


				}

			
				currentStreak +=1;
				streakExpireTime = 3.0f;
				if(topStreak < 	currentStreak) {
					topStreak = currentStreak;
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