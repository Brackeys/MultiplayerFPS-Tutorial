// ------------------------------------------------------------------
// Set up the player on the network
// ------------------------------------------------------------------

using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {

	[SerializeField]
	GameObject[] gameobjectsToDisable;
	[SerializeField]
	Behaviour[] componentsToDisable;

	private Camera sceneCam;

	void Start ()
	{
		if (!isLocalPlayer)
		{
			DisableLocalObjects();
		} else
		{
			Debug.Log("TODO: Disable scene camera outside Player Setup.");
			sceneCam = Camera.main;
			if (sceneCam != null)
				sceneCam.gameObject.SetActive(false);
		}
	}

	void OnDisable ()
	{
		if (isLocalPlayer)
		{
			if (sceneCam != null)
				sceneCam.gameObject.SetActive(true);
		}
	}

	// Disable components and gameobjects that should only
	// be active on the local player.
	[Client]
	void DisableLocalObjects ()
	{
		for (int i = 0; i < gameobjectsToDisable.Length; i++)
		{
			gameobjectsToDisable[i].SetActive( false );
		}
		for (int i = 0; i < componentsToDisable.Length; i++)
		{
			componentsToDisable[i].enabled = false;
		}
	}

}
