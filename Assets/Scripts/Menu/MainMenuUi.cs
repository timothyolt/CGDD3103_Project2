using System.IO;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Menu
{
    public class MainMenuUi : MonoBehaviour
    {
        public Button CloseButton;
        public Button OptionsButton;
        public Button PlayNewButton;
        public Button PlayLoadButton;

        [UsedImplicitly]
        private void Start()
        {
            CloseButton?.onClick.AddListener(Application.Quit);
            OptionsButton?.onClick.AddListener(() =>
            {
                var main = Instantiate(Resources.Load<GameObject>("Menus/Options"));
                main.transform.SetParent(transform.parent);
                main.transform.position = transform.position;
                main.transform.localScale = transform.localScale;
                Destroy(gameObject);
            });
            PlayLoadButton?.onClick.AddListener(() => SceneManager.LoadScene(1));
            PlayNewButton?.onClick.AddListener(() =>
            {
                try
                {
                    File.Delete($"{Application.persistentDataPath}/gamesave");
                } catch (IOException) { }
                SceneManager.LoadScene(1);
            });
            GameSettings.Load();
        }
    }
}
