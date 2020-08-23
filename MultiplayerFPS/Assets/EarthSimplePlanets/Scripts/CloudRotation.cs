using UnityEngine;
using System.Collections;

public class CloudRotation : MonoBehaviour {

	public float planetSpeedRotation = 3.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		transform.Rotate(Vector3.up * Time.deltaTime * planetSpeedRotation);


	}
}
