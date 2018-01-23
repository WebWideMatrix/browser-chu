using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Models;

public class BuildingController : MonoBehaviour {

	public Building model;

	public AddressController addressController;

	public void initialize(Building theModel, AddressController addrCtl) {
		this.model = theModel;
		this.addressController = addrCtl;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void handleClick(string context) {
		if (this.model.isComposite) {
			Debug.Log ("~~~~~~~~~~Got address controller");
			Debug.Log (addressController);
			addressController.GoToBldg (this.model.address);
		} else {
			openInBrowser (context);
		}
	}

	public void openInBrowser(string context) {
		string externalUrl = null;
		switch (context) {
			case "CONTENT":
			{
				Debug.Log ("Opening in browser: " + this.model.address);
				externalUrl = this.model.summary.external_url;
				break;
			}
			case "USER":
			{
				// TWITTER POST BLDG ONLY
				Debug.Log ("Opening in browser: " + this.model.summary.user.screen_name);
				externalUrl = "http://twitter.com/" + this.model.summary.user.screen_name;
				break;
			}
		}
		if (externalUrl != null)
			Application.OpenURL (externalUrl);
	}

	public void renderAuthorPicture() {
		Debug.Log ("renderAuthorPicture called for " + this.model.key);
		StartCoroutine (loadAuthorPicture ());
	}

	IEnumerator loadAuthorPicture() {
		Debug.Log ("loadAuthorPicture called for " + this.model.key);
		using (WWW www = new WWW (this.model.picture)) {
			yield return www;
			if (www.isDone)
				Debug.Log ("Finished loading: " + this.model.picture);
			if (www.texture) {
				this.gameObject.GetComponentInChildren<Renderer> ().material.mainTexture = www.texture;
			}
		}
	}
}
