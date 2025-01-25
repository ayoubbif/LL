using KKL.Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace KKL.UI_Toolkit.Panels
{
    public class MainMenuPresenter : MonoBehaviour
    {
        [SerializeField] private MenuManager menuManager;
        private UIDocument _document;
    
        private void Awake()
        {
            _document = GetComponent<UIDocument>();

            var root = _document.rootVisualElement;

            var playButton = root.Q<Button>("Play");
            var settingsButton = root.Q<Button>("Settings");
            var quitButton = root.Q<Button>("Quit");

            playButton.clicked += MenuManager.StartGame;
            settingsButton.clicked += () => menuManager.ShowSettingsMenu();
            quitButton.clicked += MenuManager.QuitGame;
        }
    }
}
