using UnityEngine;

public class PlayerUI : MonoBehaviour {

	[SerializeField]
	RectTransform thrusterFuelFill;

	[SerializeField]
	GameObject pauseMenu;

	private PlayerController controller;

	public void SetController (PlayerController _controller)
	{
		controller = _controller;
	}

	void Start ()
	{
		PauseMenu.IsOn = false;
	}

	void Update ()
	{
		SetFuelAmount (controller.GetThrusterFuelAmount());

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			TogglePauseMenu();
		}
	}

	void TogglePauseMenu ()
	{
		pauseMenu.SetActive(!pauseMenu.activeSelf);
		PauseMenu.IsOn = pauseMenu.activeSelf;
    }

	void SetFuelAmount (float _amount)
	{
		thrusterFuelFill.localScale = new Vector3(1f, _amount, 1f);
	}

}
