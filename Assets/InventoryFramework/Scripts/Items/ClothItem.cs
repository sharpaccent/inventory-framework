using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA.Items
{
	[CreateAssetMenu(menuName ="Items/Cloth Item")]
	public class ClothItem : Item
	{
		public Mesh mesh;
		public Material material;
	}
}
