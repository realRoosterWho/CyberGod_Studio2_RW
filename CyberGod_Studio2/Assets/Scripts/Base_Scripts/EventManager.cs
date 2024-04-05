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

public class EventManager : MonoBehaviour
{
    private Dictionary<string, Action<GameEventArgs>> eventDictionary;

    private static EventManager eventManager;

    public static EventManager Instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init();
                }
            }

            return eventManager;
        }
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, Action<GameEventArgs>>();
        }
    }

    public void AddEvent(string eventName, Action<GameEventArgs> listener)
    {
        Action<GameEventArgs> thisEvent;
        if (eventManager.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            //Add more event to the existing one
            thisEvent += listener;

            //Update the Dictionary
            eventManager.eventDictionary[eventName] = thisEvent;
        }
        else
        {
            //Add event to the Dictionary for the first time
            thisEvent += listener;
            eventManager.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public void RemoveEvent(string eventName, Action<GameEventArgs> listener)
    {
        if (eventManager == null) return;
        Action<GameEventArgs> thisEvent;
        if (eventManager.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            //Remove event from the existing one
            thisEvent -= listener;

            //Update the Dictionary
            eventManager.eventDictionary[eventName] = thisEvent;
        }
    }

    public void TriggerEvent(string eventName, GameEventArgs eventArgs)
    {
        Action<GameEventArgs> thisEvent = null;
        if (eventManager.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(eventArgs);
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

    