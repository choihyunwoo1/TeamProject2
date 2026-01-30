using UnityEngine;

[System.Serializable]
public class TutorialStep
{
    [TextArea]
    public string description;

    public Vector2 anchoredPosition;
    public Vector2 size;

    public TutorialClickType clickType;
}

public enum TutorialClickType
{
    NextButton,     // Next 버튼을 눌러야 진행
    ExitButton,     // Exit 버튼을 눌러 종료
    
}
