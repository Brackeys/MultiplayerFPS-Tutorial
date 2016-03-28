using UnityEngine;

public class PlayerUI : MonoBehaviour {

	[SerializeField]
	RectTransform thrusterFuelFill;

	private PlayerController controller;

	public void SetController (PlayerController _controller)
	{
		controller = _controller;
	}

	void Update ()
	{
		SetFuelAmount (controller.GetThrusterFuelAmount());
	}

	void SetFuelAmount (float _amount)
	{
		thrusterFuelFill.localScale = new Vector3(1f, _amount, 1f);
	}

}
