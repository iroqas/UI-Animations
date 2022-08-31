using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class UIAnimInOutEffectBase : MonoBehaviour
{
    [SerializeField] public UIAnimStateInOut state;

    [SerializeField] public LeanTweenType easeIn;
    [SerializeField] public LeanTweenType easeOut;

    [SerializeField] protected float animLengthIn;
    [SerializeField] protected float animLengthOut;

    [SerializeField] protected LTDescr activeAnim;





    /// <summary>
    /// IN Animation length in seconds 
    /// </summary>
    public float AnimLengthIn { get => animLengthIn; set => animLengthIn = value; }

    /// <summary>
    /// OUT animation length in seconds
    /// </summary>
    public float AnimLengthOut { get => animLengthOut; set => animLengthOut = value; }

    public abstract GameObject GetTarget();

    public abstract void PlayIn(UnityAction onFinish);
    public abstract void PlayIn(float animationLength, UnityAction onFinish);

    public abstract void PlayOut(UnityAction onFinish);
    public abstract void PlayOut(float animationLength, UnityAction onFinish);
    

    /// <summary>
    /// Finish active animation instantly.
    /// </summary>
    protected void FinishActiveAnimation()
    {
        if (activeAnim != null)
        {
            LeanTween.cancel(activeAnim.id);
            activeAnim = null;

            switch (state)
            {
                case UIAnimStateInOut.IN:
                    break;
                case UIAnimStateInOut.OUT:
                    break;
                case UIAnimStateInOut.TRANSITIONING_TO_IN:
                    state = UIAnimStateInOut.IN;
                    break;
                case UIAnimStateInOut.TRANSITIONING_TO_OUT:
                    state = UIAnimStateInOut.OUT;
                    break;
            }
        }
    }

}
