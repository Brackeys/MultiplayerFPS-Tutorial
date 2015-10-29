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

	// Cache references
	Manager manager;

	void Start ()
	{
		// Disable components that should only be
		// active on the player that we control
		if (!isLocalPlayer)
		{
			DisableComponents();
			gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
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

		string _ID = "Player " + GetComponent<NetworkIdentity>().netId.ToString();
		transform.name = _ID;

		manager = Manager.ins;
		manager.RegisterPlayer(_ID, this.gameObject);
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
