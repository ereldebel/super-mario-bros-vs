using System;
using UnityEngine;
using static Scenes.Game.Scripts.CollisionDirection;

namespace Scenes.Game.Scripts.NPC.Enemies
{
	/// <summary>
	/// A Koopa Troopa Shell (The turtle's shell) component.
	/// In charge of it's movement, Mario and enemy hits and self destruction if out of frame.
	/// </summary>
	public class KoopaTroopaShell : Goomba
	{
		#region Public and protected methods

		/// <summary>
		/// Takes a hit. Changes movement direction to be away from collision.
		/// </summary>
		/// <param name="normal">Normal of collision with damage giver.</param>
		/// <returns>True if died. Always false.</returns>
		public override bool TakeHit(Vector2 normal)
		{
			var velocity = Math.Abs(CurrentMovement);
			CurrentMovement = normal.x > 0 ? -velocity : velocity;
			return false;
		}

		/// <summary>
		/// Hits Hittable objects on collision. Changes direction on other side collisions.
		/// </summary>
		/// <param name="other">Collision.</param>
		protected override void CollisionBehaviour(Collision2D other)
		{
			var normal = other.GetContact(0).normal;
			if (!CollidedFromSide(normal) || CurrentMovement * normal.x >= 0)
				return;
			var hittable = other.gameObject.GetComponent<IHittable>();
			if (hittable != null)
				other.gameObject.GetComponent<IHittable>().TakeHit(other.GetContact(0).normal);
			else
				CurrentMovement = -CurrentMovement;
		}

		#endregion
	}
}