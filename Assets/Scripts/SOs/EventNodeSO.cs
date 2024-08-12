using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// 事件节点数据
/// </summary>
[CreateAssetMenu(fileName = "EventNodeSO", menuName = "配置/事件节点")]
public class EventNodeSO : ScriptableObject
{
    public Sprite icon;
    public EventNodeType eventNodeType;
    public AssetReference sceneToLoad;
}

/// <summary>
/// 事件节点类型
/// </summary>
[Flags]
public enum EventNodeType
{
    /// <summary>
    /// 最常见的节点类型。什么都有可能发生。
    /// </summary>
    UNKNOWN = 1,

    /// <summary>
    /// 休息处。你可以在这里抽取一张卡牌或者回复一些生命。
    /// </summary>
    REST = 2,

    /// <summary>
    /// 商店。你可以付出你的点数购买物资。
    /// </summary>
    STORE = 4,

    /// <summary>
    /// 对战。
    /// </summary>
    FIGHT = 8,

    /// <summary>
    /// 重要对战。
    /// </summary>
    CRUCIAL_FIGHT = 16,
}

/// <summary>
/// 事件节点状态
/// </summary>
public enum EventNodeState
{
    /// <summary>
    /// 锁定
    /// </summary>
    LOCKED,

    /// <summary>
    /// 解锁
    /// </summary>
    UNLOCKED,

    /// <summary>
    /// 已进入
    /// </summary>
    VISITED,

    /// <summary>
    /// 已错失
    /// </summary>
    MISSED,
}
