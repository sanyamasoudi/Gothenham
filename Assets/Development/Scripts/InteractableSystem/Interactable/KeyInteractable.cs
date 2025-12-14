using Development.Scripts.Core;
using Development.Scripts.Dialogue;
using UnityEngine;

namespace Development.Scripts.InteractableSystem.Interactable
{
    public class KeyInteractable : InteractableBase
    {
        public override void OnInteract()
        {
            base.OnInteract();
            if (GameManager.carInteractCount != 0)
            {
                GameManager.FoundKey();
                if (GameManager.isKeyFound)
                {
                    DialogueManager.Instance.StartDialogue();
                    Destroy(gameObject);
                }
            }
        }
    }
}