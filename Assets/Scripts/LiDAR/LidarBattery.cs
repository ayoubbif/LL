using TMPro;
using UnityEngine;

namespace KKL.LiDAR
{
    public class LidarBattery : MonoBehaviour
    {
        [SerializeField] private float maxBatteryCharge = 100f;
        [SerializeField] private float scanDrainRate = 10f; // Battery drain per second while scanning
        [SerializeField] private float paintDrainRate = 5f; // Battery drain per second while painting
        [SerializeField] private float minChargeRequired = 10f; // Minimum battery needed to start scanning/painting

        [SerializeField] private TextMeshProUGUI batteryText;
        private float _currentCharge;
        private Lidar _lidar;

        private void Awake()
        {
            _currentCharge = maxBatteryCharge;
            _lidar = GetComponent<Lidar>();
        }

        private void Update()
        {
            if (_lidar.IsScanning)
            {
                _currentCharge -= scanDrainRate * Time.deltaTime;
            }
            else if (_lidar.IsPainting)
            {
                _currentCharge -= paintDrainRate * Time.deltaTime * 4;
            }

            _currentCharge = Mathf.Max(0f, _currentCharge);
            
            batteryText.text = $"{GetBatteryPercentage():F0}%";

            // Automatically stop activities if battery depleted
            if (!(_currentCharge <= 0f)) return;
            _lidar.IsScanning = false;
            _lidar.IsPainting = false;
        }

        public bool CanActivate()
        {
            return _currentCharge >= minChargeRequired;
        }

        private float GetBatteryPercentage()
        {
            return (_currentCharge / maxBatteryCharge) * 100f;
        }
    }
}