using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes.Instructions
{
    public class CheckForInput : MonoBehaviour
    {
        private const int LevelBuildIndex = 1;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();
            if (Input.GetKeyDown(KeyCode.Space))
                SceneManager.LoadSceneAsync(LevelBuildIndex);
        }
    }
}
