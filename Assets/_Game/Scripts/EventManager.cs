using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager Singelton;
    public CustomEvent customEvent;
    void Awake()
    {
        Singelton = this;
    }

    [System.Serializable]
    public class CustomEvent : UnityEvent<string> { }


    private void Start()
    {
        FindAndSetupEventObject();
    }
    private void FindAndSetupEventObject()
    {
        customEvent.RemoveAllListeners();
        EventObject[] components = FindObjectsByType<EventObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        for (int i = 0; i < components.Length; i++)
        {
            components[i].SetupEventObject();
        }
    }

    public void InvokeEvent(string eventID)
    {
        FindAndSetupEventObject();
        Debug.Log("<color=green>Event with id = " + eventID + " invoked!</color>");
        customEvent.Invoke(eventID);
    }
}