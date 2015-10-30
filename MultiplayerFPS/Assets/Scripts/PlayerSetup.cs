//-------------------------------------
// Responsible for setting up the player.
// This includes adding/removing him correctly on the network.
//-------------------------------------

using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {

	[SerializeField]
	Behaviour[] componentsToDisable;

	[SerializeField]
	string remoteLayerName = "RemotePlayer";

	Camera sceneCamera;

	void Start ()
	{
		// Disable components that should only be
		// active on the player that we control
		if (!isLocalPlayer)
		{
			DisableComponents();
			AssignRemoteLayer();
		}
		else
		{
			// We are the local player: Disable the scene camera
			sceneCamera = Camera.main;
			if (sceneCamera != null)
			{
				sceneCamera.gameObject.SetActive(false);
            }
		}

		RegisterPlayer();

	}

	void RegisterPlayer ()
	{
		string _ID = "Player " + GetComponent<NetworkIdentity>().netId;
		transform.name = _ID;
	}

	void AssignRemoteLayer ()
	{
		gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
	}

	void DisableComponents ()
	{
		for (int i = 0; i < componentsToDisable.Length; i++)
		{
			componentsToDisable[i].enabled = false;
		}
	}

	// When we are destroyed
	void OnDisable ()
	{
		// Re-enable the scene camera
		if (sceneCamera != null)
		{
			sceneCamera.gameObject.SetActive(true);
		}
	}

}
