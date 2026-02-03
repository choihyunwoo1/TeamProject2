using UnityEngine;

[CreateAssetMenu(menuName = "Tutorial/Tutorial Data")]
public class TutorialData : ScriptableObject
{
    [Header("ID (중복되면 안 됨)")]
    public string tutorialId;
    public TutorialStep[] steps;
}
