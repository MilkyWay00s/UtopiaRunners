using UnityEngine;

public enum StageName
{
    none = 0,
    Stage1,
    Stage2,
    Stage3,
    Stage4,
    Stage5,
    Stage6
}
public class StageNode : MonoBehaviour
{
    public StageName stageId;
    public Transform pointerAnchor;
    [TextArea] public string stageTitle;
    [TextArea] public string stageDesc;
}