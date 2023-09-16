using System;
using UnityEngine;

public abstract class BaseStateMachine<T> : MonoBehaviour
{
    public T State { get; protected set; }

    public abstract void ChangeState(T newState);
    protected abstract void Think();
    protected abstract void Act();
}
