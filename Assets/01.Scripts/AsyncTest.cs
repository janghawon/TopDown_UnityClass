using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class CoroutineHandle : IEnumerator
{
    public bool IsDone { get; private set; }
    public object Current { get; }

    public bool MoveNext()
    {
        return !IsDone;
    }

    public void Reset()
    {

    }

    public CoroutineHandle(MonoBehaviour owner, IEnumerator coroutine)
    {
        Current = owner.StartCoroutine(coroutine);
    }

    private IEnumerator Wrap(IEnumerator coroutine)
    {
        yield return coroutine;
        IsDone = true;
    }
}

public class AsyncTest : MonoBehaviour
{
    private void Start()
    {
        if(Thread.CurrentThread.Name == null)
        {
            Thread.CurrentThread.Name = "MainThread";
        }
        Debug.Log(Thread.CurrentThread.Name);
        StartJob();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _num = 0;
            
            Task.Run(() => inc());
            Task.Run(() => dec());
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("I'm Alive");
        }
    }

    private void MyJob()
    {
        Thread.Sleep(3000);
        Debug.Log("Jib Complete");
        Debug.Log($"{Thread.CurrentThread.Name} : {Thread.CurrentThread.ManagedThreadId}");
    }

    private async void StartJob()
    {
        _num = 0;

        var t1 = Task.Run(() => inc());
        var t2 = Task.Run(() => dec());

        await Task.WhenAll(new[] { t1, t2 });
        Debug.Log(_num);
    }

    private int _num = 0;
    private object obj = new object();

    private void inc()
    {
        for(int i = 0; i < 9999999; i++)
        {
            lock(obj)
            {
                _num++;
            }
        }
    }

    private void dec()
    {
        for (int i = 0; i < 9999999; i++)
        {
            lock(obj)
            {
                _num--;
            }
        }
    }
}
