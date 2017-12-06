using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using FallingSloth;

namespace EclipseStudios.Orbital
{
    public class ContinueScreen : SingletonBehaviour<ContinueScreen>
    {
        static Animator menu;

        public RectTransform adButton;

        protected override void Awake()
        {
            base.Awake();

            menu = GetComponent<Animator>();
        }

        public static void Show()
        {
            if (GameManager.Instance.hasContinued)
                Instance.adButton.localScale = Vector3.zero;
            
            menu.SetTrigger("Open");
        }

        public void PlayAgain()
        {
            menu.SetTrigger("Close");

            GameManager.PlayAgain();
        }

        public void WatchAdToContinue()
        {
            menu.SetTrigger("Close");

            GameManager.WatchAdToContinue();
        }

        public void MainMenu()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
}