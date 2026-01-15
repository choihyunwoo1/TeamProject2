using UnityEngine;
using TMPro;

namespace Choi
{
    public class InteractionUI : MonoBehaviour
    {
        public static InteractionUI Instance;

        [SerializeField] private GameObject root;
        [SerializeField] private TMP_Text promptText;

        private void Awake()
        {
            Instance = this;
            Hide();
        }

        public void Show(string prompt)
        {
            root.SetActive(true);
            promptText.text = $"F: {prompt}";
        }

        public void Hide()
        {
            root.SetActive(false);
        }
    }
}