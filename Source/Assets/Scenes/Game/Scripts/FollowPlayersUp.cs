using UnityEngine;

namespace Scenes.Game.Scripts
{
	/// <summary>
	/// A component for the camera to follow the players only up.
	/// </summary>
	public class FollowPlayersUp : MonoBehaviour
	{
		[SerializeField] private Rigidbody2D[] playersRigidbody2D;

		private void Update()
		{
			var pos = transform.position;
			var y = pos.y;
			foreach (var player in playersRigidbody2D)
				if (player.IsAwake() && player.velocity.y > 0 && player.position.y - 3 > y)
					y = player.position.y - 3;
			pos.y = y;
			transform.position = pos;
		}
	}
}