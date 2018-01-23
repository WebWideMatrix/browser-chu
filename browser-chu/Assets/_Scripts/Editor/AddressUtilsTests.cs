using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using Utils;

public class AddressUtilsTests {

	// extractFlr

	[Test]
	public void extractFlr_Simple() {
		string address = "g-b(123,456)-l1";
		string flr = AddressUtils.extractFlr(address);
		Assert.That(flr == "l1");
	}


	[Test]
	public void extractFlr_BuildingAddress() {
		string address = "g-b(123,456)-l1-b(7,8)";
		string flr = AddressUtils.extractFlr(address);
		Assert.That(flr == "l1");

		address = "g-b(123,456)";
		flr = AddressUtils.extractFlr(address);
		Assert.That(flr == "g");
	}

	[Test]
	public void extractFlr_Ground() {
		string address = "g";
		string flr = AddressUtils.extractFlr(address);
		Assert.That(flr == "g");
	}



	// extractContainerFlr


	[Test]
	public void getContainerFlr_Simple() {
		string address = "g-b(123,456)-l1-b(7,8)-l2";
		string flr = AddressUtils.getContainerFlr(address);
		Assert.That(flr == "g-b(123,456)-l1");

		address = "g-b(123,456)-l1";
		flr = AddressUtils.getContainerFlr(address);
		Assert.That(flr == "g");
	}



	[Test]
	public void getContainerFlr_BuildingAddress() {
		string address = "g-b(123,456)-l1-b(7,8)";
		string flr = AddressUtils.getContainerFlr(address);
		Assert.That(flr == "g-b(123,456)-l1");

		address = "g-b(123,456)";
		flr = AddressUtils.getContainerFlr(address);
		Assert.That(flr == "g");
	}


	[Test]
	public void getContainerFlr_Ground() {
		string address = "g";
		string flr = AddressUtils.getContainerFlr(address);
		Assert.That(flr == "g");
	}


	// getFlr

	[Test]
	public void getFlr_Simple() {
		string address = "g-b(123,456)-l1-b(7,8)-l2";
		string flr = AddressUtils.getFlr(address);
		Assert.That(flr == address);

		address = "g-b(123,456)-l1-b(7,8)";
		flr = AddressUtils.getFlr(address);
		Assert.That(flr == "g-b(123,456)-l1");
	}

	[Test]
	public void getFlr_Ground() {
		string address = "g";
		string flr = AddressUtils.getFlr(address);
		Assert.That(flr == address);
	}


	// getFlrLevel

	[Test]
	public void getFlrLevel_Simple() {
		string address = "g-b(123,456)-l1-b(7,8)-l2";
		int level = AddressUtils.getFlrLevel (address);
		Assert.That (level == 2);
	}

	[Test]
	public void getFlrLevel_Building() {
		string address = "g-b(123,456)-l1-b(7,8)";
		try {
			AddressUtils.getFlrLevel(address);
			Assert.Fail();
		} catch (System.ArgumentException) {
			// as expected
		}
	}

	[Test]
	public void getFlrLevel_Ground() {
		string address = "g";
		try {
			AddressUtils.getFlrLevel(address);
			Assert.Fail();
		} catch (System.ArgumentException) {
			// as expected
		}
	}


	// getBldg

	[Test]
	public void getBldg_Simple() {
		string address = "g-b(123,456)-l1-b(7,8)";
		string flr = AddressUtils.getBldg(address);
		Assert.That(flr == address);

		address = "g-b(123,456)-l1-b(7,8)-l2";
		flr = AddressUtils.getBldg(address);
		Assert.That(flr == "g-b(123,456)-l1-b(7,8)");
	}

	[Test]
	public void getBldg_Ground() {
		string address = "g";
		string flr = AddressUtils.getBldg(address);
		Assert.That(flr == address);
	}


	// getContainingBldgAddress

	[Test]
	public void getContainingBldgAddress_Simple() {
		string address = "g-b(123,456)-l1-b(7,8)";
		string flr = AddressUtils.getContainingBldgAddress(address);
		Assert.That(flr == "g-b(123,456)");

		address = "g-b(123,456)-l1-b(7,8)-l2";
		flr = AddressUtils.getContainingBldgAddress(address);
		Assert.That(flr == "g-b(123,456)-l1-b(7,8)");
	}

	[Test]
	public void getContainingBldgAddress_Ground() {
		string address = "g";
		string flr = AddressUtils.getContainingBldgAddress(address);
		Assert.That(flr == address);
	}


	// replaceFlrLevel

	[Test]
	public void replaceFlrLevel_Simple() {
		string address = "g-b(123,456)-l1-b(7,8)";
		string flr = AddressUtils.replaceFlrLevel(address, 2);
		Debug.Log (flr);
		Assert.That(flr == "g-b(123,456)-l2-b(7,8)");

		address = "g-b(123,456)-l0-b(7,8)-l1";
		flr = AddressUtils.replaceFlrLevel(address, 2);
		Debug.Log (flr);
		Assert.That(flr == "g-b(123,456)-l0-b(7,8)-l2");
	}

	[Test]
	public void replaceFlrLevel_Ground() {
		string address = "g";
		string flr = AddressUtils.replaceFlrLevel(address, 2);
		Assert.That(flr == address);
	}



	// isBldg
//
//	public void isBldg_Simple() {
//		string address = "g-b(123,456)-l1-b(7,8)";
//		bool itIsABldg = AddressUtils.isBldg (address);
//		Assert.That (itIsABldg);
//
//		address = "g-b(123,456)-l1-b(7,8)-l0";
//		bool itIsNotABldg = AddressUtils.isBldg (address);
//		Assert.That (!itIsNotABldg);
//	}


	// generateInsideAddress

//	public void generateInsideAddress_Simple() {
//		string address = "g-b(123,456)-l1-b(7,8)";
//		string insideAddress = AddressUtils.generateInsideAddress (address);
//		Assert.That (insideAddress == "g-b(123,456)-l1-b(7,8)-l0");
//
//		address = "g-b(123,456)-l1-b(7,8)-l0";
//		insideAddress = AddressUtils.generateInsideAddress (address);
//		Assert.That (insideAddress == address);
//	}
}
