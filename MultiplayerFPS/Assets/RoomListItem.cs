using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking.Match;

public class RoomListItem : MonoBehaviour {

	public delegate void JoinRoomDelegate(MatchDesc _match);
	private JoinRoomDelegate joinRoomCallback;

	[SerializeField]
	private Text roomNameText;

	private MatchDesc match;

	public void Setup (MatchDesc _match, JoinRoomDelegate _joinRoomCallback)
	{
		match = _match;
		joinRoomCallback = _joinRoomCallback;

		roomNameText.text = match.name + " (" + match.currentSize + "/" + match.maxSize + ")";
    }

	public void JoinRoom ()
	{
		joinRoomCallback.Invoke(match);
	}

}
