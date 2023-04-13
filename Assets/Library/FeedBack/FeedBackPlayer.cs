using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedBackPlayer : MonoBehaviour
{
    private List<FeedBack> _feedBackList;

    private void Awake()
    {
        _feedBackList = new List<FeedBack>();
        GetComponents<FeedBack>(_feedBackList);
    }

    public void PlayerFeedback()
    {
        FinishFeedback();
        foreach(FeedBack f in _feedBackList)
        {
            f.CreateFeddBack();
        }
    }

    public void FinishFeedback()
    {
        foreach(FeedBack f in _feedBackList)
        {
            f.FinishFeedBack();
        }
    }
}
