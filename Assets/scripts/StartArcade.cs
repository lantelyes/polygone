using UnityEngine;
using System.Collections;

public class StartArcade : MonoBehaviour {


	public GameObject creditsButton;
	public GameObject tutorialButton;

	bool tutorialTrigger = false;
	bool creditsTrigger = false; 

	public void StartLevel()
	{
		Application.LoadLevel ("TestScene");
	}

	public void StartTutorial()
	{
	//	print ("Hey");
		tutorialTrigger = true;

	}

	public void StartCredits()
	{
		creditsTrigger = true;
	}
	
//	if(tutorialTrigger)
//	{
//		Handheld.PlayFullScreenMovie("polygone_tutorial.mp4", Color.black, FullScreenMovieControlMode.Full, FullScreenMovieScalingMode.AspectFill);
//		tutorialTrigger = false;
//	}
//
//	else if(creditsTrigger)
//	{
//		Handheld.PlayFullScreenMovie("credits.mp4", Color.black, FullScreenMovieControlMode.Full, FullScreenMovieScalingMode.AspectFill);
//		creditsTrigger = false;
//	}

	public MovieTexture tutorial;
	public MovieTexture credits;

	void OnGUI()
	{
		if (tutorialTrigger) {
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), tutorial);
			tutorial.Play ();
			StartCoroutine (Wait (tutorial.duration));

		} else if (creditsTrigger) {
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), credits);
			credits.Play ();
			StartCoroutine (Wait (credits.duration));
		}
	}

	private IEnumerator Wait(float duration)
	{
		yield return new WaitForSeconds(duration);
		if (tutorialTrigger) {
			tutorial.Stop ();
			tutorialTrigger = false;
			Destroy (tutorialButton);
		} 
		else {
			credits.Stop ();
			creditsTrigger = false;
			Destroy (creditsButton);
		}
	}

}
