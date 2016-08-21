#pragma strict

var rotSpeed = 0.0;

function Update () {
	transform.Rotate(Vector3.forward * Time.deltaTime * rotSpeed);
}