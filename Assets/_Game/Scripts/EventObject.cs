using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventObject : MonoBehaviour
{
    [Serializable]
    public class CustomEvent
    {
        public string eventID;

        public float delay;

        public UnityEvent customEvent;
    }
    public List<CustomEvent> allEvents;

    public void SetupEventObject()
    {
        EventManager.Singelton.customEvent.AddListener(EventInvokeWithId);
    }
    public void EventInvokeWithId(string eventID)
    {
        for (int i = 0; i < allEvents.Count; i++)
        {
            if (allEvents[i].eventID == eventID)
            {
                EventManager.Singelton.StartCoroutine(DelayEvent(allEvents[i].customEvent, allEvents[i].delay));
            }
        }
    }
    private IEnumerator DelayEvent(UnityEvent eventToInvoke, float delay)
    {
        yield return new WaitForSeconds(delay);
        eventToInvoke.Invoke();
    }
}