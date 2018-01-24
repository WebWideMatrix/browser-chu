using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Proyecto26;
using Models;
using Utils;


public class AddressController : MonoBehaviour {


	public GameObject twitBldg;
	public GameObject articleBldg;
	public GameObject conceptBldg;
	public GameObject dailyFeedBldg;
	public GameObject userBldg;

	string basePath = "http://localhost:4000/api";

	string currentAddress;
	string currentFlr;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GoToBldg(string address) {
		currentAddress = address;
		if (AddressUtils.isBldg(address)) {
			currentAddress = AddressUtils.generateInsideAddress (address);
		}
		InputField input = GameObject.FindObjectOfType<InputField> ();
		input.text = currentAddress;
		AddressChanged ();
	}

	public void GoIn() {
		InputField input = GameObject.FindObjectOfType<InputField> ();
		currentAddress = input.text;
		AddressChanged ();
	}


	public void GoOut() {
		InputField input = GameObject.FindObjectOfType<InputField> ();

		currentAddress = AddressUtils.getContainerFlr(input.text);
		input.text = currentAddress;
		AddressChanged ();
	}


	public void GoUp() {
		InputField input = GameObject.FindObjectOfType<InputField> ();
		currentAddress = input.text;
		int level = AddressUtils.getFlrLevel(currentAddress);
		currentAddress = AddressUtils.replaceFlrLevel (currentAddress, level + 1);
		input.text = currentAddress;
		AddressChanged ();
	}


	public void GoDown() {
		InputField input = GameObject.FindObjectOfType<InputField> ();
		currentAddress = input.text;
		int level = AddressUtils.getFlrLevel(currentAddress);
		if (level > 0) {
			currentAddress = AddressUtils.replaceFlrLevel (currentAddress, level - 1);
		}
		input.text = currentAddress;
		AddressChanged ();
	}


	public void AddressChanged() {
		Debug.Log ("Address changed to: " + currentAddress);
		// TODO validate address

		currentFlr = AddressUtils.extractFlr(currentAddress);

		// TODO check whether it changed

		switchAddress (currentAddress);
	}

	void switchAddress(string address) {
		updateFloorSign ();

		GameObject[] currentFlrBuildings = GameObject.FindGameObjectsWithTag("Building");
		foreach (GameObject bldg in currentFlrBuildings) {
			GameObject.Destroy (bldg);
		}

		// We can add default request headers for all requests
		RestClient.DefaultRequestHeaders["Authorization"] = "Bearer ...";

		RestClient.GetArray<Building>(new RequestHelper { url = basePath + "/buildings/in/" + address }).Then(res =>
			{
				int count = 0;
				foreach (Building b in res) {
					count += 1;
					Debug.Log("processing bldg " + count);


					Vector3 baseline = new Vector3(-198, 6.5F, -198);	// WHY? if you set the correct Y, some images fail to display
					baseline.x += b.x * 2;
					baseline.z += b.y * 2;
					GameObject prefab = getPrefabByEntityClass(b.contentType);
					GameObject bldgClone = (GameObject) Instantiate(prefab, baseline, Quaternion.identity);
					bldgClone.tag = "Building";
					BuildingController controller = bldgClone.GetComponentInChildren<BuildingController>();
					controller.initialize(b, this);
					Text[] labels = bldgClone.GetComponentsInChildren<Text>();
					foreach (Text label in labels) {
						if (label.name == "TwitText")
							label.text = b.summary.text;
						else if (label.name == "AuthorName")
							label.text = b.summary.user.name;
						else if (label.name == "ArticleTitle")
							label.text = b.summary.metadata.title;					
						else if (label.name == "SiteName") {
							if (b.summary.metadata.site != null)
								label.text = b.summary.metadata.site;
						}
					}
					Debug.Log("About to call renderAuthorPicture on bldg " + count);
					controller.renderMainPicture();
				}

			});
	}

	GameObject getPrefabByEntityClass(string contentType) {
		switch (contentType) {
		case "twitter-social-post":
			return twitBldg;
		case "article-text":
			return articleBldg;
		default:
			return twitBldg;
		}
	}

	void updateFloorSign() {
		Text flrSign = GameObject.FindGameObjectWithTag ("FloorSign").GetComponent<Text>();
		flrSign.text = currentFlr.ToUpper ();
	}


}
