using UnityEngine;
using System.Collections;

public class Shape : MonoBehaviour {



	public int sides;
	public Color color;
	public Color outlineColor;
	public float Speed = -0.1f;
	public bool justAlive = true;
	Transform tr;
	float fade = 0.0f;
	float fadeDuration = 0.5f;
	float t = 0.0f;
	Collider col;


	public Spawner2 originSpawner;

	GameManager2 gameManager;
	
	public float KillY;

	public bool hasBeenSelected = false;
	public bool isFirst = false;

	Renderer rend;
	bool isSloMo = false;

	public GameObject DestroyEffect;

	Mesh shapeMesh;

	public void SlowDown(bool sloMo){
		isSloMo = sloMo;
	}

	// Use this for initialization

	public virtual void Start () {

		gameManager = GameObject.FindGameObjectWithTag ("manager").GetComponent<GameManager2>();
		gameManager.shapes.Add (this);

		tr = GetComponent<Transform> ();
		col = GetComponent<Collider> ();


		rend = GetComponentsInChildren<MeshRenderer>()[0];
		rend.material.color = color;
		rend.material.SetColor ("_OutlineColor", outlineColor);



	}

	public virtual void OnExplode () {

	
	}

	public void SetSpawner(Spawner2 sp) {
		originSpawner = sp;
	}

	public void Respawn() {
		originSpawner.Spawn ();

	}
	

	
	// Update is called once per frame
	public virtual void Update () {


		if (gameManager.currentStreak > 0) {

			tr.Rotate(0,gameManager.currentStreak * 1.0f,0);
		}



		if(justAlive){
			tr.localScale = new Vector3(0.1f,0.1f,0.1f);
			if(t < 1) {
				t += Time.deltaTime/fadeDuration;
				tr.localScale = Vector3.Lerp (new Vector3(0.1f,0.1f,0.1f), new Vector3(1.0f,1.0f,1.0f), t);
			}
			if(t >= 1) {
				justAlive = false;
				t = 0;
				
				
			}

		}



		
		if (isFirst && hasBeenSelected) {
			transform.Rotate(0,gameManager.currentStreak,0);
		}
		if (!hasBeenSelected && gameManager.currentStreak == 0) {
			transform.rotation = Quaternion.identity;
		}



		if (hasBeenSelected) {
			rend.material.color = new Color (255, 0, 0);
		} else {
			rend.material.color = color;
		}

		if (!isSloMo) {
			Time.timeScale = 1.0f;
			transform.position = transform.position + new Vector3 (0, Speed * Time.deltaTime, 0);
		} else {
			Time.timeScale = .5f;
			transform.position = transform.position + new Vector3 (0, Speed * Time.deltaTime, 0);
		}


		if (transform.position.y <= KillY) {
			if(hasBeenSelected) {
				gameManager.Reset();
			}
			Destroy(gameObject);

		}
	
	}
}
