using System;

/// <summary>
/// 调试器。仅在开发环境中使用！
/// </summary>
public class Debugger
{
    static readonly bool indev = true;

    public static void Run(Action action)
    {
        if (indev)
        {
            action.Invoke();
        }
    }
}
