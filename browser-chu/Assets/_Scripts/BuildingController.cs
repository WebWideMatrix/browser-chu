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
		if (this.model.is_composite) {
			addressController.GoToBldg (this.model.address);
		} else {
			openInBrowser (context);
		}
	}

	public void openInBrowser(string context) {
		string externalUrl = this.getExternalURLByContext (context);
		if (externalUrl != null)
			Application.OpenURL (externalUrl);
	}

	public void renderMainPicture() {
		if (this.getMainPicture () != "null") {
			StartCoroutine (loadMainPicture ());
		}
	}

	IEnumerator loadMainPicture() {
		string pictureUrl = this.getMainPicture ();
		// handle null
		using (WWW www = new WWW (pictureUrl)) {
			yield return www;
			if (www.isDone)
				Debug.Log ("Finished loading: " + pictureUrl);
			if (www.texture) {
				this.gameObject.GetComponentInChildren<Renderer> ().material.mainTexture = www.texture;
			}
		}
	}


	public string getMainPicture() {
		return this.model.picture_url;
	}

	public string getExternalURLByContext(string context) {
		return this.model.web_url;
	}

	public string getExternalURLByContext_twit(string context) {
		string url = null;
		switch (context) {
		case "CONTENT":
			{
				url = this.model.web_url;
				break;
			}
		case "USER":
			{
				url = this.model.web_url;
				break;
			}
		}
		return url;
	}

	public string getExternalURLByContext_article(string context) {
		string url = null;
		switch (context) {
		case "CONTENT":
			{
				url = this.model.web_url;
				break;
			}
		case "USER":
			{
				url = this.model.web_url;
				break;
			}
		}
		return url;
	}
}
