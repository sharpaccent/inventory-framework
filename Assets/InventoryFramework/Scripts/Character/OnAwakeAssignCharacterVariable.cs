using UnityEngine;
using System.Collections;

namespace SA.Characters
{
	public class OnAwakeAssignCharacterVariable : MonoBehaviour
	{
		public CharacterReferencesVariable variable;

		private void Awake()
		{
			variable.value = GetComponent<CharacterReferences>();
		}
	}
}
