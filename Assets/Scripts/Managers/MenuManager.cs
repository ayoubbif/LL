using KKL.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace KKL.Managers
{
    public class MenuManager : Singleton<MenuManager>
    {
        [SerializeField] private UIDocument mainMenuUI;
        [SerializeField] private UIDocument settingsMenuUI;

        private void Start()
        {
            // Check if GameObjects are assigned.
            if (mainMenuUI != null && settingsMenuUI != null) 
                return;
            
            Debug.LogError("One or more of the required GameObjects is not assigned in the editor.");
        }

        public static void StartGame()
        {
            Debug.Log("Play");
            // Check if the "Game" scene exists.
            if (Application.CanStreamedLevelBeLoaded("Game"))
            {
                SceneManager.LoadScene("Game");
                CursorUtils.LockCursor();
            }
            else
            {
                Debug.LogError("The specified scene does not exist.");
            }
        }

        public void ShowSettingsMenu()
        {
            var mainMenuRoot = mainMenuUI.rootVisualElement;
            var settingsRoot = settingsMenuUI.rootVisualElement;

            var settingsPanel = settingsRoot.Q<VisualElement>("Background");
            var mainMenuPanel = mainMenuRoot.Q<VisualElement>("Background");
            
            mainMenuPanel.visible = false;
            settingsPanel.visible = true;
        }

        public void ShowMainMenu()
        {
            var mainMenuRoot = mainMenuUI.rootVisualElement;
            var settingsRoot = settingsMenuUI.rootVisualElement;

            var settingsPanel = settingsRoot.Q<VisualElement>("Background");
            var mainMenuPanel = mainMenuRoot.Q<VisualElement>("Background");
            
            settingsPanel.visible = false;
            mainMenuPanel.visible = true;
            
            CursorUtils.UnlockCursor();
        }
    
        public static void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit(); 
#endif
        }
    }
}