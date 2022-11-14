using System;
using Code.Installers;
using UnityEngine;
using Zenject;

namespace Code.Services
{
    public class FollowCameraService : ILateTickable
    {
        public bool IsEnable { get; private set; } = true;
        private readonly Transform _playerTransform;
        private readonly CameraConfig _cameraConfig;
        private readonly Transform _cameraTransform;

        public FollowCameraService(Camera camera,[InjectOptional(Id = BindID.PlayerTransform)] Transform playerTransform, CameraConfig cameraConfig)
        {
            _cameraConfig = cameraConfig;
            _playerTransform = playerTransform;
            _cameraTransform = camera.transform;
        }

        public void Enable() => IsEnable = true;

        public void Disable() => IsEnable = false;
        
        public void LateTick()
        {
            if (IsEnable)
            {
                _cameraTransform.position = GetCameraPosition();
                _cameraTransform.LookAt(_playerTransform);
            }
        }

        private Vector3 GetCameraPosition()
        {
            return _playerTransform.position + (Quaternion.Euler(_cameraConfig._pitch,_cameraConfig._yaw,0) * Vector3.forward * _cameraConfig._distance);
        }
    }
}