using Scenes.Game.Scripts.Mario;
using UnityEngine;

namespace Scenes.Game.Scripts.LevelEnd
{
	public class FlagPole : MonoBehaviour
	{
		[SerializeField] private Flag flag;

		private void OnCollisionEnter2D(Collision2D other)
		{
			var cutsceneController = other.gameObject.GetComponent<CutsceneController>();
			if (cutsceneController == null) return;
			cutsceneController.LowerFlag = () => StartCoroutine(flag.LowerFlag());
			cutsceneController.enabled = true;
			GameManager.FlagPoleSlide(other.gameObject.GetComponent<MarioManager>().Player,
				(other.gameObject.transform.position.y - transform.position.y + 3) * 1.5f);
		}
	}
}