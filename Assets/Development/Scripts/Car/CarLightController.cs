using Development.Scripts.Audio.Core;
using UnityEngine;

namespace Development.Scripts.Car
{
    public class CarLightController : MonoBehaviour
    {
        [Header("Main Light (Normal + Alarm)")]
        [SerializeField] private Light mainLight;
        [SerializeField] private float changeAlarmColorTime = 0.5f;
        [SerializeField] private Color alarmColor1 = Color.red;
        [SerializeField] private Color alarmColor2 = Color.blue;
        [SerializeField] private Color normalColor = Color.white;

        [Header("Bat Signal")]
        [SerializeField] private GameObject batSignal;
        [SerializeField] private float rotationBatSignalSpeed = 40f;

        [Header("Environment Light")]
        [SerializeField] private Light environmentLight;
        private readonly float defaultEnvironmentIntensity = 1.5f;

        private float _alarmTimer;
        private bool _normalActive = false;
        private bool _alarmActive = false;
        private float _batSignalTimer;
        
        public bool IsInStealthMode => environmentLight.intensity == 0;

        private static CarLightController _instance;
        public static CarLightController Instance => _instance;

        private void Awake()
        {
            if (_instance == null) _instance = this;
        }

        private void Start()
        {
            environmentLight.intensity = defaultEnvironmentIntensity;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                if (_alarmActive)
                {
                    _normalActive = true;
                    _alarmActive = false;
                }
                else
                {
                    _normalActive = !_normalActive;
                }
                
                if (_normalActive)
                {
                    mainLight.enabled = true;
                    mainLight.color = normalColor;
                }
                else
                {
                    mainLight.enabled = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.C))
                ToggleStealthMode();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ToggleAlarm();
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                ToggleBatSignal();
            }

            HandleAlarm();
        }
        

        private void ToggleStealthMode()
        {
            environmentLight.intensity =
                environmentLight.intensity == 0 ? defaultEnvironmentIntensity : 0;
        }

        private void ToggleAlarm()
        {
            if (_normalActive)
            {
                _alarmActive = true;
                _normalActive = false;
            }
            else
            {
                _alarmActive = !_alarmActive;
            }
            
            if (_alarmActive)
            {
                AudioManager.Instance.PlaySequentialAudioTrack("Alert");
                mainLight.enabled = true;
                mainLight.color = alarmColor1;
            }
            else
            {
                mainLight.enabled = false;
            }
        }

        private void ToggleBatSignal()
        {
            batSignal.gameObject.SetActive(!batSignal.gameObject.activeSelf);
        }
        
        private void HandleAlarm()
        {
            if (!_alarmActive || !mainLight.enabled)
            {
                if(AudioManager.Instance.IsAudioTrackPlaying("Alert"))
                {
                    AudioManager.Instance.StopAudioTrack("Alert");
                }
                return;
            }

            _alarmTimer += Time.deltaTime;

            if (_alarmTimer >= changeAlarmColorTime)
            {
                mainLight.color = 
                    mainLight.color == alarmColor1 ? alarmColor2 : alarmColor1;

                _alarmTimer = 0;
            }
        }
    }
}
