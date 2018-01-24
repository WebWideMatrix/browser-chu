using System;
using System.Collections.Generic;

namespace Models
{
	[Serializable]
	public class Building
	{
		public int x;

		public int y;

		public string key;

		public Summary summary;

		public bool isComposite;

		public string flr;

		public string contentType;

		public string address;

		public string picture;

		public Payload payload;

		public override string ToString(){
			return UnityEngine.JsonUtility.ToJson (this, true);
		}
	}

	[Serializable]
	public struct UserSummary
	{
		public string screen_name;
		public string profile_text_color;
		public string profile_background_color;
		public string name;
	}


	[Serializable]
	public struct ArticleMetadata
	{
		public string title;
		public string image_url;
		public string site;
	}


	[Serializable]
	public struct Payload
	{
		// article-text
		public string url;
		public string display_url;
		// twitter-social-post
		public int reshare_count;
		public int favorite_count;
	}


	[Serializable]
	public struct Summary
	{
		public UserSummary user;
		public ArticleMetadata metadata;
		public string text;
		public string external_url;
		public DateTime created_at;
	}

}

