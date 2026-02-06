using System.Collections;
using UnityEngine;
using TMPro;

public class ItemAcquirePopupUI : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    public float slideDuration = 0.25f;
    public float lifeTime = 2.5f;

    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();

        if (text == null)
            text = GetComponentInChildren<TMP_Text>(true);

        if (rect == null || text == null)
            Debug.LogError("ItemAcquirePopupUI 초기화 실패", this);
    }

    public void Init(string message, Vector2 startPos, Vector2 targetPos)
    {
        if (!gameObject.activeInHierarchy)
        {
            Debug.LogError("Popup 오브젝트 비활성 상태");
            return;
        }

        text.text = message;
        rect.anchoredPosition = startPos;

        StartCoroutine(SlideIn(targetPos));
    }

    private IEnumerator SlideIn(Vector2 target)
    {
        float t = 0f;
        Vector2 start = rect.anchoredPosition;

        while (t < 1f)
        {
            t += Time.deltaTime / slideDuration;
            rect.anchoredPosition = Vector2.Lerp(start, target, t);
            yield return null;
        }

        rect.anchoredPosition = target;

        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (ItemAcquirePopupManager.Instance != null)
            ItemAcquirePopupManager.Instance.Unregister(this);
    }
}
