using System;
using UnityEngine;

namespace Scenes.Game.Scripts.Mario.Fireballs
{
	/// <summary>
	/// The component in charge of throwing fireballs. Is active only when the corresponding power up is equipped. 
	/// </summary>
	[RequireComponent(typeof(PlayerMovement))]
	public class ThrowFireballs : MonoBehaviour
	{
		public Action ThrowAnimation;

		#region Serialized fields

		[SerializeField] private KeyCode throwKey = KeyCode.LeftControl;
		[SerializeField] private GameObject fireballPrefab;
		[SerializeField] private float throwForce;

		#endregion

		#region Private fields

		private PlayerMovement _player;
		private Collider2D _collider;

		#endregion

		#region Function events

		private void Awake()
		{
			_player = GetComponent<PlayerMovement>();
			_collider = GetComponent<Collider2D>();
		}

		private void Update()
		{
			if (!Input.GetKeyDown(throwKey)) return;
			ThrowFireball();
		}

		#endregion

		#region Private methods

		/// <summary>
		/// Throws a single fireball.
		/// </summary>
		private void ThrowFireball()
		{
			var fireball = Instantiate(fireballPrefab, transform.position + 0.3f * Vector3.up, Quaternion.identity);
			var velocity = Vector2.right * throwForce;
			fireball.GetComponent<Rigidbody2D>().velocity = _player.Direction == Direction.Right ? velocity : -velocity;
			AudioManager.Fireball();
			ThrowAnimation();
		}

		#endregion
	}
}