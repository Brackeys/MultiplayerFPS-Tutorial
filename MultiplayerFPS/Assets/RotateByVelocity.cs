using UnityEngine;

public class RotateByVelocity : MonoBehaviour {
	
	Vector3 point;
	
	[SerializeField]
	Transform target;
	
	[SerializeField]
	float amount = 0.7f;
	
	void Update () {
		float _input = Input.GetAxis("Vertical");

		Vector3 _point;
        if (Mathf.Abs (_input) > 0.01f)
		{
			_point = transform.forward * _input * amount;
			_point.y = Mathf.Lerp(-1f, 0f, Mathf.Abs(_input * amount));	
		} else
		{
			_point = -transform.up + transform.forward * 0.01f;
		}

		point = _point + target.position;

		target.rotation = Quaternion.LookRotation(-_point);
	}
	
	void OnDrawGizmosSelected () {
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere (point, 0.1f);
		Gizmos.DrawLine (transform.position, point);
	}

}
