using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Models;

public class BuildingController : MonoBehaviour {

	public Building model;

	public void initialize(Building theModel) {
		this.model = theModel;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void openInBrowser() {
		Debug.Log ("Opening in browser: " + this.model.address);
		string externalUrl = this.model.summary.external_url;
		Application.OpenURL (externalUrl);
	}

	public void openUserInBrowser() {
		// TWITTER POST BLDG ONLY
		Debug.Log ("Opening in browser: " + this.model.summary.user.screen_name);
		string externalUrl = "http://twitter.com/" + this.model.summary.user.screen_name;
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
