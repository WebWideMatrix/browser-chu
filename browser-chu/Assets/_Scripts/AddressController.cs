using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Proyecto26;
using Models;
using Utils;


public class AddressController : MonoBehaviour {


	public GameObject bldg;

	string basePath = "http://localhost:4000/api";

	string currentAddress;
	string currentFlr;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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

//		string json = @"{""y"":82,""x"":118,""key"":""932931363233247232"",""flr"":""g-b(101,20)-l0-b(2,0)-l0"",
//					   ""contentType"":""twitter-social-post"",""address"":""g-b(101,20)-l0-b(2,0)-l0-b(118,82)"",
//					   ""summary"":{""user"":{""screen_name"":""machinelearnbot"",
//					   ""profile_text_color"":""000000"",""profile_background_color"":""000000"",
//					   ""name"":""Machine Learning""},
//					   ""text"":""RT @Ronald_vanLoon: How Uber Uses Big Data to Optimize Customer Experience | #BigData #CX #RT https://t.co/6GlLaVTXes https://t.co/XYakYFpS…""}}";
//		Building bldg = UnityEngine.JsonUtility.FromJson<Building>(json);
//		Debug.Log (bldg);
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
					GameObject bldgClone = (GameObject) Instantiate(bldg, baseline, Quaternion.identity);
					bldgClone.tag = "Building";
					BuildingController controller = bldgClone.GetComponentInChildren<BuildingController>();
					controller.initialize(b);
					Text[] labels = bldgClone.GetComponentsInChildren<Text>();
					foreach (Text label in labels) {
						if (label.name == "TwitText")
							label.text = b.summary.text;
						else if (label.name == "AuthorName")
							label.text = b.summary.user.name;
					}
					Debug.Log("About to call renderAuthorPicture on bldg " + count);
					controller.renderAuthorPicture();
				}

			});
	}

	void updateFloorSign() {
		Text flrSign = GameObject.FindGameObjectWithTag ("FloorSign").GetComponent<Text>();
		flrSign.text = currentFlr.ToUpper ();
	}


}
