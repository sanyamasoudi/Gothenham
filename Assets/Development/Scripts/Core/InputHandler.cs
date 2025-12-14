using System;
using Development.Scripts.InteractableSystem;
using UnityEngine;

namespace Development.Scripts.Core
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private InteractionInputData interactionInputData;

        private void Start()
        {
            interactionInputData.Reset();
        }

        private void Update()
        {
            GetInteractionInputData();
        }

        private void GetInteractionInputData()
        {
            interactionInputData.InteractedClicked = Input.GetKeyDown(interactionInputData.InteractionKey);
            interactionInputData.InteractedReleased = Input.GetKeyUp(interactionInputData.InteractionKey);
        }
    }
}