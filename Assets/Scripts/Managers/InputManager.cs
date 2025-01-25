using KKL.LiDAR;
using KKL.Player;
using KKL.Utils;
using UnityEngine;

namespace KKL.Managers
{
    public class InputManager : Singleton<InputManager>
    {
        
        [SerializeField] private Lidar lidar;
        [SerializeField] private PointRenderer pointRenderer;
        
        private FirstPersonController _controller;
        private PlayerInput _playerInput;
        private PlayerInput.PlayerActions _playerActions;
        private PlayerInput.UIActions _uiActions;
        
        protected override void Awake()
        {
            InitializeComponents();
            SetupInputActions();
        }

        private void OnEnable()
        {
            _playerActions.Enable();
        }

        private void FixedUpdate()
        {
            _controller.HandleMovement(_playerActions.Move.ReadValue<Vector2>());
            
            if (lidar.IsPainting)
            {
                lidar.Painter.Paint();
            }
        }
        
        private void LateUpdate()
        {
            _controller.HandleCamera(_playerActions.Look.ReadValue<Vector2>());
        }

        private void OnDisable()
        {
            _playerActions.Disable();
        }
        
        private void InitializeComponents()
        {
            _playerInput = new PlayerInput();
            
            _playerActions = _playerInput.Player;
            _uiActions = _playerInput.UI;
            
            if (!_controller)  _controller = GetComponent<FirstPersonController>();
            if (!lidar) lidar = GetComponent<Lidar>();
        }
        
        private void SetupInputActions()
        {
            _playerActions.Enable();
            
            // Jump
            _playerActions.Jump.performed += _ =>
            {
                _controller.HandleJump();
            };
            
            // Sprint
            _playerActions.Sprint.performed += _ =>
            {
                _controller.StartSprinting();
            };
            
            _playerActions.Scan.canceled += _ =>
            {
                _controller.StopSprinting();
            };
            
            // Crouch
            _playerActions.Crouch.performed += _ =>
            {
                _controller.HandleCrouch();
            };
            
            
            // Lidar actions
            _playerActions.Scan.started += _ => ChangeLidarState(true);
            _playerActions.Scan.canceled += _ => ChangeLidarState(false);
            _playerActions.Paint.canceled += _ => ChangeLidarPaintingState(false);
            _playerActions.Paint.started += _ => ChangeLidarPaintingState(true);
            
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
    }
}