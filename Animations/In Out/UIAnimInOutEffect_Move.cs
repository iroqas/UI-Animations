using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Implementation of the abstract class UIAnimationBase.
/// 
/// This class allows animating a UI gameobject to show/hide it by moving it ininside/outside of the screen.
/// 
/// Dependences:
///     - This script requires LeanTween package to work.
/// 
/// </summary>
public class UIAnimInOutEffect_Move : UIAnimInOutEffectBase
{
    public enum AnimationDIrection { LEFT, RIGHT, UP, DOWN }
    public AnimationDIrection hideDirection;
    public Canvas canvas;
    public GameObject target;


    private void Update()
    {

        switch (state)
        {
            case UIAnimStateInOut.IN:
                this.transform.position = CalculateWorldShownPosition(canvas, this.gameObject);
                break;
            case UIAnimStateInOut.OUT:
                this.transform.position = CalculateWorldHidePosition(canvas, this.gameObject, hideDirection);
                break;
            case UIAnimStateInOut.TRANSITIONING_TO_IN:
                break;
            case UIAnimStateInOut.TRANSITIONING_TO_OUT:
                break;
        }
    }

    #region public methods

    /// <summary>
    /// Triggers animation in.
    /// </summary>
    /// <param name="onFinish"></param>
    public override void PlayIn(UnityAction onFinish)
    {
        PlayIn(animLengthIn, onFinish);
    }

    /// <summary>
    /// Triggers animation in.
    /// </summary>
    /// <param name="delay">Waiting seconds before triggering animation.</param>
    /// <param name="animationLength">Length of the animation in seconds.</param>
    /// <param name="onFinish">Callback called when animation finishes</param>
    public override void PlayIn(float animationLength, UnityAction onFinish)
    {


        FinishActiveAnimation();


        activeAnim = LeanTween.value(0, 1, animationLength)
            .setEase(easeIn)
            .setOnUpdate((float value) =>
            {
                Vector2 showPosition = CalculateWorldShownPosition(canvas, target);
                Vector2 hidePosition = CalculateWorldHidePosition(canvas, target, hideDirection);

                Debug.DrawLine(showPosition, hidePosition);
                this.state = UIAnimStateInOut.TRANSITIONING_TO_IN;
                target.transform.position = Vector3.Lerp(hidePosition, showPosition, value);
            })
                .setOnComplete(() =>
                {
                    this.state = UIAnimStateInOut.IN;
                    activeAnim = null;
                    onFinish.Invoke();
                });

    }


    /// <summary>
    /// Triggers animation out.
    /// </summary>
    /// <param name="onFinish"></param>
    public override void PlayOut(UnityAction onFinish)
    {
        PlayOut(animLengthOut, onFinish);
    }

    /// <summary>
    /// Triggers animation out.
    /// </summary>
    /// <param name="delay">Waiting seconds before triggering animation.</param>
    /// <param name="animationLength">Length of the animation in seconds.</param>
    /// <param name="onFinish">Callback called when animation finishes</param>
    public override void PlayOut(float animationLength, UnityAction onFinish)
    {

        FinishActiveAnimation();

        activeAnim = LeanTween.value(0, 1, animationLength)
            .setEase(easeOut)
            .setOnUpdate((float value) =>
            {
                Vector2 showPosition = CalculateWorldShownPosition(canvas, this.gameObject);
                Vector2 hidePosition = CalculateWorldHidePosition(canvas, this.gameObject, hideDirection);

                Debug.DrawLine(showPosition, hidePosition);
                this.state = UIAnimStateInOut.TRANSITIONING_TO_OUT;
                target.transform.position = Vector3.Lerp(showPosition, hidePosition, value);
            })
            .setOnComplete(() =>
            {
                this.state = UIAnimStateInOut.OUT;
                activeAnim = null;
                onFinish.Invoke();
            });

    }


    

    #endregion





    /// <summary>
    /// Returns this GameObject reference
    /// </summary>
    /// <returns></returns>
    public override GameObject GetTarget()
    {
        return this.gameObject;
    }

    //public void Init()
    //{
    //    switch (state)
    //    {
    //        case UIAnimStateInOut.IN:
    //            this.transform.position = CalculateWorldShownPosition(canvas, this.gameObject);
    //            break;
    //        case UIAnimStateInOut.OUT:
    //            this.transform.position = CalculateWorldHidePosition(canvas, this.gameObject, hideDirection);
    //            break;
    //        case UIAnimStateInOut.TRANSITIONING_TO_IN:
    //            this.transform.position = CalculateWorldShownPosition(canvas, this.gameObject);
    //            break;
    //        case UIAnimStateInOut.TRANSITIONING_TO_OUT:
    //            this.transform.position = CalculateWorldHidePosition(canvas, this.gameObject, hideDirection);
    //            break;
    //    }
    //}

    /// <summary>
    /// Checks if the animation is playing or not.
    /// </summary>
    /// <returns></returns>
    public bool IsOnTransition()
    {
        if (state != UIAnimStateInOut.IN || state != UIAnimStateInOut.OUT)
        {
            return false;
        }

        return true;
    }



    /// <summary>
    /// Calculates the position were the target GameObject should move on to hide from screen.
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="target"></param>
    /// <param name="hideDirection"></param>
    /// <returns></returns>

    private Vector2 CalculateWorldHidePosition(Canvas canvas, GameObject target, AnimationDIrection hideDirection)
    {
        RectTransform canvasRectTransform = (RectTransform)canvas.transform;
        RectTransform targetRectTransform  = (RectTransform)target.transform;

        Vector2 canvasSize = new Vector2(canvasRectTransform.rect.width, canvasRectTransform.rect.height);

        Vector2 result = canvasRectTransform.InverseTransformPoint(CalculateWorldShownPosition(canvas, target));
        


        switch (hideDirection)
        {
            case AnimationDIrection.LEFT:
                result.x = -(canvasSize.x / 2) - (targetRectTransform.rect.width/2);
                //result.y = canvasRectTransform.InverseTransformPoint(targetRectTransform.position).y;

                break;

            case AnimationDIrection.RIGHT:
                result.x = (canvasSize.x / 2) +  (targetRectTransform.rect.width / 2);
                //result.y = canvasRectTransform.InverseTransformPoint(targetRectTransform.position).y;
                break;

            case AnimationDIrection.UP:
                //result.x = canvasRectTransform.InverseTransformPoint(targetRectTransform.position).x;
                result.y = (canvasSize.y / 2) + (targetRectTransform.rect.width / 2);
                break;

            case AnimationDIrection.DOWN:
                //result.x = canvasRectTransform.InverseTransformPoint(targetRectTransform.position).x;
                result.y = -(canvasSize.y / 2) - (targetRectTransform.rect.width / 2);
                break;
        }

        Vector2 worldResult = canvasRectTransform.TransformPoint(result);

        return worldResult;
    }


    /// <summary>
    /// Calculates the position were the target GameObject should move to be shown on screen.
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="target"></param>
    /// <param name="hideDirection"></param>
    /// <returns></returns>
    private Vector2 CalculateWorldShownPosition(Canvas canvas, GameObject target)
    {
        return target.transform.parent.TransformPoint(Vector2.zero);
        //return canvas.transform.InverseTransformPoint(worldPos);
    }

    

    

    
}



