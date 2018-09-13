using UnityEngine;
using System.Collections;
using SA.Items;

namespace SA.InventoryFramework
{
	[CreateAssetMenu(menuName = "Inventory Framework/Slot Types/Drop On Character Zone")]
	public class DropOnCharacterZone : SlotType
	{
		public override void OnClick(UI_Slot slot, UI_InventoryManager invManager)
		{
		
		}

		public override void OnDropItem(UI_Slot slot, Item item, UI_InventoryManager invManager)
		{
			invManager.LoadCurrentItemOnEquipmentSlot();
		}
	}
}
