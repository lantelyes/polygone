using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour {

	public List<int> highScores = new List<int>();
	public List<Text> textBoxes = new List<Text>();
	public GameObject scoreObject;
	Text texts;


	// Use this for initialization
	void Start () {



		GameObject textMeshObject = GameObject.FindGameObjectWithTag ("scoretext");
		for (int i = 0; i<4; i++) {
			textBoxes.Add(GameObject.FindGameObjectWithTag(i.ToString()).GetComponent<Text>());
		}

		Sort ();

		DontDestroyOnLoad (gameObject);
	
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < highScores.Count ; i++) {
			textBoxes[i].text = (i+1).ToString() + ": " + highScores[i].ToString(); 
			if( i >=4 ){
				break;
			}
		}
	
	}

	public void Sort() {
		highScores.Sort ();
		highScores.Reverse ();
	}


	public void AddScore(int score) {
		print (score);
		highScores.Add (score);
		Sort ();

	}
}
