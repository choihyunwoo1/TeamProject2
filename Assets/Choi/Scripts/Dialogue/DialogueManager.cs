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

        private DialogueLine[] lines;
        private int currentIndex = 0;

        private void Awake()
        {
            Instance = this;
            dialogueUI.SetActive(false);
        }
        private void Update()
        {
            if (!dialogueUI.activeSelf) return;

            if (Input.GetMouseButtonDown(0))
            {
                Next();
            }
        }

        public void StartDialogue(TextAsset json)
        {
            dialogueUI.SetActive(true);

            DialogueRoot root = JsonUtility.FromJson<DialogueRoot>(json.text);

            if (root.dialogue == null || root.dialogue.Length == 0)
            {
                Debug.LogWarning("Nothing in here");
                return;
            }

            lines = root.dialogue;

            // 첫 줄은 항상 id 1이라고 가정
            currentIndex = FindIndexById(1);

            ShowLine();
        }

        public void Next()
        {
            DialogueLine current = lines[currentIndex];

            if (current.next == null)
            {
                EndDialogue();
                return;
            }

            currentIndex = FindIndexById(current.next.Value);
            ShowLine();
        }

        private void ShowLine()
        {
            DialogueLine line = lines[currentIndex];

            nameText.text = line.character;
            sentenceText.text = line.text;
        }

        private int FindIndexById(int id)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].id == id) return i;
            }

            Debug.LogError("Can't find ID: " + id);
            return 0;
        }

        private void EndDialogue()
        {
            dialogueUI.SetActive(false);
        }
    }

    [System.Serializable]
    public class DialogueLine
    {
        public int id;
        public string character;
        public string text;
        public int? next;
    }

    [System.Serializable]
    public class DialogueRoot
    {
        public DialogueLine[] dialogue;
    }
}
