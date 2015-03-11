using UnityEngine;
using System.Collections;

public class Cube : Shape {

	// Use this for initialization
	public override void Start () {
		sides = 4;
		base.Start();
	
	}
	
	// Update is called once per frame
	public override void Update () {

		base.Update ();
	
	}
	public override void OnDestroy () {
		
		base.OnDestroy();
		
	}
}
