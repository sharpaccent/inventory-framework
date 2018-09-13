using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SA.InventoryFramework
{
	public class UI_Slot : MonoBehaviour
	{
		public SlotType slotType;
	
		public Items.BodyPart bodyPart;
		[SerializeField]
		Image icon;

		public bool isWeaponSlot;
		public bool isLeft;

		[HideInInspector]
		public Items.Item itemInstance;



		private void Awake()
		{
			if(icon != null)
				icon.enabled = false;
		}

		public void LoadItem(Items.Item targetItem)
		{
			itemInstance = targetItem;
			if (itemInstance != null)
			{
				itemInstance.currentSlot = this;
				icon.sprite = itemInstance.ui_info.icon;
				icon.enabled = true;
			}
			else
			{
				icon.sprite = null;
				icon.enabled = false;
			}
		}

		public void UnloadItem()
		{
			itemInstance = null;
			icon.sprite = null;
			icon.enabled = false;
		}

		public void OnClick(UI_InventoryManager invManager)
		{
			if(slotType != null)
				slotType.OnClick(this, invManager);
		}

		public void OnDropItem(Items.Item item, UI_InventoryManager invManager)
		{
			if (slotType != null)
				slotType.OnDropItem(this, item, invManager);
		}
	}
}
