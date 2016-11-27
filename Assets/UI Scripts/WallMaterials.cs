using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WallMaterials : MonoBehaviour {

	public Material mat;
	public GameObject slider1;
	public GameObject slider2;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		mat.SetFloat ("_Metallic", slider1.GetComponent<Slider> ().value);
		mat.SetFloat ("_Glossiness", slider2.GetComponent<Slider> ().value);
	}
}
