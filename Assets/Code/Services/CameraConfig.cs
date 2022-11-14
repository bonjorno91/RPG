using UnityEngine;

namespace Code.Services
{
    [CreateAssetMenu(menuName = "Camera Config", fileName = "CameraConfig", order = 0)]
    public class CameraConfig : ScriptableObject
    {
        public float _distance = 10;
        public float _pitch = 0;
        public float _yaw = 35;
    }
}