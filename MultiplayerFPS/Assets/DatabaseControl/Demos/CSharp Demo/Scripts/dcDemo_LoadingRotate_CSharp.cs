using UnityEngine;
using System.Collections;

public class dcDemo_LoadingRotate_CSharp : MonoBehaviour {

	public float rotSpeed = 0f;

	void Update () {
		transform.Rotate(Vector3.forward * Time.deltaTime * rotSpeed);
	}
}
