using System;
using System.Collections.Generic;

namespace Models
{
	[Serializable]
	public class Building
	{
		public int x;

		public int y;

		public string web_url;

		public string summary;

		public bool is_composite;

		public string flr;

		public string entity_type;

		public string name;

		public string state;

		public string category;

		public string[] tags;

		public string address;

		public string picture_url;

		//public Map data;

		public override string ToString(){
			return UnityEngine.JsonUtility.ToJson (this, true);
		}
	}

}

