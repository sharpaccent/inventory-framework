using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA.Characters
{
	[CreateAssetMenu(menuName ="Variables/Character References")]
	public class CharacterReferencesVariable : ScriptableObject
	{
		[System.NonSerialized]
		CharacterReferences _value;
		public CharacterReferences value {
			get {
				
				return _value;
						 }
			set {
				_value = value;
			}
		}
	}
}
