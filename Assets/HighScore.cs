using UnityEngine;
using System.Collections;

public class HighScore : MonoBehaviour {


	public int HighScoreNum;
	public GameObject HighScoreText;
	// Use this for initialization
	void Start () {
		PlayerPrefs.GetInt ("highscore");
		HighScoreNum = PlayerPrefs.GetInt ("highscore");
		TextMesh tMesh =  HighScoreText.GetComponent<TextMesh> ();
		tMesh.text = "High Score: " + HighScoreNum;
	
	}
	
	// Update is called once per frame
	void Update () {

	
	
	}
}
