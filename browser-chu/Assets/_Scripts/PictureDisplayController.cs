using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureDisplayController : MonoBehaviour {

	void OnMouseDown() {
		Debug.Log ("Mouse down on pic display!");
		gameObject.GetComponentInParent<BuildingController> ().handleClick ("USER");
	}
}
