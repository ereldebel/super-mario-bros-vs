using UnityEngine;
using static Scenes.Game.Scripts.CollisionDirection;

namespace Scenes.Game.Scripts.Mario
{
	public class HopOnEnemy : MonoBehaviour
	{
		#region Serialized fields

		[SerializeField] private float hopForce = 100;

		#endregion

		#region Private fields

		private Rigidbody2D _rigidbody2D;
		private Vector2 _hop = Vector2.up;
		private MarioManager _marioManager;

		#endregion

		#region Function events

		private void Awake() => InitFields();

		private void OnCollisionEnter2D(Collision2D other)
		{
			var hittable = other.gameObject.GetComponent<IHittable>();
			if (hittable == null) return;
			HitEnemy(other, hittable);
		}

		#endregion

		#region Private methods

		/// <summary>
		/// Hits the enemy if collided from above or if invincible. If enemy died add points via game manager.
		/// </summary>
		/// <param name="other">Collision</param>
		/// <param name="hittable">The hittable component of the collided object.</param>
		private void HitEnemy(Collision2D other, IHittable hittable)
		{
			if (CollidedFromBelow(other.GetContact(0).normal))
			{
				_rigidbody2D.AddForce(_hop, ForceMode2D.Impulse);
				AudioManager.EnemyHop();
			}
			else if (!_marioManager.IsInvincible) return;

			if (hittable.TakeHit(other.GetContact(0).normal))
				_marioManager.EnemyKilled();
		}

		/// <summary>
		/// Initializes the class fields.
		/// </summary>
		private void InitFields()
		{
			_rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
			_marioManager = gameObject.GetComponent<MarioManager>();
			_hop *= hopForce;
		}

		#endregion
	}
}