using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 地图生成器
/// </summary>
public class MapGenerator : MonoBehaviour
{
    public MapSO mapConfig;
    public EventNode eventNodePrefab;
    public LineRenderer linePrefab;
    public List<EventNodeSO> eventNodeSOPrefabs = new();

    Vector2 initializedPosition;
    float screenWidth;
    float screenHeight;
    float rowGap;
    float columnGap;

    protected void Start()
    {
        screenHeight = Camera.main.orthographicSize * 2;
        screenWidth = screenHeight * Camera.main.aspect;
        rowGap = 0.2f * screenHeight;
        columnGap = 0.3f * screenWidth;

        initializedPosition = transform.position;
        Run();
    }

    protected void Update()
    {
        if (Input.GetKey(KeyCode.A) && transform.position.x < initializedPosition.x)
        {
            transform.position = new(transform.position.x + 0.03f, transform.position.y);
        }
        else if (Input.GetKey(KeyCode.D) && transform.position.x > initializedPosition.x - columnGap * (mapConfig.mapColumns.Count + 1) + screenWidth)
        {
            transform.position = new(transform.position.x - 0.03f, transform.position.y);
        }
    }

    readonly List<EventNode> nodes = new();
    readonly List<LineRenderer> lines = new();

    public void Run()
    {
        List<EventNode> previousColumn;
        List<EventNode> currentColumn = new();

        for (int columnIndex = 0; columnIndex < mapConfig.mapColumns.Count; columnIndex++)
        {
            previousColumn = currentColumn;
            currentColumn = new();

            MapColumn column = mapConfig.mapColumns[columnIndex];

            int nodeAmount = Random.Range(column.minNodeNum, column.maxNodeNum + 1);
            for (int rowIndex = 0; rowIndex < nodeAmount; rowIndex++)
            {
                EventNode n = Instantiate(eventNodePrefab, new(columnGap * columnIndex, (screenHeight - rowGap * nodeAmount) / 2 + rowGap * rowIndex), Quaternion.identity, transform);

                n.transform.position = new(n.transform.position.x - 4.5f, n.transform.position.y - 4);
                n.Inject(columnIndex, rowIndex, DecideEventNodeSO(column));

                currentColumn.Add(n);
                nodes.Add(n);
            }

            if (columnIndex > 0)
            {
                HashSet<EventNode> tos = new();
                for (int pointer = 0; pointer < previousColumn.Count; pointer++)
                {
                    EventNode from = previousColumn[pointer];
                    EventNode to = currentColumn[Random.Range(0, currentColumn.Count)];
                    tos.Add(to);
                    Link(from, to);

                    if (pointer == previousColumn.Count - 1)
                    {
                        foreach (EventNode toInForce in currentColumn.Except(tos))
                        {
                            Link(from, toInForce);
                        }
                    }
                }
            }
        }

        nodes[0].eventNodeState = EventNodeState.UNLOCKED;
    }

    void Link(EventNode n1, EventNode n2)
    {
        LineRenderer line = Instantiate(linePrefab, transform);
        line.SetPosition(0, n1.transform.position);
        line.SetPosition(1, n2.transform.position);
        lines.Add(line);
    }

    EventNodeSO DecideEventNodeSO(MapColumn config)
    {
        string[] types = config.eventNodeTypes.ToString().Split(",");
        string selectedType = types[Random.Range(0, types.Length)];

        EventNodeType type = (EventNodeType)System.Enum.Parse(typeof(EventNodeType), selectedType);

        return eventNodeSOPrefabs.Where((e) => e.eventNodeType == type).ToArray()[0];
    }

    [ContextMenu("调试：重新生成地图")]
    public void ReRun()
    {
        foreach (EventNode n in nodes)
        {
            Destroy(n.gameObject);
        }
        nodes.Clear();
        foreach (LineRenderer l in lines)
        {
            Destroy(l.gameObject);
        }
        lines.Clear();

        Run();
    }
}