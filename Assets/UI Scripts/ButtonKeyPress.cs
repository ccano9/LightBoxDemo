using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonKeyPress : MonoBehaviour {

	public GameObject sliderView;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("m")){
			if(sliderView.activeSelf){
				sliderView.SetActive(false);
			}
			else{
				sliderView.SetActive(true);
			}
		}
	}
}
