using System;
using System.Collections;
using _NBGames.Scripts.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _NBGames.Scripts.Behaviors
{
    public class GateEnterZone : MonoBehaviour
    {
        [SerializeField] private string _levelToLoad;
        [SerializeField] private GameObject _buttonPrompt;

        private bool _canInteract, _isEntering;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!enabled) return;
            _canInteract = true;
            _buttonPrompt.SetActive(true);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!enabled) return;
            _canInteract = false;
            _buttonPrompt.SetActive(false);
        }

        private void Update()
        {
            if (!_canInteract || !Input.GetKeyDown(KeyCode.E) || _isEntering) return;
            UIManager.instance.Fade(true);
            StartCoroutine(LoadLevel());
            
            EventManager.LoadLevel();
            _isEntering = true;
        }

        private IEnumerator LoadLevel()
        {
            yield return new WaitForSeconds(2);
            SceneManager.LoadScene(_levelToLoad);
           
        }
    }
}
