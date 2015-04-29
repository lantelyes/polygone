using UnityEngine;
using System.Collections;

public class Shape : MonoBehaviour {



	public int sides;
	public Color color;
	public Color outlineColor;
	public float Speed = -0.1f;
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


		rend = GetComponentsInChildren<MeshRenderer>()[0];
		rend.material.color = color;
		rend.material.SetColor ("_OutlineColor", outlineColor);



	}

	public virtual void OnExplode () {

	
	}

	public void Respawn() {
		originSpawner.Spawn ();
	}
	

	
	// Update is called once per frame
	public virtual void Update () {


		if (isFirst && hasBeenSelected) {
			transform.Rotate(0,5,0);
		}
		if (!hasBeenSelected) {
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
