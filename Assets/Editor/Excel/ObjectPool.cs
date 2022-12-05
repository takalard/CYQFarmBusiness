using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ObjectPool<T> : IDisposable where T : new()
{
    private readonly Stack<T> m_Stack = new Stack<T>();
    private bool disposedValue;

    public int instanceNum { get; private set; }
    private Action<T> m_Release;
    public ObjectPool(Action<T> Release, int initNum = 0)
    {
        for (int i = 0; i < initNum; i++)
        {
            m_Stack.Push(new T());
        }
        instanceNum = initNum;
        m_Release = Release;
    }

    public T Get()
    {
        T element;
        if (m_Stack.Count == 0)
        {
            element = new T();
            instanceNum++;
        }
        else
        {
            element = m_Stack.Pop();
            instanceNum--;
        }
        return element;
    }

    public void Release(T element)
    {
        if (m_Stack.Count > 0 && ReferenceEquals(m_Stack.Peek(), element))
            Debug.LogError("Internal error. Trying to destroy object that is already released to pool.");
        if (m_Release != null) m_Release(element);
        m_Stack.Push(element);
        instanceNum++;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                m_Stack.Clear();
                // TODO: 释放托管状态(托管对象)
            }

            // TODO: 释放未托管的资源(未托管的对象)并重写终结器
            // TODO: 将大型字段设置为 null
            disposedValue = true;
        }
    }

    // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
    ~ObjectPool()
    {
        // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}