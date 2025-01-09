using KKL.LiDAR;
using KKL.Player;
using KKL.Utils;
using UnityEngine;

namespace KKL.Managers
{
    public class InputManager : Singleton<InputManager>
    {
        [Header("Components")]
        [SerializeField] private PlayerController playerController;
        [SerializeField] private PlayerLook playerLook;
        [SerializeField] private Lidar lidar;
        [SerializeField] private PointRenderer pointRenderer;
        
        private PlayerInput _playerInput;
        private PlayerInput.PlayerActions _playerActions;
        private PlayerInput.UIActions _uiActions;
        
        protected override void Awake()
        {
            InitializeComponents();
            SetupInputActions();
        }
        
        private void InitializeComponents()
        {
            _playerInput = new PlayerInput();
            
            _playerActions = _playerInput.Player;
            
            if (!playerController) playerController = GetComponent<PlayerController>();
            if (!playerLook) playerLook = GetComponent<PlayerLook>();
            if (!lidar) lidar = GetComponent<Lidar>();
        }
        
        private void SetupInputActions()
        {
            _playerActions.Enable();
            
            // Jump
            _playerActions.Jump.performed += _ =>
            {
                playerController.Jump();
            };
            
            // Shooting actions
            _playerActions.Scan.started += _ => ChangeLidarState(true);
            _playerActions.Scan.canceled += _ => ChangeLidarState(false);
            _playerActions.Paint.started += _ => ChangeLidarPaintingState(true);
            _playerActions.Paint.canceled += _ => ChangeLidarPaintingState(false);
            
            // Pause Menu
            _uiActions.Pause.started += _ =>
            {
                GameManager.Instance.TogglePauseMenu();
            };
            
            // Clear Points
            _playerActions.Clear.performed += _ =>
            {
                pointRenderer.ClearAllPoints();
            };
        }
        
        private void ChangeLidarState(bool scanning)
        {
            lidar.IsScanning = scanning;
        }
        
        private void ChangeLidarPaintingState(bool painting)
        {
            lidar.IsPainting = painting;
        }
        
        private void FixedUpdate()
        {
            playerController.ProcessMove(_playerActions.Move.ReadValue<Vector2>());
        }
        
        private void LateUpdate()
        {
            playerLook.ProcessLook(_playerActions.Look.ReadValue<Vector2>());
        }

        private void OnDisable()
        {
            _playerActions.Disable();
        }
    }
}