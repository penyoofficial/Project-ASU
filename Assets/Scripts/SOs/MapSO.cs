using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地图数据
/// </summary>
[CreateAssetMenu(fileName = "MapSO", menuName = "配置/地图")]
public class MapSO : ScriptableObject
{
    public List<MapColumn> mapColumns;
}

/// <summary>
/// 地图每列
/// </summary>
[Serializable]
public class MapColumn
{
    public int minNodeNum, maxNodeNum;
    public EventNodeType eventNodeTypes;
}
