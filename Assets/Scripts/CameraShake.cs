using UnityEngine;
using System.Collections;
public class CameraShake : MonoBehaviour
{
	private Vector3 originPosition;
	private Quaternion originRotation;
	public float shake_decay = 0.00001f;
	public float shake_intensity = 0.0f;

	void Start() {

		//Shake (0.02f);
	}

	public void SetDecay(float decay) {
		
		shake_decay = decay;
	}


	
	void Update (){

		if (shake_intensity < 0.02){
			transform.position = originPosition + Random.insideUnitSphere * shake_intensity;
			transform.rotation = new Quaternion(
				originRotation.x + Random.Range (-shake_intensity,shake_intensity) * .2f,
				originRotation.y + Random.Range (-shake_intensity,shake_intensity) * .2f,
				originRotation.z + Random.Range (-shake_intensity,shake_intensity) * .2f,
				originRotation.w + Random.Range (-shake_intensity,shake_intensity) * .2f);

		}
		shake_intensity += shake_decay;

	}
	
	public void Shake(){
		originPosition = transform.position;
		originRotation = transform.rotation;
		shake_intensity = 0.0f;


	}
}