using UnityEngine;

namespace Scenes.Game.Scripts
{
	/// <summary>
	/// Interface of a component that can receive a hit for it's GameObject. 
	/// </summary>
	public interface IHittable
	{
		/// <summary>
		/// Cause the GameObject damage.
		/// </summary>
		/// <param name="normal">Normal of collision with damage giver.</param>
		/// <returns>True if killed.</returns>
		bool TakeHit(Vector2 normal);
	}
}