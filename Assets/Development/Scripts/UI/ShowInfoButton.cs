using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Development.Scripts.UI
{
    public class ShowInfoButton : MonoBehaviour
    {
        [SerializeField] private GameObject showButton;
        [SerializeField] private List<GameObject> keyGameObject;
        [SerializeField] private float initialTime = 0.5f;
        [SerializeField] private float nextShowTime = 0.3f;
        [SerializeField] [Range(0, 100)] private float moveXPresent = 600f;
        
        private float _startX;

        private void Start()
        {
            _startX = keyGameObject[0].transform.position.x;
        }

        public void OnShow()
        {
            showButton.SetActive(false);
            var time = initialTime;

            for (int i = 0; i < keyGameObject.Count; i++)
            {
                time += i * nextShowTime;
                keyGameObject[i].SetActive(true);
                keyGameObject[i].transform.DOMoveX(Screen.width * moveXPresent / 100, time);
            }
        }

        public void OnClose()
        {
            var time = initialTime;
            for (int i = 0; i < keyGameObject.Count; i++)
            {
                time += i * nextShowTime;
                var i1 = i;
                keyGameObject[i].transform.DOMoveX(_startX, time);
            }

            showButton.SetActive(true);
        }
    }
}