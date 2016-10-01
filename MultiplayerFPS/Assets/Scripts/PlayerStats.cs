using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerStats : MonoBehaviour {

	public Text killCount;
	public Text deathCount;

	// Use this for initialization
	void Start () {
		if (UserAccountManager.IsLoggedIn)
			UserAccountManager.instance.GetData(OnReceivedData);
	}

	void OnReceivedData (string data)
	{
		if (killCount == null || deathCount == null)
			return;

		killCount.text = DataTranslator.DataToKills(data).ToString() + " KILLS";
		deathCount.text = DataTranslator.DataToDeaths(data).ToString() + " DEATHS";
	}
	
}
