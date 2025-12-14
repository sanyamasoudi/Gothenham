using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Development.Scripts.Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private DialogueHolder dialogueHolders;
        [SerializeField] private float dialogueFadeTime = 2.5f;

        private readonly Queue<Dialogue> dialoguesQueue = new Queue<Dialogue>();
        private static DialogueManager _instance;
        public static DialogueManager Instance => _instance;
        private bool _isOpen = false;
        private Coroutine _dialogueCoroutine;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                InitializeDialogues();
                InitializeDialoguePanel();
            }
        }

        private void Start()
        {
            StartDialogue();
        }

        public void StartDialogue()
        {
            if (dialoguesQueue.Count == 0) return;
            dialogueText.text = String.Empty;
            var dialogue = dialoguesQueue.Dequeue();
            if (_isOpen) CloseDialoguePanel();
            OpenDialoguePanel();
            if (_dialogueCoroutine != null)
            {
                StopCoroutine(_dialogueCoroutine);
            }
            _dialogueCoroutine = StartCoroutine(DisplayDialogue(dialogue));
        }

        public void OpenDialoguePanel()
        {
            _isOpen = true;
            FadeDialogue(1f);
        }

        public void CloseDialoguePanel()
        {
            _isOpen = false;
            FadeDialogue(0f);
        }

        private void FadeDialogue(float fadeEndValue)
        {
            for (int i = 0; i < dialoguePanel.transform.childCount; i++)
            {
                var child = dialoguePanel.transform.GetChild(i);
                var image = child.GetComponent<Image>();
                var text = child.GetComponent<TextMeshProUGUI>();
                if (image)
                {
                    image.DOFade(fadeEndValue, dialogueFadeTime);
                }

                if (text)
                {
                    text.DOFade(fadeEndValue, dialogueFadeTime).OnComplete(() => { text.text = ""; });
                }
            }
        }

        private void InitializeDialogues()
        {
            var dialogues = dialogueHolders.GetDialogues();
            foreach (var dialogue in dialogues)
            {
                dialoguesQueue.Enqueue(dialogue);
            }
        }

        private void InitializeDialoguePanel()
        {
            for (int i = 0; i < dialoguePanel.transform.childCount; i++)
            {
                var child = dialoguePanel.transform.GetChild(i);
                var image = child.GetComponent<Image>();
                if (image)
                {
                    image.DOFade(0f, 0f);
                }
            }
        }


        private IEnumerator DisplayDialogue(Dialogue dialogue)
        {
            yield return new WaitForSeconds(dialogueFadeTime);
            dialogueText.text = "";
            foreach (var sentence in dialogue.sentences)
            {
                foreach (var letter in sentence.ToCharArray())
                {
                    dialogueText.text += letter;
                    yield return new WaitForSeconds(dialogue.delayBetweenCharacters);
                }

                dialogueText.text += "\n";
                yield return new WaitForSeconds(dialogue.delayBetweenSentences);
            }
        }
    }
}