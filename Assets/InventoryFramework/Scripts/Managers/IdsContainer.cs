using UnityEngine;
using System.Collections;

namespace SA.InventoryFramework
{
	[System.Serializable]
	public class IdsContainer 
	{
		[System.NonSerialized]
		public int instId;
		public Items.BodyPart bodyPart;
		public bool isLeft;
	}

	[System.Serializable]
	public class StartingIdContainer
	{
		public string itemId;
		public Items.BodyPart bodyPart;
		public bool isLeft;
	}
}
