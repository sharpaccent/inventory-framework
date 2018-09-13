using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA.Items;

namespace SA.InventoryFramework
{
	[CreateAssetMenu]
	public class ResourcesManager : ScriptableObject
	{
		public string[] startingItems;
		public StartingIdContainer[] startingWearingIds;

		public Item[] allItems;
		public IdsContainer[] currentIds;

		[System.NonSerialized]
		int itemCount;
		[System.NonSerialized]
		Dictionary<int, Item> createdItemsIds = new Dictionary<int, Item>();
		[System.NonSerialized]
		Dictionary<Items.BodyPart, IdsContainer> idsDict = new Dictionary<Items.BodyPart, IdsContainer>();
		[System.NonSerialized]
		Dictionary<string, Item> itemsDict = new Dictionary<string, Item>();
		[System.NonSerialized]
		List<Item> currentItems = new List<Item>();
		public List<Item> GetCurrentItems()
		{
			return currentItems;
		}

		public void Init()
		{
			itemsDict.Clear();
			idsDict.Clear();
			currentItems.Clear();

			for (int i = 0; i < allItems.Length; i++)
			{
				if (!itemsDict.ContainsKey(allItems[i].name))
				{
					itemsDict.Add(allItems[i].name, allItems[i]);
				}
			}

			for (int i = 0; i < currentIds.Length; i++)
			{
				if (!idsDict.ContainsKey(currentIds[i].bodyPart))
				{
					idsDict.Add(currentIds[i].bodyPart, currentIds[i]);
				}
			}
		}

		public void CreateStartingItems()
		{
			for (int i = 0; i < startingItems.Length; i++)
			{
				Item it = CreateItemInstance(startingItems[i]);
				if (it != null)
				{
					currentItems.Add(it);
				}
			}

			for (int i = 0; i < startingWearingIds.Length; i++)
			{
				StartingIdContainer s = startingWearingIds[i];
				if (string.IsNullOrEmpty(s.itemId))
					continue;

				Item it = CreateItemInstance(s.itemId);
				if (it != null)
				{
					currentItems.Add(it);
				}

				if (s.bodyPart.isWeapon)
				{
					UI_InventoryManager.singleton.LoadItemOnTargetEquipmentSlot(it, s.isLeft);
				}
				else
				{
					UI_InventoryManager.singleton.LoadItemOnTargetEquipmentSlot(it, false);
				}
				
			}
		}

		public Item GetItemInstance(int instId)
		{
			if (instId < 0)
				return null;

			Item result = null;
			createdItemsIds.TryGetValue(instId, out result);
			return result;
		}

		Item CreateItemInstance(string id)
		{
			Item result = null;
			Item defaultItem = GetItem(id);
			if (defaultItem != null)
			{
				result = Instantiate(defaultItem);
				result.name = defaultItem.name;
				itemCount++;
				result.instId = itemCount;
				createdItemsIds.Add(result.instId, result);

			}
			else
			{
				Debug.LogError(id + " item doesn't exist!");
			}
			
			return result;
		}

		Item GetItem(string id)
		{
			Item result = null;
			itemsDict.TryGetValue(id, out result);
			return result;
		}

		public IdsContainer GetIdContainer(Items.BodyPart bodyPart)
		{
			IdsContainer result = null;
			idsDict.TryGetValue(bodyPart, out result);
			return result;
		}

		public IdsContainer GetWeaponIdContainer(Items.BodyPart bodyPart, bool isLeft)
		{
			for (int i = 0; i < currentIds.Length; i++)
			{
				if (currentIds[i].isLeft == isLeft && currentIds[i].bodyPart == bodyPart)
				{
					return currentIds[i];
				}
			}

			return null;
		}
		
		public void RemoveItemFromCurrent(Item targetItem)
		{
			if (currentItems.Contains(targetItem))
				currentItems.Remove(targetItem);

			///TODO: replace this with the prefab you are going to have inside your world for the items
			if (targetItem.pickableInstance == null)
			{
				GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				go.transform.localScale = Vector3.one * .4f;
				PickableInstance pi = go.AddComponent<PickableInstance>();
				pi.itemInstance = targetItem;
				targetItem.pickableInstance = go;
			}

			targetItem.pickableInstance.transform.position = Vector3.zero;
			targetItem.pickableInstance.SetActive(true);
		}

		public void PickupItem(Item targetItem)
		{
			currentItems.Add(targetItem);
			if (targetItem.pickableInstance != null)
			{
				targetItem.pickableInstance.SetActive(false);
			}
		}
	}
}
