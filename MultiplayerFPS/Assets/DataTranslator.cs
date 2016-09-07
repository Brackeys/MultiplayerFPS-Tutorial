using UnityEngine;
using System;

public class DataTranslator : MonoBehaviour {

	private static string KILLS_SYMBOL = "[KILLS]";
	private static string DEATHS_SYMBOL = "[DEATHS]";

	public static string ValuesToData (int kills, int deaths)
	{
		return KILLS_SYMBOL + kills + "/" + DEATHS_SYMBOL + deaths;
	}

	public static int DataToKills (string data)
	{
		return int.Parse (DataToValue(data, KILLS_SYMBOL));
    }

	public static int DataToDeaths (string data)
	{
		return int.Parse(DataToValue(data, DEATHS_SYMBOL));
	}

	private static string DataToValue (string data, string symbol)
	{
		string[] pieces = data.Split('/');
		foreach (string piece in pieces)
		{
			if (piece.StartsWith(symbol))
			{
				return piece.Substring(symbol.Length);
			}
		}

		Debug.LogError(symbol + " not found in " + data);
		return "";
	}

}
