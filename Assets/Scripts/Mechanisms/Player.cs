using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家
/// 
/// <para>
/// 玩家是游戏活动的主体，也是承载卡牌的容器。
/// </para>
/// </summary>
public class Player : MonoBehaviour
{
    /// <summary>
    /// 手牌堆
    /// </summary>
    public List<Card> cards = new();

    /// <summary>
    /// 对手
    /// </summary>
    public Player rival;

    /// <summary>
    /// 全局效果
    /// </summary>
    public Dictionary<VType, Func<float, float>> g_effects;
    /// <summary>
    /// 局内效果
    /// </summary>
    public Dictionary<VType, Func<float, float>> effects;
}
