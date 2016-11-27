using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour {

	public GameObject sliderView;

	void Start () {
	}
	void Update () {
	
	}

	public void onClick(){
		if (sliderView.activeSelf) {
			sliderView.SetActive (false);
		} else {
			sliderView.SetActive (true);
		}
	}
}
