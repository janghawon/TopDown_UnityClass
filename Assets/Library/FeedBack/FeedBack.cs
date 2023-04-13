using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FeedBack : MonoBehaviour
{
    public abstract void CreateFeddBack();
    public abstract void FinishFeedBack();

    protected virtual void OnDestroy()
    {
        FinishFeedBack();
    }

    protected virtual void OnDisable()
    {
        CreateFeddBack();
    }
}
