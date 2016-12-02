using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class ButtonScript : MonoBehaviour {

	public GameObject sliderView;
	public GameObject FPS;

	//public GameObject cameraStuff;

	void Start () {
	
	 
	}
	void Update () {
		if (sliderView.activeSelf) {
			FirstPersonController walking = FPS.GetComponent<FirstPersonController> ();
			walking.enabled = false;
			Cursor.lockState = CursorLockMode.Confined;
			Cursor.visible = true;
		} else {
			FirstPersonController walking = FPS.GetComponent<FirstPersonController> ();
			walking.enabled = true;
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	
	}

	public void onClick(){
		if (sliderView.activeSelf) {
			sliderView.SetActive (false);
			//cameraStuff.SetActive (true);
		} else {
			sliderView.SetActive (true);
			//cameraStuff.SetActive (false);	
		}
	}
}
