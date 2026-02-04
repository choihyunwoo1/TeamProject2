using UnityEngine;

public enum TutorialAnchor
{
    LeftTop,
    Top,
    RightTop,

    LeftCenter,
    Center,
    RightCenter,

    LeftBottom,
    Bottom,
    RightBottom
}

public enum TutorialButtonType
{
    Next,
    Exit,
    None
}

[System.Serializable]
public class TutorialStep
{
    [TextArea]
    public string description;

    [Header("Anchor")]
    public TutorialAnchor anchorType;

    [Header("Panel")]
    public Vector2 panelOffset;
    public Vector2 panelSize;

    [Header("Text")]
    public Vector2 textOffset;
    public Vector2 textSize;

    [Header("Button Type")]
    public TutorialButtonType buttonType;
}
