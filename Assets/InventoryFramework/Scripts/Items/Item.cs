using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA.Items
{
	public abstract class Item : ScriptableObject
	{
		public int instId;
		public Item_UI_Info ui_info;
		public BodyPart bodyPart;

		[System.NonSerialized]
		public InventoryFramework.UI_Slot currentSlot;
		[System.NonSerialized]
		public bool isEquiped;
		[System.NonSerialized]
		public GameObject pickableInstance;
	}
}
