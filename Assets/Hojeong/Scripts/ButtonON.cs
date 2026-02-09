using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Choi;

namespace HJ
{
    public class ButtonON : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        public GameObject restartBG;

        public void OnPointerEnter(PointerEventData eventData)
        {
            restartBG.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            restartBG.SetActive(false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            SoundManager.Instance.Play("Button");

            PortalManager.LastPortalID = null;

            SceneManager.LoadScene("Village");
        }

    }
}