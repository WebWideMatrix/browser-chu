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
		string pictureUrl = null;
		switch (this.model.contentType) {
		case "twitter-social-post":
			pictureUrl = this.model.picture;
			break;
		case "article-text": 
			pictureUrl = this.model.summary.metadata.image_url;
			break;
		case "user": 
			pictureUrl = this.model.summary.picture;
			break;		
		default:
			pictureUrl = this.model.picture;
			break;
		}
		return pictureUrl;
	}

	public string getExternalURLByContext(string context) {
		switch (this.model.contentType) {
		case "twitter-social-post":
			return getExternalURLByContext_twit (context);
		case "article-text":
			return getExternalURLByContext_article (context);
		default:
			return null;
		}
	}

	public string getExternalURLByContext_twit(string context) {
		string url = null;
		switch (context) {
		case "CONTENT":
			{
				url = this.model.summary.external_url;
				break;
			}
		case "USER":
			{
				url = "http://twitter.com/" + this.model.summary.user.screen_name;
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
				url = this.model.payload.url;
				break;
			}
		case "USER":
			{
				url = this.model.payload.url;
				break;
			}
		}
		return url;
	}
}
