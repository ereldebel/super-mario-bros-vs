using UnityEngine;

namespace Scenes.Game.Scripts.NPC.Enemies
{
	/// <summary>
	/// A Koopa Troopa (The turtle enemy) enemy component.
	/// In charge of it's movement, Mario hits and self destruction if out of frame.
	/// </summary>
	public class KoopaTroopa : Goomba
	{
		private bool _createdShell = false;
		[SerializeField] private GameObject shellPrefab;

		/// <summary>
		/// Takes a hit. Dies and instantiates it's shell in it's place.
		/// </summary>
		/// <param name="normal">Normal of collision with damage giver.</param>
		/// <returns>True if died. always true.</returns>
		public override bool TakeHit(Vector2 normal)
		{
			if (_createdShell) return false;
			_createdShell = true;
			Instantiate(shellPrefab, transform.position - new Vector3(0, 0.281f, 0), Quaternion.identity);
			Destroy(gameObject);
			return true;
		}
	}
}