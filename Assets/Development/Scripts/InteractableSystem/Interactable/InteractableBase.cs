using UnityEngine;

namespace Development.Scripts.InteractableSystem.Interactable
{
    public class InteractableBase : MonoBehaviour, IInteractable
    {
        [Header("Interactable Settings")] 
        [SerializeField] private bool multipleUse;
        [SerializeField] private float holdDuration;
        [SerializeField] private bool holdInteract;
        [SerializeField] private bool isInteractable;

        public bool MultipleUse => multipleUse;
        public float HoldDuration => holdDuration;
        public bool HoldInteract => holdInteract;
        public bool IsInteractable => isInteractable;

        public virtual void OnInteract()
        {
            Debug.Log("Interact With" + gameObject.name);
        }
    }
}