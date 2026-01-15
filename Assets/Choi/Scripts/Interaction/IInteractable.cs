    using UnityEngine;

namespace Choi
{
    public interface IInteractable
    {
        // 플레이어가 봤을 때 표시되는 문구
        string GetInteractPrompt();

        // 실제 상호작용 실행
        void Interact(GameObject player);
    }
}