using UnityEngine;
using System.Collections;

public class Shape : MonoBehaviour {

	public GameObject ScoreText;

	public int sides;
	public Color color;
	public float Speed = -0.1f;
	float SloMoSpeed;


	public GameObject gameManager;
	private TextMesh scoreTextMesh;


	public float KillY;

	public bool hasBeenSelected = false;

	Renderer rend;
	bool isSloMo = false;

	public GameObject DestroyEffect;

	GameManager gm;


	Mesh shapeMesh;

	public void SlowDown(bool sloMo){
		isSloMo = sloMo;
	}

	// Use this for initialization

	public virtual void Start () {

		gameManager = GameObject.FindGameObjectWithTag ("manager");
		ScoreText = GameObject.FindGameObjectWithTag ("scoretext");


		rend = GetComponentsInChildren<MeshRenderer>()[0];
		rend.material.color = color;
		scoreTextMesh = ScoreText.GetComponent<TextMesh> ();

		gm = gameManager.GetComponent<GameManager> ();

	}

	public virtual void OnDestroy () {

		gm.score += sides;
	}




	
	
	// Update is called once per frame
	public virtual void Update () {

		SloMoSpeed = Speed / 10.0f;

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
			//Destroy(gameObject);

		}
	
	}
}
