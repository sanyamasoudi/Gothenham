using System;
using Development.Scripts.Core;
using UnityEngine;

namespace Development.Scripts.Car
{
    public class CarMovementController : MonoBehaviour
    {
        [Header("Force Settings")] 
        [SerializeField] private float motorForce = 100f;
        [SerializeField] private float brakeForce = 1000f;
        [SerializeField] private float maxSteerAngle = 30f;

        [Header("Wheel Settings")] 
        [SerializeField] private WheelCollider topRightWheelCollider;
        [SerializeField] private WheelCollider bottomRightWheelCollider;
        [SerializeField] private WheelCollider topLeftWheelCollider;
        [SerializeField] private WheelCollider bottomLeftWheelCollider;

        [SerializeField] private Transform topRightWheelTransform;
        [SerializeField] private Transform bottomRightWheelTransform;
        [SerializeField] private Transform topLeftWheelTransform;
        [SerializeField] private Transform bottomLeftWheelTransform;

        private float _horizontalInput;
        private float _verticalInput;
        private float _currentBrakeForce;
        private float _currentSteerAngle;
        private bool _isBraking = false;

        private void Update()
        {
            if(!GameManager.isInTheCar) return;
            GetInput();
            HandleMotor();
            ApplySteering();
            UpdateWheels();
        }

        private void GetInput()
        {
            _horizontalInput = Input.GetAxis("Horizontal");
            _verticalInput = Input.GetAxis("Vertical");
            _isBraking = Input.GetKey(KeyCode.Q);
        }

        private void UpdateWheels()
        {
            UpdateSingleWheel(topRightWheelCollider,topRightWheelTransform);
            UpdateSingleWheel(bottomRightWheelCollider,bottomRightWheelTransform);
            UpdateSingleWheel(bottomLeftWheelCollider,bottomLeftWheelTransform);
            UpdateSingleWheel(topLeftWheelCollider,topLeftWheelTransform);
        }

        private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
        {
            if(_horizontalInput != 0 || _verticalInput != 0)
            {
                Vector3 position;
                Quaternion rotation;
                wheelCollider.GetWorldPose(out position, out rotation);
                wheelTransform.position = position;
                wheelTransform.rotation = rotation;
            }
        }

        private void ApplySteering()
        {
            _currentSteerAngle = maxSteerAngle * _horizontalInput;
            topLeftWheelCollider.steerAngle = _currentSteerAngle;
            topRightWheelCollider.steerAngle = _currentSteerAngle;
        }

        private void HandleMotor()
        {
            topLeftWheelCollider.motorTorque = _verticalInput * motorForce;
            topRightWheelCollider.motorTorque = _verticalInput * motorForce;

            _currentBrakeForce = _isBraking ? brakeForce : 0;
            ApplyBraking();
        }

        private void ApplyBraking()
        {
            topRightWheelCollider.brakeTorque = _currentBrakeForce;
            topLeftWheelCollider.brakeTorque = _currentBrakeForce;
            bottomRightWheelCollider.brakeTorque = _currentBrakeForce;
            bottomLeftWheelCollider.brakeTorque = _currentBrakeForce;
        }
        
    }
}