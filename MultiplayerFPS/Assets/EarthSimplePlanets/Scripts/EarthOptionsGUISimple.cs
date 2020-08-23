using UnityEngine;
using System.Collections;

public class EarthOptionsGUISimple : MonoBehaviour {

	private bool clouds = true;
	private float earthRotationSpeed = 2.0f;
	private float cloudRotationSpeed = 3.0f;
	private CloudRotation cloudRotationScript;
	private GameObject cloudsTransform;
	private float cloudType = 1.0f;
	private float currentSelectedCloud = 1.0f;
	private GeneralUI generalUIScript;
	public float labelWidth = 160;

	// Use this for initialization
	void Start () {
		cloudsTransform = GameObject.FindGameObjectWithTag("Earth Clouds");
		cloudRotationScript = cloudsTransform.GetComponent<CloudRotation>();
		generalUIScript = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<GeneralUI> ();
	}

	void Update()
	{
		transform.Rotate(new Vector3(0, Time.deltaTime * earthRotationSpeed, 0));

		if ((int)cloudType != (int)currentSelectedCloud) {
			var selectedCloudMaterial = generalUIScript.cloudMaterials[(int)cloudType];	
			var selectedCloudShadowMaterial = generalUIScript.cloudShadowMaterials[(int)cloudType];
			GameObject.Find("Clouds/Clouds").GetComponent<Renderer>().material = selectedCloudMaterial;
			GameObject.Find("Clouds/CloudsOuter").GetComponent<Renderer>().material = selectedCloudShadowMaterial;		

			currentSelectedCloud = (int)cloudType;
		} 

	}

	void OnGUI () 
	{
		GUI.Box (new Rect (Screen.width - 300,Screen.height - 25,250,120), "Left click & drag to rotate around Earth.");

		clouds = GUI.Toggle (new Rect (25, 30, 100, 30), clouds, "Clouds");

		GUI.Label(new Rect(25,60,labelWidth,30), "Earth rotation speed");
		earthRotationSpeed = GUI.HorizontalScrollbar (new Rect (25, 90, labelWidth, 30), earthRotationSpeed, 1.0f, 0.0f, 10.0f);

		if(clouds)
		{
			ToggleChildrenMeshRendered(clouds, cloudsTransform);
			GUI.Label(new Rect(25,120,labelWidth,30), "Cloud rotation speed");
			cloudRotationSpeed = GUI.HorizontalScrollbar (new Rect (25, 150, labelWidth, 30), cloudRotationSpeed, 1.0f, 0.0f, 15.0f);

			GUI.Label(new Rect(25,180,labelWidth,30), "Cloud type");
			cloudType = GUI.HorizontalScrollbar(new Rect(25,210,labelWidth,30), cloudType, 1.0f,0.0f,generalUIScript.cloudMaterials.Count);
		}
		else
		{
			ToggleChildrenMeshRendered(clouds, cloudsTransform);
		}

		cloudRotationScript.planetSpeedRotation = cloudRotationSpeed;
	

	}

	void ToggleChildrenMeshRendered(bool on, GameObject cloudsTransfom)
	{
		if(on)
			{
			foreach (var item in cloudsTransform.GetComponentsInChildren<MeshRenderer>()) {
				item.enabled = true;
			}
		}
		else
		{
			foreach (var item in cloudsTransform.GetComponentsInChildren<MeshRenderer>()) {
				item.enabled = false;
			}
		}
	}
}
