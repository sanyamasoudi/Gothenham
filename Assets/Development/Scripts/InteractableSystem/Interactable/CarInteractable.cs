using System.Collections;
using Development.Scripts.Audio.Core;
using Development.Scripts.Core;
using Development.Scripts.Dialogue;
using Unity.Cinemachine;
using UnityEngine;

namespace Development.Scripts.InteractableSystem.Interactable
{
    public class CarInteractable : InteractableBase
    {
        [SerializeField] private CinemachineCamera cinemachineCamera;
        [SerializeField] private CinemachineOrbitalFollow cinemachineCameraFollow;
        [SerializeField] private float cameraFollowRadius = 8f;
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject interactSign;

        public override void OnInteract()
        {
            base.OnInteract();
            GameManager.VisitCar();
            if (GameManager.carInteractCount == 1)
            {
                DialogueManager.Instance.StartDialogue();
            }

            if (GameManager.isKeyFound && GameManager.carInteractCount > 1)
            {
                player.SetActive(false);
                interactSign.SetActive(false);
                cinemachineCamera.Follow = this.gameObject.transform;
                cinemachineCamera.LookAt = this.gameObject.transform;
                cinemachineCameraFollow.OrbitStyle = CinemachineOrbitalFollow.OrbitStyles.Sphere;
                cinemachineCameraFollow.Radius = cameraFollowRadius;
                DialogueManager.Instance.StartDialogue();
                StartCoroutine(PlayCarAudio());
            }
        }

        private IEnumerator PlayCarAudio()
        {
            yield return new WaitForSeconds(5f);
            GameManager.GetInToTheCar();
            AudioManager.Instance.StopAudioTrack("Main");
            AudioManager.Instance.PlaySequentialAudioTrack("Radio");
        }
    }
}