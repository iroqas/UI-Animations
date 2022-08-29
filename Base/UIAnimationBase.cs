using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class UIAnimationBase : MonoBehaviour
{
    public LeanTweenType easeIn;
    public LeanTweenType easeOut;
    public UIAnimState state;

    public abstract GameObject GetTarget(); 
    public abstract void Show(float delay, float animationLength, UnityAction onFinish);
    public abstract void Hide(float delay, float animationLength, UnityAction onFinish);

}
