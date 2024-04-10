using System.Collections.Generic;
using UnityEngine;

public class MainThreadDispatcher : MonosingletonTemp<MainThreadDispatcher>
{
    private static readonly Queue<System.Action> ExecuteOnMainThreadQueue = new Queue<System.Action>();

    public static void ExecuteInUpdate(System.Action action)
    {
        lock (ExecuteOnMainThreadQueue)
        {
            ExecuteOnMainThreadQueue.Enqueue(action);
        }
    }

    private void Update()
    {
        while (ExecuteOnMainThreadQueue.Count > 0)
        {
            System.Action action = null;
            lock (ExecuteOnMainThreadQueue)
            {
                if (ExecuteOnMainThreadQueue.Count > 0)
                {
                    action = ExecuteOnMainThreadQueue.Dequeue();
                }
            }

            action?.Invoke();
        }
    }
}

// MainThreadDispatcher.ExecuteInUpdate(() =>
// {
//     // 这里的代码会在主线程的Update方法中执行
//     // 例如，可以安全地更新Unity的UI元素
// });
