/// <summary>
/// 号角：狗
/// 
/// <para>
/// <i>
/// 从路边随手拣来的小东西，笃定你是它一生的主人。
/// </i>
/// </para>
/// </summary>
public class BugleDog : Card
{
    public new void InteractWith(Card another)
    {
        another.life--;
    }
}
