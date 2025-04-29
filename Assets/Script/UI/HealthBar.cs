// Project : My_RogueLike
// Script by : Nanatchy


using System;
using Script.Player;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Script.UI
{
	public class HealthBar : MonoBehaviour
	{
		#region Attributs

		private float health = 0f;
		private float maxHealth = 0f;
		private float emptyHealth = 0f;

		private PlayerMove _playerMove;
		
		[SerializeField] private Image healthBarImage;
		[SerializeField] private Image emptyHealthBarImage;
		
		#endregion

		#region Methods



		#endregion

		#region Behaviors

		private void Start()
		{
			_playerMove = GetComponent<PlayerMove>();
			maxHealth = _playerMove.LifeMax;
		}

		private void Update()
		{
			health = _playerMove.Life;
			emptyHealth = _playerMove.LifeMaxCurrent;
			healthBarImage.fillAmount = health / maxHealth;
			emptyHealthBarImage.fillAmount = emptyHealth / maxHealth;
		}
	
		#endregion
	}
}
