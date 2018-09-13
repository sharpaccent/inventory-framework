using System.Collections;
using System.Collections.Generic;
using SA.Items;
using UnityEngine;

namespace SA.InventoryFramework
{
	[CreateAssetMenu(menuName = "Inventory Framework/Slot Types/Equipment Slot")]
	public class EquipmentSlot : SlotType
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
			if (item.bodyPart == slot.bodyPart)
			{
				Item previousItem = slot.itemInstance;
				slot.LoadItem(item);
				item.isEquiped = true;
				invManager.UnEquipFromStoredSlot();
				invManager.StoreIdOnResources(item, slot);
				invManager.LoadItemsOnCharacter();
				invManager.ClearCurrentItem();

				if (previousItem != null)
				{
					previousItem.isEquiped = false;
					invManager.SetItemOnStoredSlot(previousItem);
				}
			}
		}
	}
}
