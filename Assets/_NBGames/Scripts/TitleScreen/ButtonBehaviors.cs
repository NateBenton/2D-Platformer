using _NBGames.Scripts.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _NBGames.Scripts.TitleScreen
{
    public class ButtonBehaviors : MonoBehaviour
    {
        private bool _newGameActivated;
        
        public void NewGameButton()
        {
            if (_newGameActivated) return;
            StartCoroutine(UIManager.instance.NewGameFade());

            _newGameActivated = true;
        }

        public void ExitGameButton()
        {
            Application.Quit();
        }
    }
}
