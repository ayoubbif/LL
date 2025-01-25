using KKL.Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace KKL.UI_Toolkit.Panels
{
    public class SettingsPresenter : MonoBehaviour
    {
        [SerializeField] private MenuManager menuManager;
        private UIDocument _document;
    
        private void Awake()
        {
            _document = GetComponent<UIDocument>();

            var root = _document.rootVisualElement;

            var returnButton = root.Q<Button>("Return");

            returnButton.clicked += () => menuManager.ShowMainMenu();
        }
    }
}
