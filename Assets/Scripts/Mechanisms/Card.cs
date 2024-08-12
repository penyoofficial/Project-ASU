using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 卡牌骨架
/// 
/// <para>
/// 卡牌是游戏的核心，由玩家抽取、购买或获赠而得。
/// 游戏开始时（局外），玩家可免费抽取 10 张卡牌：3 张庇佑（选 1）和 7 张其他牌。
/// 之后每次进入对战节点可摸 3 张牌（选 1），或消耗点数重新摸牌。
/// 若一方自身生命值耗尽，则本局游戏宣告结束。
/// 玩家应当充分利用不同种类卡牌构建自己的阵营。
/// </para>
/// <list type="bullet">
///     <item>
///         <term>
///         号角
///         </term>
///         <description>
///         在场上占据实际位置，吸引敌方优先攻击自身而不是玩家或其他卡牌。
///         每次行动一般只对一个敌方目标生效。
///         </description>
///     </item>
///     <item>
///         <term>
///         庇佑
///         </term>
///         <description>
///         仅限开局抽卡时获得，使用后产生全局效果，并立刻销毁。
///         数值计算时单独享受一个乘区。
///         </description>
///     </item>
///     <item>
///         <term>
///         秘法
///         </term>
///         <description>
///         附着于场上已有的卡牌时生效，为指定目标提供持续或一次性的效果。
///         若指定目标退场，则立刻销毁。
///         </description>
///     </item>
///     <item>
///         <term>
///         诅咒
///         </term>
///         <description>
///         抽到时强制进入手牌堆，不占用选牌机会和槽位；无法被主动使用，无法被分解。
///         当满足隐藏条件时，就会触发携带的负面效果，并立刻销毁。
///         未触发前不能得知具体的效果。
///         </description>
///     </item>
/// </list>
/// <para>
/// 卡牌在被抽取时确定稀有度和家族。
/// 稀有度可分为山铜、秘银、钯金，为卡牌提供不同级别的增强。
/// 不同家族的卡牌之间互相作用的效果不一。
/// </para>
/// <para>
/// 卡牌可被主动分解为点数，这些点数可用于局外购买卡牌，或局内重新摸牌消耗。
/// </para>
/// </summary>
public class Card : MonoBehaviour
{
    /// <summary>
    /// 所属玩家
    /// </summary>
    public Player belongingTo;

    /// <summary>
    /// 行动所消耗的点数/分解所获得的点数
    /// </summary>
    public int pt = 5;
    public Text ptView;

    /// <summary>
    /// 攻击力
    /// </summary>
    public int atk = 0;
    public Text atkView;

    /// <summary>
    /// 生命值
    /// </summary>
    public int life = 0;
    public Text lifeView;

    /// <summary>
    /// 名称
    /// </summary>
    public string nickname;
    public Text nicknameView;

    /// <summary>
    /// 文字描述
    /// </summary>
    public string description;
    public Text descriptionView;

    public CardRare rare;
    public CardClan clan;
    public Text rareAndClanView;

    protected void Start()
    {
        nicknameView.text = nickname;
        descriptionView.text = description;

        rareAndClanView.text = Enum.GetName(typeof(CardClan), clan);
        rareAndClanView.color = rare switch
        {
            CardRare.COPPER => new Color(0.69f, 0.45f, 0.33f),
            CardRare.SILVER => new Color(0.75f, 0.75f, 0.75f),
            CardRare.GOLD => new Color(1f, 0.84f, 0f),
            _ => Color.white
        };
    }

    protected void Update()
    {
        ptView.text = $"{(pt > 0 ? pt : "/")}";
        atkView.text = $"{(atk > 0 ? atk : "/")}";
        lifeView.text = $"{(life > 0 ? life : "/")}";
    }

    protected int FinallyBe(int value, VType vType, Card another = null)
    {
        float vPeriod1 = value * (1 + (int)rare / 100f);

        float vPeroid2 = vPeriod1;
        if (vType.Equals(VType.ATK) && another != null)
        {
            if (
                clan.Equals(CardClan.DRAGON) && another.clan.Equals(CardClan.HUMAN)
            ||
                clan.Equals(CardClan.HUMAN) && another.clan.Equals(CardClan.PLANT)
            ||
                clan.Equals(CardClan.PLANT) && another.clan.Equals(CardClan.DRAGON)
            ||
                clan.Equals(CardClan.DEMON) && (another.clan.Equals(CardClan.DRAGON) || another.clan.Equals(CardClan.HUMAN))
            )
            {
                vPeroid2 *= 1.2f;
            }
            else if (clan.Equals(another.clan))
            {
                vPeroid2 *= 0.8f;
            }
        }

        float vPeriod3 = vPeroid2;
        if (belongingTo.effects.TryGetValue(vType, out var f))
        {
            vPeriod3 = f.Invoke(vPeriod3);
        }

        float vPeriod4 = vPeriod3;
        if (belongingTo.g_effects.TryGetValue(vType, out f))
        {
            vPeriod4 = f.Invoke(vPeriod4);
        }

        return (int)vPeriod4;
    }

    public void InteractWith(Card another) {}
}

/// <summary>
/// 值类型
/// </summary>
public enum VType
{
    PT,
    ATK,
    LIFE,
}

/// <summary>
/// 卡牌稀有度
/// </summary>
public enum CardRare
{
    COPPER = 0,
    SILVER = 4,
    GOLD = 10,
}

/// <summary>
/// 卡牌家族
/// </summary>
public enum CardClan
{
    /// <summary>
    /// 德拉古（龙）。克人。
    /// </summary>
    DRAGON,

    /// <summary>
    /// 人。克普兰特。
    /// </summary>
    HUMAN,

    /// <summary>
    /// 普兰特（植物）。克德拉古。
    /// </summary>
    PLANT,

    /// <summary>
    /// 妖怪。克德拉古、人。
    /// </summary>
    DEMON,
}
