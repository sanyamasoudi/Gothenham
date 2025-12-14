using UnityEngine;

namespace Development.Scripts.InteractableSystem
{
    [CreateAssetMenu(fileName = "InteractionInputData", menuName = "InputData/InteractionInputData")]
    public class InteractionInputData : ScriptableObject
    {
        [SerializeField] private KeyCode interactionKey = KeyCode.E;

        private bool interactedClicked;
        private bool interactedReleased;

        public bool InteractedClicked
        {
            get => interactedClicked;
            set => interactedClicked = value;
        }

        public bool InteractedReleased
        {
            get => interactedReleased;
            set => interactedReleased = value;
        }

        public KeyCode InteractionKey
        {
            get => interactionKey;
        }

        public void Reset()
        {
            interactedClicked = false;
            interactedReleased = false;
        }
    }
}