using UnityEngine;
using TMPro;

namespace Choi
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance;

        [SerializeField] private GameObject dialogueUI;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text sentenceText;

        private string[] lines;
        private int index = 0;

        private void Awake()
        {
            Instance = this;
            dialogueUI.SetActive(false);
        }

        public void StartDialogue(string npcName, TextAsset json)
        {
            nameText.text = npcName;
            lines = JsonUtility.FromJson<DialogueData>(json.text).lines;
            index = 0;

            dialogueUI.SetActive(true);
            ShowLine();
        }

        public void Next()
        {
            index++;

            if (index >= lines.Length)
            {
                EndDialogue();
                return;
            }

            ShowLine();
        }

        private void ShowLine()
        {
            sentenceText.text = lines[index];
        }

        private void EndDialogue()
        {
            dialogueUI.SetActive(false);
        }
    }

    [System.Serializable]
    public class DialogueData
    {
        public string[] lines;
    }
}