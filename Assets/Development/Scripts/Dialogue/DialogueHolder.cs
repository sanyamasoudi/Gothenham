using UnityEngine;

namespace Development.Scripts.Dialogue
{
    [CreateAssetMenu(fileName = "DialogueHolder", menuName = "DialogueManger/Dialogue Holder")]
    public class DialogueHolder : ScriptableObject
    {
        [SerializeField] private Dialogue[] dialogues;

        public Dialogue[] GetDialogues() => dialogues;
    }
}