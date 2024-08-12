using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 事件节点（不是具体的事件场景！）
/// </summary>
public class EventNode : MonoBehaviour
{
    public int locaX, locaY;
    public EventNodeSO eventNodeSO;
    public EventNodeState eventNodeState;

    protected void Start()
    {
        Debugger.Run(() => Inject(0, 0, eventNodeSO));
    }

    protected void Update() {
        if (eventNodeState!=EventNodeState.UNLOCKED) {
            // GetComponent<SpriteRenderer>().
        }
    }

    protected void OnMouseDown()
    {
        switch (eventNodeState)
        {
            case EventNodeState.LOCKED:
            case EventNodeState.MISSED:
            case EventNodeState.VISITED:
                Debug.Log("该节点拒绝访问！");
                break;
            case EventNodeState.UNLOCKED:
                Debug.Log($"你即将进入“{eventNodeSO.eventNodeType}”！");
                // SceneManager.LoadScene(eventNodeSO.sceneToLoad);
                break;
        }
    }

    public void Inject(int locaX, int locaY, EventNodeSO data)
    {
        this.locaX = locaX;
        this.locaY = locaY;

        eventNodeSO = data;
        GetComponent<SpriteRenderer>().sprite = data.icon;
    }
}
