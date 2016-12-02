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
        public Button PlayButton;

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
            PlayButton?.onClick.AddListener(() => SceneManager.LoadScene(1));
            GameSettings.Load();
        }
    }
}
