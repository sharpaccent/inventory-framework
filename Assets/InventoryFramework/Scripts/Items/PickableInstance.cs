using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA.Items
{
	public class PickableInstance : MonoBehaviour
	{
		public Item itemInstance;

		private void OnMouseDown()
		{
			InventoryFramework.UI_InventoryManager.singleton.PickupItem(itemInstance);
		}

	}
}
