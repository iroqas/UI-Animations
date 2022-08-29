using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIAnimation_MoveInOut : UIAnimationBase
{
    public enum AnimationDIrection { LEFT, RIGHT, UP, DOWN }
    public AnimationDIrection hideDirection;
    public float delay;
    public float animationLength;
    public Canvas canvas;
    public GameObject target;


    public bool debugShow = false;
    public bool debugHide = false;


    private void Update()
    {
        if (debugShow)
        {
            debugShow = false;
            Show(delay, animationLength, () =>
            {
                Debug.Log("Debug Show aniation finished");
            });
        }

        if (debugHide)
        {
            debugHide = false;
            Hide(delay, animationLength,  () =>
            {
                Debug.Log("Debug Hide aniation finished");
            });
        }

        switch (state)
        {
            case UIAnimState.SHOWING:
                this.transform.position = CalculateWorldShownPosition(canvas, this.gameObject);
                break;
            case UIAnimState.HIDDEN:
                this.transform.position = CalculateWorldHidePosition(canvas, this.gameObject, hideDirection);
                break;
            case UIAnimState.TRANSITIONING_TO_SHOW:
                break;
            case UIAnimState.TRANSITIONING_TO_HIDE:
                break;
        }
    }

    /// <summary>
    /// Triggers hide animation.
    /// </summary>
    /// <param name="delay">Waiting seconds before triggering animation.</param>
    /// <param name="animationLength">Length of the animation in seconds.</param>
    /// <param name="onFinish">Callback called when animation finishes</param>
    public override void Hide(float delay, float animationLength, UnityAction onFinish)
    {

        LeanTween.value(0,  1, animationLength)
            .setEase(easeOut)
            .setOnUpdate((float value) =>
            {
                Vector2 showPosition = CalculateWorldShownPosition(canvas, this.gameObject);
                Vector2 hidePosition = CalculateWorldHidePosition(canvas, this.gameObject, hideDirection);

                Debug.DrawLine(showPosition, hidePosition);
                this.state = UIAnimState.TRANSITIONING_TO_HIDE;
                target.transform.position = Vector3.Lerp(showPosition, hidePosition, value);
            })
            .setOnComplete(() =>
            {
                this.state = UIAnimState.HIDDEN;
                onFinish.Invoke();
            });
    }

    /// <summary>
    /// Triggers show animation.
    /// </summary>
    /// <param name="delay">Waiting seconds before triggering animation.</param>
    /// <param name="animationLength">Length of the animation in seconds.</param>
    /// <param name="onFinish">Callback called when animation finishes</param>
    public override void Show(float delay, float animationLength, UnityAction onFinish)
    {

        LeanTween.value(0, 1, animationLength)
            .setEase(easeIn)
            .setOnUpdate((float value) =>
            {
                Vector2 showPosition = CalculateWorldShownPosition(canvas, target);
                Vector2 hidePosition = CalculateWorldHidePosition(canvas, target, hideDirection);

                Debug.DrawLine(showPosition, hidePosition);
                this.state = UIAnimState.TRANSITIONING_TO_SHOW;
                target.transform.position = Vector3.Lerp(hidePosition, showPosition, value);
            })
             .setOnComplete(() =>
             {
                 this.state = UIAnimState.SHOWING;
                 onFinish.Invoke();
             });
    }

    public override GameObject GetTarget()
    {
        return this.gameObject;
    }

    public void Init()
    {
        switch (state)
        {
            case UIAnimState.SHOWING:
                this.transform.position = CalculateWorldShownPosition(canvas, this.gameObject);
                break;
            case UIAnimState.HIDDEN:
                this.transform.position = CalculateWorldHidePosition(canvas, this.gameObject, hideDirection);
                break;
            case UIAnimState.TRANSITIONING_TO_SHOW:
                this.transform.position = CalculateWorldShownPosition(canvas, this.gameObject);
                break;
            case UIAnimState.TRANSITIONING_TO_HIDE:
                this.transform.position = CalculateWorldHidePosition(canvas, this.gameObject, hideDirection);
                break;
        }
    }

    public bool IsOnTransition()
    {
        if (state != UIAnimState.SHOWING || state != UIAnimState.HIDDEN)
        {
            return false;
        }

        return true;
    }




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

    private Vector2 CalculateWorldShownPosition(Canvas canvas, GameObject target)
    {
        return target.transform.parent.TransformPoint(Vector2.zero);
        //return canvas.transform.InverseTransformPoint(worldPos);
    }

    

    



}



