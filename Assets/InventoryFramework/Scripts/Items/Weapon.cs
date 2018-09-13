using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA.Items
{
	[CreateAssetMenu(menuName ="Items/Weapon")]
	public class Weapon : Item
	{
		public GameObject modelPrefab;
		public WeaponRuntime weaponRuntime;
	}
}
