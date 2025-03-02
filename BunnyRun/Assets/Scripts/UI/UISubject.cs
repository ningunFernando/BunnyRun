using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISubject : MonoBehaviour
{
    private List <UIObserver> uiObservers = new List<UIObserver>();
   public void AddObserver(UIObserver observer)
    {
        uiObservers.Add(observer);
    }
    public void RemoveObserver(UIObserver observer)
    {
        uiObservers.Remove(observer);
    }
    protected void NotifyObservers(UIEnum action)
    {
        uiObservers.ForEach(observer =>
        {
            observer.OnNotify(action);
        });
    }

}
