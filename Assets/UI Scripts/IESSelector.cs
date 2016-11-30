using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IESSelector : MonoBehaviour {

	public GameObject dropdown;
	public GameObject light1;
	public GameObject light2;
	public GameObject light3;
	public GameObject light4;
	public GameObject light5;
	public GameObject light6;
	public GameObject light7;
	public GameObject light8;
	public GameObject light9;
	public GameObject light10;
	public GameObject light11;
	public GameObject light12;

	ArrayList lightList = new ArrayList();
	// Use this for initialization
	void Start () {
		lightList.Add (light1);
		lightList.Add (light2);
		lightList.Add (light3);
		lightList.Add (light4);
		lightList.Add (light5);
		lightList.Add (light6);
		lightList.Add (light7);
		lightList.Add (light8);
		lightList.Add (light9);
		lightList.Add (light10);
		lightList.Add (light12);

	}
	
	// Update is called once per frame
	void Update () {
		Dropdown db = dropdown.GetComponent<Dropdown> ();
		if (db.value != 0) {
			string baseName = "OA";
			baseName = baseName + db.value.ToString ();
			Texture2D inputTexture = (Texture2D)Resources.Load (baseName, typeof(Texture2D)) as Texture2D;
			foreach (GameObject light in lightList) {
				light.GetComponent<Light> ().cookie = inputTexture;
			}

		}
	}

	public ArrayList getLightList(){
		return lightList;
	}
		
}
