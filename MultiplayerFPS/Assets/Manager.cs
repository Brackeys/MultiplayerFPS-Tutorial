using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Manager : NetworkManager {

	public static Manager ins;

	[SerializeField]
	private Dictionary<string, GameObject> players;

	void Awake ()
	{
		if (ins == null)
			ins = this;
		else
			this.enabled = false;

		players = new Dictionary<string, GameObject>();
	}

	public void RegisterPlayer (string _ID, GameObject _player)
	{
		players.Add(_ID, _player);
	}

	public GameObject GetPlayer (string _ID)
	{
		return players[_ID];
	}

	void OnGUI ()
	{
		GUILayout.BeginArea(new Rect(0f, 300f, 100f, 200f));
		foreach (KeyValuePair<string, GameObject> _player in players)
		{
			GUILayout.Box(_player.Key + "   " + _player.Value.name);
		}
		GUILayout.EndArea();
	}

}
