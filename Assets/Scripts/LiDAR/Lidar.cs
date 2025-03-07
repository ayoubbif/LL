﻿using UnityEngine;

namespace KKL.LiDAR
{
    public class Lidar : MonoBehaviour
    {
        [SerializeField] private PointRenderer pointRenderer;
        
        [SerializeField] private GameObject rayPrefab;
        [SerializeField] private float rayDistance;
        [SerializeField] private LayerMask layerMask;
        
        [SerializeField] private GameObject scanIndicator;
        
        [SerializeField] private Scanner scanner;
        [SerializeField] private Painter painter;
        
        public bool IsScanning
        {
            get => scanner.IsScanning;
            set
            {
                scanner.IsScanning = value;
                IsPainting = !IsScanning && IsPainting;
            }
        }
        
        public bool IsPainting
        {
            get => painter.IsPainting;
            set
            {
                if(IsScanning)
                    return;
                
                painter.IsPainting = value;
            }
        }
        
        private void Awake()
        {
            painter.Setup(pointRenderer, rayDistance, layerMask, rayPrefab);
            scanner.Setup(pointRenderer, rayDistance, layerMask, rayPrefab);
        }
        
        private void FixedUpdate()
        {
            // Must set scan indicator active first since IsPainting tries to deactivate itself every frame
            scanIndicator.SetActive(IsPainting || IsScanning);
            painter.Paint();
            scanner.Scan(Time.fixedDeltaTime);
        }
        
        public void AdjustPaintAngle(float scrollDelta)
        {
            painter.AdjustAngle(scrollDelta);
        }
    }
}