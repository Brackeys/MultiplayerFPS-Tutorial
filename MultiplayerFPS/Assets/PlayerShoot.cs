using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour {

	[SerializeField]
	PlayerWeapon weapon;

	[SerializeField]
	Camera cam;

	[SerializeField]
	LayerMask mask;

	// Cached references
	private Manager manager;

	void Start ()
	{
		manager = Manager.ins;
	}

	void Update ()
	{
		if (Input.GetButtonDown ("Fire1"))
		{
			Shoot();
		}
	}

	[Client]
	void Shoot ()
	{
		RaycastHit _hit;
		if (Physics.Raycast ( cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask))
		{
			//Debug.Log(_hit.transform.name);
			if (_hit.transform.tag == "Player")
			{
				CmdPlayerShot(_hit.transform.name);
			}
		}
	}

	[Command]
	void CmdPlayerShot (string _ID)
	{
		GameObject _player = manager.GetPlayer(_ID);
		if (_player == null)
		{
			Debug.LogError(_ID + " not found?");
		} else
		{
			Debug.Log(_ID + " damaged by " + weapon.damage);
		}
	}

}
