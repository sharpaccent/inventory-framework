using UnityEngine;
using System.Collections;

namespace SA.Characters
{
	[System.Serializable]
	public class CharacterPart
	{
		public SkinnedMeshRenderer meshRenderer;
		public Items.BodyPart bodyPart;
		[HideInInspector]
		public Mesh defaultMesh;
		[HideInInspector]
		public Material defaultMaterial;
	}
}
