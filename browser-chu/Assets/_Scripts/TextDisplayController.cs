using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDisplayController : MonoBehaviour {

	void OnMouseDown() {
		Debug.Log ("Mouse down on text display!");
		gameObject.GetComponentInParent<BuildingController> ().openInBrowser ();
	}
}
