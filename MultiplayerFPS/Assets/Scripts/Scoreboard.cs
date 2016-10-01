using UnityEngine;
using System.Collections;

public class Scoreboard : MonoBehaviour {

	[SerializeField]
	GameObject playerScoreboardItem;

	[SerializeField]
	Transform playerScoreboardList;

	void OnEnable ()
	{
		Player[] players = GameManager.GetAllPlayers();

		foreach (Player player in players)
		{
			GameObject itemGO = (GameObject)Instantiate(playerScoreboardItem, playerScoreboardList);
			PlayerScoreboardItem item = itemGO.GetComponent<PlayerScoreboardItem>();
			if (item != null)
			{
				item.Setup(player.username, player.kills, player.deaths);
			}
		}
	}

	void OnDisable ()
	{
		foreach (Transform child in playerScoreboardList)
		{
			Destroy(child.gameObject);
		}
	}

}
