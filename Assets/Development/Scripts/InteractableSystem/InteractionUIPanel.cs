using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Development.Scripts.InteractableSystem
{
    public class InteractionUIPanel : MonoBehaviour
    {
        [SerializeField] private Image progressBar;
        [SerializeField] private TextMeshProUGUI tooltipText;

        public void SetTooltipText(string text)
        {
            tooltipText.text = text;
        }

        public void UpdateProgress(float progress)
        {
            progressBar.fillAmount = progress;
        }
        
        public void Reset()
        {
            progressBar.fillAmount = 0;
            SetTooltipText("");
        }
    }
}
