// Script by : Nanatchy
// Porject : Metroid Like

using System;
using System.Collections;
using System.Transactions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Player
{
	public class TriggerCollidePlayer : MonoBehaviour
	{
		#region Attributs

		[SerializeField] private string enemy;

		public bool Invincibility { get; set; } = false;
		public bool IsHitDamage { get; set; } = false;

		#endregion

		#region Methods



		#endregion

		#region InputSystem



		#endregion

		#region Behaviors

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag(enemy) && !Invincibility)
			{
				IsHitDamage = true;
				Invincibility = true;
			}
		}

		private void Start()
		{
			
		}

		private void Update()
		{
        
		}
		
		#endregion
	}
}
