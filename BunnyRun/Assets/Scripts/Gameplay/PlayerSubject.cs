using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSubject : MonoBehaviour
{
    private List<PlayerObserver> playerObservers = new List<PlayerObserver>();
    public void AddObserver(PlayerObserver observer)
    {
        playerObservers.Add(observer);
    }
    public void RemoveObserver(PlayerObserver observer)
    {
        playerObservers.Remove(observer);
    }
    protected void NotifyObservers()
    {
        playerObservers.ForEach((observer) =>
        {
            observer.OnNotify();
        });
    }
}
