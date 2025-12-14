using System;
using Development.Scripts.Audio.Core;
using UnityEngine;

namespace Development.Scripts.Character
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Animator))]
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float runSpeedMultiplier;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private Transform cameraTransform;

        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int IsRunning = Animator.StringToHash("IsRunning");

        private Rigidbody _rigidbody;
        private Animator _animator;
        private float _horizontalInput;
        private float _verticalInput;
        private float _angle;
        private float _speed;
        private Vector3 _direction;
        private Vector3 _moveDirection;
        private bool _isRunning;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            GetInputs();
            HandleAnimation();
            HandleAudio();
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void GetInputs()
        {
            _horizontalInput = Input.GetAxisRaw("Horizontal");
            _verticalInput = Input.GetAxisRaw("Vertical");
            _direction = new Vector3(_horizontalInput, 0, _verticalInput).normalized;
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                _isRunning = true;
            }
            else
            {
                _isRunning = false;
            }
        }

        private void HandleAnimation()
        {
            _animator.SetFloat(Speed, _direction.magnitude > 0.1f ? 1f : 0f);
            _animator.SetBool(IsRunning, _isRunning);
        }

        private void HandleAudio()
        {
            if (_direction.magnitude > 0.1f)
            {
                if (_isRunning)
                {
                    if (AudioManager.Instance.IsAudioTrackPlaying("Running")) return;
                    StopWalkingAudio();
                    AudioManager.Instance.PlaySequentialAudioTrack("Running");
                }
                else
                {
                    if (AudioManager.Instance.IsAudioTrackPlaying("Walking")) return;
                    StopRunningAudio();
                    AudioManager.Instance.PlaySequentialAudioTrack("Walking");
                }
            }
            else
            {
                StopRunningAudio();
                StopWalkingAudio();
            }
        }

        private static void StopRunningAudio()
        {
            if (AudioManager.Instance.IsAudioTrackPlaying("Running"))
            {
                AudioManager.Instance.StopAudioTrack("Running");
            }
        }

        private static void StopWalkingAudio()
        {
            if (AudioManager.Instance.IsAudioTrackPlaying("Walking"))
            {
                AudioManager.Instance.StopAudioTrack("Walking");
            }
        }


        private void Move()
        {
            if (_direction.magnitude > 0.1f)
            {
                _angle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
                _moveDirection = (Quaternion.Euler(0, _angle, 0f) * Vector3.forward).normalized;

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_moveDirection),
                    rotationSpeed * Time.fixedDeltaTime);

                _speed = _isRunning ? moveSpeed * runSpeedMultiplier : moveSpeed;
                _rigidbody.MovePosition(transform.position + _moveDirection * (_speed * Time.fixedDeltaTime));
            }
        }
    }
}