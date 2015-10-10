//-------------------------------------
// Responsible for setting up the player.
// This includes adding/removing him correctly on the network.
//-------------------------------------

using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {

	[SerializeField]
	Behaviour[] componentsToDisable;

	Camera sceneCamera;

	void Start ()
	{
		// Disable components that should only be
		// active on the player that we control
		if (!isLocalPlayer)
		{
			for (int i = 0; i < componentsToDisable.Length; i++)
			{
				componentsToDisable[i].enabled = false;
			}
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
