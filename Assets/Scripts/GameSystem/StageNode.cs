using UnityEngine;

public enum StageName
{
    none = 0,
    world1
}
public class StageNode : MonoBehaviour
{
    public StageName stageId;
    public Transform pointerAnchor;
    [TextArea] public string stageTitle;
    [TextArea] public string stageDesc;
}