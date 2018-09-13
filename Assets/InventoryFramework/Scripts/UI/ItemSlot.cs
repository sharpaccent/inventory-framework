using System.Collections;
using System.Collections.Generic;
using SA.Items;
using UnityEngine;

namespace SA.InventoryFramework
{
	[CreateAssetMenu(menuName ="Inventory Framework/Slot Types/Item Slot")]
	public class ItemSlot : SlotType
	{
		public override void OnClick(UI_Slot slot, UI_InventoryManager invManager)
		{
			if (slot.itemInstance != null)
			{
				invManager.SetCurrentItem(slot.itemInstance, slot);
				slot.UnloadItem();
			}
		}

		public override void OnDropItem(UI_Slot slot, Item item, UI_InventoryManager invManager)
		{
			Item previousItem = slot.itemInstance;	
			slot.LoadItem(item);
			item.isEquiped = false;
			invManager.ClearCurrentItem();
			invManager.UnEquipFromStoredSlot();

			if (previousItem != null)
			{
				invManager.SetItemOnStoredSlot(previousItem);
			}
		}
	}
}
