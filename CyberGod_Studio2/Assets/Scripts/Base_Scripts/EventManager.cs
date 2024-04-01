using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic; // Add this line


public class GameEventArgs : EventArgs
{
    public int IntValue { get; set; }
    public float FloatValue { get; set; }
    public string StringValue { get; set; }
    public Vector3 Vector3Value { get; set; }
    public List<Vector3> Vector3ListValue { get; set; }
    
    public Queue<Vector3> Vector3QueueValue { get; set; }
    
}

public class EventManager : MonosingletonTemp<EventManager>
{
    // Use a dictionary to store events
    private Dictionary<string, Action<GameEventArgs>> gameEvents = new Dictionary<string, Action<GameEventArgs>>(); //事件字典, 事件名字，事件的行为
    //事件的行为是一个委托，委托是一个类，它可以存储一个或多个方法的引用，委托的实例可以存储对任何方法的引用，该方法与委托的签名和返回类型相同。

    // Method to add an event
    public void AddEvent(string eventName, Action<GameEventArgs> eventAction)//添加事件,事件名字，事件的行为
    {
        if (!gameEvents.ContainsKey(eventName))//如果事件字典中不包含这个事件
        {
            gameEvents.Add(eventName, eventAction);
            Debug.Log("Event: " + eventName + " added");
        }
        //如果事件字典中包含这个事件，就不添加并且Debug.Log
        else
        {
            Debug.Log("Event: " + eventName + " already exists");
        }
    }

    // Method to remove an event
    public void RemoveEvent(string eventName)
    {
        if (gameEvents.ContainsKey(eventName))
        {
            gameEvents.Remove(eventName);
        }
    }

    // Method to trigger an event
    public void TriggerEvent(string eventName, GameEventArgs eventArgs = null)
    {
        Action<GameEventArgs> thisEvent = null; 
        if (gameEvents.TryGetValue(eventName, out thisEvent))
        {
            thisEvent?.Invoke(eventArgs);
        }
    }
}



//先订阅事件，再发布

//订阅：
// void OnMove(GameEventArgs args) {
//     Debug.Log("Movement detected");
//     // 你可以使用 args 中的数据来执行一些操作
// }
//
// void Start() {
//     EventManager.Instance.AddEvent("OnMove", OnMove);
// }

//发布：
// void TriggerMovementEvent() {
//     GameEventArgs args = new GameEventArgs {
//         IntValue = 1,        // 设置整型值
//         FloatValue = 2.5f,   // 设置浮点值
//         StringValue = "Move",// 设置字符串值
//         Vector3Value = new Vector3(1, 0, 0) // 设置 Vector3 值
//         // 你也可以设置其他 GameEventArgs 属性
//     };
//
//     EventManager.Instance.TriggerEvent("OnMove", args);
// }

    