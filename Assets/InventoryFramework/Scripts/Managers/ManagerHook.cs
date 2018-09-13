using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SA.InventoryFramework
{
	public class ManagerHook : MonoBehaviour
	{
		public UnityEvent onAwake;

		private void Awake()
		{
			Invoke("ExecuteEvent", 0.1f);
		}

		void ExecuteEvent()
		{
			onAwake.Invoke();
		}
	}
}
