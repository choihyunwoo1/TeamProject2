using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAcquirePopupManager : MonoBehaviour
{
    public static ItemAcquirePopupManager Instance;

    [Header("References")]
    [SerializeField] private ItemAcquirePopupUI popupPrefab;
    [SerializeField] private RectTransform root;

    [Header("Layout")]
    [SerializeField] private int maxLine = 4;
    [SerializeField] private float lineSpacing = 60f;

    [Header("Timing")]
    [SerializeField] private float popInterval = 0.3f;

    private readonly List<ItemAcquirePopupUI> popups = new();
    private readonly Queue<string> messageQueue = new();

    private bool isProcessing;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // 어디서든 그냥 이거만 호출
    public void ShowMessage(string message)
    {
        messageQueue.Enqueue(message);

        if (!isProcessing)
            StartCoroutine(ProcessQueue());
    }

    private IEnumerator ProcessQueue()
    {
        isProcessing = true;

        while (messageQueue.Count > 0)
        {
            SpawnPopup(messageQueue.Dequeue());
            yield return new WaitForSeconds(popInterval);
        }

        isProcessing = false;
    }

    private void SpawnPopup(string message)
    {
        // 기존 팝업 위로 밀기
        for (int i = 0; i < popups.Count; i++)
        {
            RectTransform r = popups[i].GetComponent<RectTransform>();
            if (r != null)
                r.anchoredPosition += Vector2.up * lineSpacing;
        }

        // 최대 줄 초과 시 맨 아래 제거
        if (popups.Count >= maxLine)
        {
            Destroy(popups[0].gameObject);
            popups.RemoveAt(0);
        }

        ItemAcquirePopupUI popup = Instantiate(popupPrefab, root);
        popup.gameObject.SetActive(true);
        Vector2 startPos = new Vector2(300f, 0f);
        Vector2 targetPos = Vector2.zero;

        popup.Init(message, startPos, targetPos);
        popups.Add(popup);
    }

    public void Unregister(ItemAcquirePopupUI popup)
    {
        popups.Remove(popup);
    }
}
