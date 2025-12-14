using Development.Scripts.Core;
using Development.Scripts.Road;
using UnityEngine;

namespace Development.Scripts.Car
{
    public class CarMovement : MonoBehaviour
    {
        [SerializeField] private GameObject roadPrefab;
        [SerializeField] private float moveForce;
        [SerializeField] private AnimationCurve curve;
        [SerializeField][Range(0f,1f)] private float brakeForce;

        private Rigidbody _rigidbody;
        private bool _isMoving;
        private float _moveTimer;
        private float _t;
        private float _easeSpeed;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            GetInput();
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("CheckPoint"))
            {
                var position = other.transform.position;
                position.z += 30f;
                Instantiate(roadPrefab, position, transform.rotation);
                other.gameObject.GetComponent<RoadCheckPoint>().OnDestroyRoad();
            }
        }

        private void GetInput()
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                _isMoving = true;
            }
            else
            {
                _isMoving = false;
            }
        }

        private void Move()
        {
            if (!GameManager.isInTheCar) return;
            if (_isMoving)
            {
                _moveTimer += Time.deltaTime;
                _t = Mathf.Clamp01(_moveTimer);
                _easeSpeed = curve.Evaluate(_t) * moveForce;
                if (CarLightController.Instance.IsInStealthMode)
                {
                    _easeSpeed *= brakeForce;
                }
                _rigidbody.linearVelocity = Vector3.forward * _easeSpeed;
            }
            else
            {
                _rigidbody.linearVelocity = Vector3.zero;
            }
        }
    }
}