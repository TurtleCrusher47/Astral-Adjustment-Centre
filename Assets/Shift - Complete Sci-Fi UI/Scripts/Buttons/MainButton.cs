using UnityEngine;
using TMPro;

namespace Michsky.UI.Shift
{
    [ExecuteInEditMode]
    public class MainButton : MonoBehaviour
    {
        [Header("Settings")]
        public string buttonText = "My Title";
        public bool useCustomText = false;

        [Header("Resources")]
        private bool changeText = false;
        public TextMeshProUGUI normalText;
        public TextMeshProUGUI highlightedText;
        public TextMeshProUGUI pressedText;

        void OnEnable()
        {
            if (!useCustomText && !changeText)
            {
                if (normalText != null)
                {
                    normalText.text = buttonText;
                }

                if (highlightedText != null)
                {
                    highlightedText.text = buttonText;
                }

                if (pressedText != null)
                {
                    pressedText.text = buttonText;
                }
            }
        }

        void OnDisable()
        {
            changeText = false;
        }

        public void ChangeText(string newText)
        {
            changeText = true;

            if (normalText != null)
            {
                normalText.text = newText;
            }

            if (highlightedText != null)
            {
                highlightedText.text = newText;
            }

            if (pressedText != null)
            {
                pressedText.text = newText;
            }
        }
    }
}