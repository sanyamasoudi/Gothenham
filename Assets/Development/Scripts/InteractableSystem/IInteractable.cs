using UnityEngine;

namespace Development.Scripts.InteractableSystem
{
    public interface IInteractable
    {
        public bool MultipleUse { get; }
        public float HoldDuration { get; }
        public bool HoldInteract { get; }
        public bool IsInteractable { get; }
        public void OnInteract();
    }
}