using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Proyecto26;
using Models;
using Utils;
using System;
using UnityEngine.SceneManagement;


public class AddressController : MonoBehaviour {


	public GameObject twitBldg;
	public GameObject articleBldg;
	public GameObject conceptBldg;
	public GameObject dailyFeedBldg;
	public GameObject userBldg;

	// TODO get from configuration
	string basePath = "http://localhost:4000/api";

	string currentAddress;
	string currentFlr;


	// Use this for initialization
	void Start () {
		if (currentAddress == null) {
			if (PlayerPrefs.GetString ("currentAddress") != null) {
				currentAddress = PlayerPrefs.GetString ("currentAddress");
			} else {
				currentAddress = "g";
			}
			AddressChanged ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GoToBldg(string address) {
		currentAddress = address;
		if (AddressUtils.isBldg(address)) {
			currentAddress = AddressUtils.generateInsideAddress (address);
		}
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

		AddressChanged ();
	}


	public void GoUp() {
		InputField input = GameObject.FindObjectOfType<InputField> ();
		currentAddress = input.text;
		int level = AddressUtils.getFlrLevel(currentAddress);
		currentAddress = AddressUtils.replaceFlrLevel (currentAddress, level + 1);
		AddressChanged ();
	}


	public void GoDown() {
		InputField input = GameObject.FindObjectOfType<InputField> ();
		currentAddress = input.text;
		int level = AddressUtils.getFlrLevel(currentAddress);
		if (level > 0) {
			currentAddress = AddressUtils.replaceFlrLevel (currentAddress, level - 1);
		}
		AddressChanged ();
	}


	public void AddressChanged() {
		Debug.Log ("Address changed to: " + currentAddress);
		InputField input = GameObject.FindObjectOfType<InputField> ();
		if (input.text != currentAddress) {
			input.text = currentAddress;
		}

		PlayerPrefs.SetString ("currentAddress", currentAddress);

		// TODO validate address

		currentFlr = AddressUtils.extractFlr(currentAddress);

		// TODO check whether it changed

		// check whether we need to switch scene
		if (currentAddress.ToLower () == "g" && SceneManager.GetActiveScene().name == "Floor") {
			SceneManager.LoadScene ("Ground");
			return;
		}
		if (currentAddress.ToLower () != "g" && SceneManager.GetActiveScene().name == "Ground") {
			SceneManager.LoadScene ("Floor");
			return;
		}

		// load the new address
		switchAddress (currentAddress);
	}

	void switchAddress(string address) {
		if (address.ToLower() != "g") {
			updateFloorSign ();
		}

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
						if (label.name == "DayInWeek")
							label.text = extractDatePart(b.key, "DayInWeek");	
						else if (label.name == "Month")
							label.text = extractDatePart(b.key, "Month");	
						else if (label.name == "Date")
							label.text = extractDatePart(b.key, "Date");	
						else if (label.name == "TwitText")
							label.text = b.summary.text;
						else if (label.name == "AuthorName")
							label.text = b.summary.user.name;
						else if (label.name == "ArticleTitle")
							label.text = b.summary.metadata.title;					
						else if (label.name == "UserName")
							label.text = b.summary.name;							
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
		case "daily-feed":
			return dailyFeedBldg;
		case "user":
			return userBldg;
		default:
			return twitBldg;
		}
	}

	void updateFloorSign() {
		Text flrSign = GameObject.FindGameObjectWithTag ("FloorSign").GetComponent<Text>();
		flrSign.text = currentFlr.ToUpper ();
	}

	string extractDatePart(string date, string part) {
		// for sample input: "2017-Nov-26"
		// if part = "DayInWeek", return: "Sunday"
		// if part = "Month", return: "Nov"
		// if part = "Date", return: "26"
		// if part = "Year", return: "2017"
		char[] delim = new char[] {'-'};
		string[] parts = date.Split (delim);
		if (parts.Length != 3) {
			Debug.LogError ("Wrong date format: " + date);
			return "Error";
		}
		switch (part) {
		case "Year":
			return parts [0];
		case "Month":
			return parts [1];
		case "Date":
			return parts [2];
		case "DayInWeek":
			DateTime d = Convert.ToDateTime (date);
			return d.ToString("dddd");
		default:
			Debug.LogError ("Unexpected argument: " + part);
			return "Error";
		}
	}

}
