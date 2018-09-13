using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA.Items
{
	[CreateAssetMenu]
	public class BodyPart : ScriptableObject
	{
		public bool isWeapon;
		public bool isDisabledWhenEmpty;
	}
}
