using UnityEngine;

public class RotateByVelocity : MonoBehaviour {
	
	Vector3 point;
	
	[SerializeField]
	Transform target;
	
	[SerializeField]
	float amount = 0.7f;
	
	void Update () {
		Vector3 _point = transform.forward * Input.GetAxis ("Vertical") * amount;
		_point.y = Mathf.Lerp (-1f, 0f, Mathf.Abs (Input.GetAxis ("Vertical") * amount));
		
		/*
		if (_point == Vector3.zero)
			_point = Vector3.up;
		*/
		
		//Debug.Log (_point);
		point = _point + target.position;
		
		target.rotation = Quaternion.LookRotation (-_point);
	}
	
	void OnDrawGizmosSelected () {
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere (point, 0.1f);
		Gizmos.DrawLine (transform.position, point);
	}

}
