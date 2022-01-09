using System.Collections;
using Scenes.Game.Scripts.Mario;
using UnityEngine;

namespace Scenes.Game.Scripts.LevelEnd
{
    public class Flag : MonoBehaviour
    {
        private const float LoweringSpeed = 0.1f;

        private void OnTriggerEnter2D(Collider2D other)
        {
            var cutsceneController = other.gameObject.GetComponent<CutsceneController>();
            cutsceneController.LowerFlag = () => StartCoroutine(LowerFlag());
            cutsceneController.GotFlag(gameObject);
        }
        
        /// <summary>
        /// Lowers the flag down the pole.
        /// </summary>
        public IEnumerator LowerFlag()
        {
            while (gameObject.activeSelf)
            {
                var pos = transform.position;
                pos += Vector3.down * LoweringSpeed;
                transform.position = pos;
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
