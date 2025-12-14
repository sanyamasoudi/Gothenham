using Development.Scripts.InteractableSystem.Interactable;
using UnityEngine;

namespace Development.Scripts.InteractableSystem
{
    [CreateAssetMenu(fileName = "InteractionData", menuName = "InteractionSystem/InteractionData")]
    public class InteractionData : ScriptableObject
    {
        private InteractableBase interactable;

        public InteractableBase Interactable
        {
            get => interactable;
            set => interactable = value;
        }

        public void Interact()
        {
            interactable.OnInteract();
            ResetData();
        }

        public bool IsSameInteractable(InteractableBase newinteractable) => interactable == newinteractable;

        public void ResetData()
        {
            interactable = null;
        }
        
        public bool IsEmpty() => interactable == null;
}
}