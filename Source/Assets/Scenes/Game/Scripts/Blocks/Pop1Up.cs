using UnityEngine;

namespace Scenes.Game.Scripts.Blocks
{
	public class Pop1Up : MonoBehaviour, ISpecialBlockStrategy
	{
		#region Private fields

		[SerializeField] private GameObject oneUpEntity;
		[SerializeField] private float blockSize = 0.67f;
		private bool _bought;

		#endregion

		#region Public methods

		/// <summary>
		/// Plays power up behaviour.
		/// </summary>
		public void CollisionBehaviour(PlayerIdentifier player)
		{
			_bought = GameManager.Buy1Up(player);
			if (!_bought) return;
			AudioManager.PowerUpAppear();
		}

		/// <summary>
		/// Shows block and instantiates 1up mushroom.
		/// </summary>
		/// <param name="isGiant">True if mario is a giant.</param>
		public void BlockActivated(bool isGiant)
		{
			if (!_bought) return;
			var instantiationPosition = transform.position + new Vector3(0, blockSize, 0);
			Instantiate(oneUpEntity, instantiationPosition, Quaternion.identity, transform);
		}

		#endregion
	}
}