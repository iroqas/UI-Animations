using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIAnimationInOut : MonoBehaviour
{

    public List<UIAnimInOutEffectBase> inOutEffects;

    private void Awake()
    {
        inOutEffects = new List<UIAnimInOutEffectBase>(GetComponents<UIAnimInOutEffectBase>());    
    }

    public void PlayIn(UnityAction onFinish)
    {
        foreach(var effect in inOutEffects)
        {
            effect.PlayIn(onFinish);
        }
    }

    public void PlayOut(UnityAction onFinish)
    {
        foreach (var effect in inOutEffects)
        {
            effect.PlayOut(onFinish);
        }
    }


}
