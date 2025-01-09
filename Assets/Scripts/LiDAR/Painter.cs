using System;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace KKL.LiDAR
{
    [Serializable]
    public class Painter : LidarBase
    {
        [Serializable]
        private struct Angles
        {
            public float min;
            public float max;
            public float initial;
        }
        
        [SerializeField] private Transform rayContainer;
        [SerializeField] private int raysPerLayer;
        [SerializeField] private int numOfLayers;
        [SerializeField] private Angles angles;
        [SerializeField] private float angleAdjustSensitivity;
        
        private float _paintAngle;
        private GameObject[,] _paintRays;
        private bool _isPainting;
        
        public bool IsPainting
        {
            get => _isPainting;
            set
            {
                _isPainting = value;
                ActivateRays(_paintRays, _isPainting);
            }
            
        }
        public override void InitRays(GameObject rayPrefab)
        {
           _paintRays = new GameObject[raysPerLayer, numOfLayers];
           _paintAngle = angles.initial;
           
              for (var i = 0; i < numOfLayers; i++)
              {
                for (var j = 0; j < raysPerLayer; j++)
                {
                     _paintRays[i, j] = Object.Instantiate(rayPrefab, rayContainer);
                }
              }
              
              IsPainting = false;
        }
        
        public void AdjustAngle(float scrollDelta)
        {
            if(!_isPainting)
                return;

            _paintAngle = Mathf.Clamp(_paintAngle + scrollDelta * angleAdjustSensitivity, angles.min, angles.max);
        }
        
        public void Paint()
        {
            if(!_isPainting)
                return;

            AdjustRays();
            IsPainting = false;
        }
        
        private void AdjustRays()
        {
            var hit = new RaycastHit();

            for (var i = 0; i < numOfLayers; i++)
            {
                var angleFromCenter = Random.Range(
                    _paintAngle * (i / (float)numOfLayers),
                    _paintAngle * ((i + 1) / (float)numOfLayers));

                var radianOffset = Random.Range(0, 2 * Mathf.PI);

                for (var j = 0; j < raysPerLayer; j++)
                {
                    var radians = 2 * Mathf.PI * (j / (float)raysPerLayer) + radianOffset;
                    if(AdjustRayFromRaycast(_paintRays[i,j].transform, angleFromCenter, radians, ref hit))
                        CreateDotFromRaycast(hit);
                }
            }
        }

        private bool AdjustRayFromRaycast(Transform ray, float angleFromCenter, float radians, ref RaycastHit hit)
        {
            ray.localEulerAngles = angleFromCenter * new Vector3(Mathf.Sin(radians), Mathf.Cos(radians), 0);

            var successfulHit = Raycast(ray.position, ray.forward, out hit);
            ResizeRay(ray, hit.distance);

            return successfulHit;
        }
    }
}