using UnityEngine;

public class Player : MonoBehaviour {

	[SerializeField]
	private float health = 100f;

	public void DamagePlayer (float _amount)
	{
		health -= _amount;
		Debug.Log(transform.name + " took " + _amount + " damage.");
	}

}
