using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA.Characters
{
	public class CharacterReferences : MonoBehaviour
	{
		public CharacterPart[] parts;
		Dictionary<Items.BodyPart, CharacterPart> partsDict = new Dictionary<Items.BodyPart, CharacterPart>();

		Items.Weapon rightHandWeapon;
		Items.Weapon leftHandWeapon;
		public Transform rightHand;
		public Transform leftHand;

		InventoryFramework.ResourcesManager rm;

		private void Awake()
		{
			for (int i = 0; i < parts.Length; i++)
			{
				if (!partsDict.ContainsKey(parts[i].bodyPart))
				{
					partsDict.Add(parts[i].bodyPart, parts[i]);
					parts[i].defaultMesh = parts[i].meshRenderer.sharedMesh;
					parts[i].defaultMaterial = parts[i].meshRenderer.material;
				}
			}
		}

		CharacterPart GetPart(Items.BodyPart bodyPart)
		{
			CharacterPart result = null;
			partsDict.TryGetValue(bodyPart, out result);
			return result;
		}

		public void LoadWeapon(Items.Weapon weapon, bool isLeft)
		{
			Transform par =(isLeft)? leftHand : rightHand;
			if (isLeft)
				leftHandWeapon = weapon;
			else
				rightHandWeapon = weapon;

			GameObject go = weapon.weaponRuntime.modelInstance;
			if (weapon.weaponRuntime.modelInstance == null)
			{
				go = Instantiate(weapon.modelPrefab) as GameObject;
				weapon.weaponRuntime.modelInstance = go;
			}

			go.transform.parent = par;
			go.transform.localPosition = Vector3.zero;
			go.transform.localRotation = Quaternion.identity;
			go.transform.localScale = Vector3.one;
			go.SetActive(true);
		}

		public void UnEquipWeapon(bool isLeft)
		{
			Items.Weapon w = (isLeft) ? leftHandWeapon : rightHandWeapon;
	
			if (w == null)
				return;
			if (w.weaponRuntime.modelInstance)
			{
				
				w.weaponRuntime.modelInstance.SetActive(false);
			}

			if (isLeft)
				leftHandWeapon = null;
			else
				rightHandWeapon = null;

		}

		public void LoadItemOnPart(Items.ClothItem item, Items.BodyPart bodyPart)
		{
			CharacterPart part = GetPart(bodyPart);
			if (item == null)
			{
				if (bodyPart.isDisabledWhenEmpty)
				{
					if (part != null)
						part.meshRenderer.enabled = false;
				}
				else
				{
					if (part != null)
					{
						part.meshRenderer.sharedMesh = part.defaultMesh;
						part.meshRenderer.material = part.defaultMaterial;
						part.meshRenderer.enabled = true;
					}
				}
			}
			else
			{
				part.meshRenderer.sharedMesh = item.mesh;
				part.meshRenderer.material = item.material;
				part.meshRenderer.enabled = true;
			}
		}

		public void LoadItemsFromStoredIds()
		{
			if(rm == null)
				rm = Resources.Load("ResourcesManager") as InventoryFramework.ResourcesManager;

			for (int i = 0; i < rm.currentIds.Length; i++)
			{
				Items.Item it = rm.GetItemInstance(rm.currentIds[i].instId);
				if (it == null)
				{
					if (rm.currentIds[i].bodyPart.isWeapon)
					{
						UnEquipWeapon(rm.currentIds[i].isLeft);
					}
					else
					{
						LoadItemOnPart(null, rm.currentIds[i].bodyPart);
					}
				}
				else
				if (it is Items.ClothItem)
				{
					Items.ClothItem c = (Items.ClothItem)it;
					LoadItemOnPart(c, rm.currentIds[i].bodyPart);
				}
				else if(it is Items.Weapon)
				{
					Items.Weapon w = (Items.Weapon)it;
					LoadWeapon(w, rm.currentIds[i].isLeft);
				}
			}
		}
	}
}
