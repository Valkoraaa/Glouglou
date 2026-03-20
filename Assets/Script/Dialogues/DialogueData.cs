using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Dialogue/Dialogue")]
public class DialogueData : ScriptableObject
{
    public string speakerName;
    public List<DialogueLine> lines;
}

[System.Serializable]
public class DialogueLine
{
    [TextArea(2, 5)]
    public string text;
}