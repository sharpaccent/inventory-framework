using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SA.InventoryFramework
{
	public class UI_InventoryManager : MonoBehaviour
	{
		[SerializeField]
		ResourcesManager r_manager;
		[SerializeField]
		private int slotAmount = 10;
		[SerializeField]
		private GameObject slotPrefab;
		[SerializeField]
		private Transform slotGrid;
		[SerializeField]
		private Transform equipmentGrid;
		[SerializeField]
		private Transform mouseFollower;
		[SerializeField]
		private Image mouseIcon;
		[SerializeField]
		Characters.CharacterReferencesVariable character;

		public GameObject dropText;

		List<UI_Slot> inventorySlots = new List<UI_Slot>();
		Dictionary<Items.BodyPart, UI_Slot> equipmentSlots = new Dictionary<Items.BodyPart, UI_Slot>();
		private UI_Slot currentSlot;
		private Items.Item currentItem;
		private UI_Slot storedSlot;

		private UI_Slot leftHandSlot;
		private UI_Slot rightHandSlot;
		bool mouseIsDown;
		bool mouseIsUp;

		public static UI_InventoryManager singleton;

		#region Init
		private void Awake()
		{
			if (singleton == null)
			{
				singleton = this;
			}
			else
			{
				Destroy(this.transform.parent.gameObject);
			}
		}

		private void Start()
		{
			//Init();	
		}

		public void Init()
		{
			for (int i = 0; i < slotAmount; i++)
			{
				GameObject go = Instantiate(slotPrefab) as GameObject;
				go.transform.SetParent(slotGrid);
				go.SetActive(true);
				inventorySlots.Add(go.GetComponent<UI_Slot>());
			}

			mouseIcon.enabled = false;

			UI_Slot[] eq = equipmentGrid.GetComponentsInChildren<UI_Slot>();
			for (int i = 0; i < eq.Length; i++)
			{
				if (eq[i].bodyPart != null)
				{
					if (eq[i].isWeaponSlot)
					{
						if (eq[i].isLeft)
							leftHandSlot = eq[i];
						else
							rightHandSlot = eq[i];
					}

					if (!equipmentSlots.ContainsKey(eq[i].bodyPart))
					{
						equipmentSlots.Add(eq[i].bodyPart, eq[i]);
					}
				}
			}
		}
		#endregion

		#region Update
		private void Update()
		{
			FindCurrentSlot();
			MouseInput();
			DetectAction();
		}

		void FindCurrentSlot()
		{
			PointerEventData pointerData = new PointerEventData(EventSystem.current)
			{
				position = Input.mousePosition
			};

			List<RaycastResult> results = new List<RaycastResult>();
			EventSystem.current.RaycastAll(pointerData, results);

			for (int i = 0; i < results.Count; i++)
			{
				currentSlot = results[i].gameObject.GetComponentInParent<UI_Slot>();
			}

			if (results.Count == 0)
			{
				currentSlot = null;
			}
		}

		void MouseInput()
		{
			mouseIsDown = Input.GetMouseButtonDown(0);
			mouseIsUp = Input.GetMouseButtonUp(0);

			mouseFollower.position = Input.mousePosition;
		}

		void DetectAction()
		{
			if (mouseIsDown)
			{
				if (currentItem == null)
				{
					if (currentSlot == null)
						return;

					currentSlot.OnClick(this);
				}
			}

			if (mouseIsUp)
			{
				if (currentItem != null)
				{
					if (currentSlot != null)
					{
						currentSlot.OnDropItem(currentItem, this);
					}
				}

				if (currentItem != null)
				{
					SetItemOnStoredSlot(currentItem);
				}

				currentItem = null;
				storedSlot = null;
				mouseIcon.sprite = null;
				mouseIcon.enabled = false;
			}
		}
		#endregion

		#region Manager Methods
		public void ClearCurrentItem()
		{
			currentItem = null;
		}

		List<UI_Slot> GetAvailableSlots()
		{
			List<UI_Slot> result = new List<UI_Slot>();
			for (int i = 0; i < inventorySlots.Count; i++)
			{
				if (inventorySlots[i].itemInstance == null)
				{
					result.Add(inventorySlots[i]);
				}
			}

			return result;
		}

		public void LoadCurrentItems()
		{
			
			List<Items.Item> holdingItems = r_manager.GetCurrentItems();
			List<UI_Slot> availableSlots = GetAvailableSlots();

			int index = 0;

			for (int i = 0; i < holdingItems.Count; i++)
			{
				if (i < availableSlots.Count -1 && index < availableSlots.Count-1)
				{
					if (!holdingItems[i].isEquiped && holdingItems[i].currentSlot == null)
					{
						holdingItems[i].currentSlot = availableSlots[i];
						availableSlots[index].LoadItem(holdingItems[i]);
						index++;
					}
					else
					{
						holdingItems[i].currentSlot.LoadItem(holdingItems[i]);
					}
				}
			}
		}

		public void SetCurrentItem(Items.Item targetItem, UI_Slot slot)
		{
			storedSlot = slot;
			currentItem = targetItem;
			if (currentItem != null)
			{
				mouseIcon.sprite = currentItem.ui_info.icon;
				mouseIcon.enabled = true;
			}
		}

		public void SetItemOnStoredSlot(Items.Item targetItem)
		{
			storedSlot.LoadItem(targetItem);
		}

		public void LoadItemOnTargetEquipmentSlot(Items.Item item, bool isLeft)
		{
			item.isEquiped = true;

			UI_Slot slot = null;
			if (item is Items.Weapon)
			{
				slot = GetWeaponEquipmentSlot(isLeft);
			}
			else
			{
				slot = GetEquipmentSlot(item.bodyPart);
			}

			slot.LoadItem(item);
			StoreIdOnResources(item, slot);
		}

		public void LoadCurrentItemOnEquipmentSlot()
		{
			if (currentItem == null)
				return;

			if (currentItem is Items.ClothItem)
			{
				currentItem.isEquiped = true;
				UI_Slot slot = GetEquipmentSlot(currentItem.bodyPart);
				if (slot.itemInstance != null)
				{
					SetItemOnStoredSlot(slot.itemInstance);
				}

				slot.LoadItem(currentItem);
				StoreIdOnResources(currentItem, slot);
				LoadItemsOnCharacter();

				currentItem = null;
			}
			else
			{
				//we droped a weapon on our character
			}
		}

		public void StoreIdOnResources(Items.Item item, UI_Slot slot)
		{
			Items.BodyPart targetPart = slot.bodyPart;
			int targetInstId = -1;

			if (item != null)
			{
				targetPart = item.bodyPart;
				targetInstId = item.instId;
			}

			if (item is Items.Weapon)
			{
				IdsContainer c = r_manager.GetWeaponIdContainer(targetPart, slot.isLeft);
				c.instId = targetInstId;
			}
			else
			{
				IdsContainer c = r_manager.GetIdContainer(targetPart);
				c.instId = targetInstId;
			}
		}

		public void UnEquipFromStoredSlot()
		{
			if (storedSlot.slotType is EquipmentSlot)
			{
				if (storedSlot.itemInstance != null)
				{
					storedSlot.itemInstance.isEquiped = false;
				}

				UnloadIdOnResources(storedSlot.bodyPart, storedSlot.isWeaponSlot, storedSlot.isLeft);
				LoadItemsOnCharacter();
			}
		}

		void UnloadIdOnResources(Items.BodyPart targetPart, bool isWeapon, bool isLeft)
		{
			if (isWeapon)
			{
				IdsContainer c = r_manager.GetWeaponIdContainer(targetPart,isLeft);
				c.instId = -1;
			}
			else
			{
				IdsContainer c = r_manager.GetIdContainer(targetPart);
				c.instId = -1;
			}
		}

		public void LoadItemsOnCharacter()
		{
			if(character.value != null)
				character.value.LoadItemsFromStoredIds();
		}

		public void DropItem()
		{
			if (currentItem != null)
			{
				currentItem.currentSlot = null;
				currentItem.isEquiped = false;
				UnEquipFromStoredSlot();
				r_manager.RemoveItemFromCurrent(currentItem);
				currentItem = null;
			}
		}

		public void PickupItem(Items.Item targetItem)
		{
			if (targetItem != null)
			{
				r_manager.PickupItem(targetItem);
				LoadCurrentItems();
			}
		}

		public void EnableDropText()
		{
			if (currentItem != null)
				dropText.SetActive(true);
		}

		public void DisableDropText()
		{
			dropText.SetActive(false);
		}
		#endregion

		#region Helper Methods
		UI_Slot GetEquipmentSlot(Items.BodyPart bodyPart)
		{
			UI_Slot result = null;
			equipmentSlots.TryGetValue(bodyPart, out result);
			return result;
		}

		UI_Slot GetWeaponEquipmentSlot(bool isLeft)
		{
			return (isLeft) ? leftHandSlot : rightHandSlot;
		}

		#endregion
	}
}
