using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GeneralUI : MonoBehaviour {

	private float activatedPlanet = 0;
	private float currentActivatedPlanet = 0;
	private IList<GameObject> earths;
	public List<Material> cloudMaterials;
	public List<Material> cloudShadowMaterials;

	public string earthTypeLabel = "Choose Earth Type";

	// Use this for initialization
	void Start () {
		activatedPlanet = 0;

		earths = new List<GameObject> ();
		foreach (Transform earth in GameObject.FindGameObjectWithTag ("Player").transform) {
			earths.Add(earth.gameObject);
				}

		earths [(int)currentActivatedPlanet].SetActive (true);
	}

	void Update()
	{

		if ((int)currentActivatedPlanet != (int)activatedPlanet) {
			earths[(int)currentActivatedPlanet].SetActive(false);			

				currentActivatedPlanet = (int)activatedPlanet;
				} 

	}


	void OnGUI()
	{

		GUI.Label(new Rect(25,300,160,30), earthTypeLabel.ToUpper());
		activatedPlanet = GUI.HorizontalScrollbar (new Rect (25, 330, 160, 30), activatedPlanet, 1, 0, earths.Count);
		if (!earths [(int)activatedPlanet].activeInHierarchy) {
						earths [(int)activatedPlanet].SetActive (true);
				}


	}
	

}
