using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class GenericSubject <T> : MonoBehaviour where T : Enum
{
    private HashSet<IGenericObserver<T>> _observers = new HashSet<IGenericObserver<T>>();

    public void AddObserver(IGenericObserver<T> observer)
    {
        _observers.Add(observer);
    }

    public void RemoveObserver(IGenericObserver<T> observer)
    {
        _observers.Remove(observer);
    }

    public void NotifyObservers(T action) {
        // iterate through the current list of stored observers and call their 
        foreach(var observer in _observers) {
            observer.OnNotify(action);
        }
    }
}





















