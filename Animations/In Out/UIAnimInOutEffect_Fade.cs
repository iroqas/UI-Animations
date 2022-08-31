using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIAnimInOutEffect_Fade : UIAnimInOutEffectBase
{
    public GameObject target;

    private void Update()
    {

        //switch (state)
        //{
        //    case UIAnimStateInOut.IN:
                
        //        break;
        //    case UIAnimStateInOut.OUT:
                
        //        break;
        //    case UIAnimStateInOut.TRANSITIONING_TO_IN:
        //        break;
        //    case UIAnimStateInOut.TRANSITIONING_TO_OUT:
        //        break;
        //}
    }


    public override GameObject GetTarget()
    {
        return this.gameObject;
    }

    public bool IsOnTransition()
    {
        if (state != UIAnimStateInOut.IN || state != UIAnimStateInOut.OUT)
        {
            return false;
        }

        return true;
    }

    public override void PlayIn(UnityAction onFinish)
    {
        PlayIn(animLengthIn, onFinish);
    }

    public override void PlayIn(float animationLength, UnityAction onFinish)
    {
        CanvasGroup canvasGroup = target.GetComponent<CanvasGroup>();



            FinishActiveAnimation();


            activeAnim = LeanTween.value(0, 1, animationLength)
                .setEase(easeIn)
                .setOnUpdate((float value) =>
                {
                    canvasGroup.alpha = value;
                    this.state = UIAnimStateInOut.TRANSITIONING_TO_IN;
                })
                 .setOnComplete(() =>
                 {
                     this.state = UIAnimStateInOut.IN;
                     activeAnim = null;
                     onFinish.Invoke();
                 });

    }

    public override void PlayOut(UnityAction onFinish)
    {
        PlayOut(animLengthOut, onFinish);
    }

    public override void PlayOut(float animationLength, UnityAction onFinish)
    {
        CanvasGroup canvasGroup = target.GetComponent<CanvasGroup>();



            FinishActiveAnimation();


            activeAnim = LeanTween.value(1, 0, animationLength)
                .setEase(easeIn)
                .setOnUpdate((float value) =>
                {
                    canvasGroup.alpha = value;
                    this.state = UIAnimStateInOut.TRANSITIONING_TO_OUT;
                })
                 .setOnComplete(() =>
                 {
                     this.state = UIAnimStateInOut.OUT;
                     activeAnim = null;
                     onFinish.Invoke();
                 });
        
    }
}
