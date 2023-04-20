using UnityEngine;

public abstract class Feedback : MonoBehaviour
{
    public abstract void CreateFeedback();
    public abstract void FinishFeedback();

    protected virtual void OnDestroy()
    {
        //FinishFeedback();
    }

    protected virtual void OnDisable()
    {
        FinishFeedback();
    }
}
