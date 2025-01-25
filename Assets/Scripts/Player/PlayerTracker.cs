using KKL.LiDAR;
using UnityEngine;

namespace KKL.Player
{
    public class PlayerTracker : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private PointRenderer pointRenderer;
        
        private void Update()
        {
            pointRenderer.SetRefPosition(player.position);
        }
    }
}