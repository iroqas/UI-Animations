using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIAnimInOutGroup : MonoBehaviour
{


[SerializeField] private List<UIAnimationInOut> animations;

    public bool debugShow;
    public bool debugHide;


    public float delay;
    public float waitCascade;

    public enum GroupAnimationType { ONE_TIME, CASCADE}
    public GroupAnimationType animType;



    private void Update()
    {

        HandleDebugTriggers();

    }

    private void HandleDebugTriggers()
    {
        if (debugShow)
        {
            debugShow = false;
            PlayIn(delay, () =>
            {
                Debug.Log("Debug Show aniation finished");
            });
        }

        if (debugHide)
        {
            debugHide = false;
            PlayOut(delay, () =>
            {
                Debug.Log("Debug Hide aniation finished");
            });
        }


    }

    public void PlayIn(float delay, UnityAction onFinish)
    {
        switch (animType)
        {
            case GroupAnimationType.ONE_TIME:
                StartCoroutine(PlayInCoroutine(delay, onFinish));
                break;
            case GroupAnimationType.CASCADE:
                StartCoroutine(PlayInCoroutine(delay, waitCascade, onFinish));
                break;
        }
        

    }

    public IEnumerator PlayInCoroutine(float delay, UnityAction onFinish)
    {
        yield return new WaitForSeconds(delay);

        int waitingToFinish = 0;

        foreach (var animation in animations)
        {

            waitingToFinish++;


            animation.PlayIn(() =>
            {
                waitingToFinish--;

                if (waitingToFinish == 0)
                {
                    waitingToFinish = -1;
                    onFinish.Invoke();
                }
            });
        }
    }

    public IEnumerator PlayInCoroutine(float delay, float wait, UnityAction onFinish)
    {
        yield return new WaitForSeconds(delay);

        int waitingToFinish = 0;

        foreach (var animation in animations)
        {

            waitingToFinish++;


            animation.PlayIn(() =>
            {
                waitingToFinish--;

                if (waitingToFinish == 0)
                {
                    waitingToFinish = -1;
                    onFinish.Invoke();
                }
            });

            yield return new WaitForSeconds(wait);
        }
    }

    public void PlayOut(float delay, UnityAction onFinish)
    {

        switch (animType)
        {
            case GroupAnimationType.ONE_TIME:
                StartCoroutine(PlayOutCoroutine(delay, onFinish));
                break;
            case GroupAnimationType.CASCADE:
                StartCoroutine(PlayOutCoroutine(delay, waitCascade, onFinish));
                break;
        }

    }

    public IEnumerator PlayOutCoroutine(float delay, UnityAction onFinish)
    {
        yield return new WaitForSeconds(delay);

        int waitingToFinish = 0;

        foreach (var animation in animations)
        {

            waitingToFinish++;

            animation.PlayOut(() =>
            {
                waitingToFinish--;

                if (waitingToFinish == 0)
                {
                    waitingToFinish = -1;
                    onFinish.Invoke();
                }
            });
        }
    }

    public IEnumerator PlayOutCoroutine(float delay, float wait, UnityAction onFinish)
    {
        yield return new WaitForSeconds(delay);

        int waitingToFinish = 0;

        foreach (var animation in animations)
        {

            waitingToFinish++;

            animation.PlayOut(() =>
            {
                waitingToFinish--;

                if (waitingToFinish == 0)
                {
                    waitingToFinish = -1;
                    onFinish.Invoke();
                }
            });

            yield return new WaitForSeconds(wait);
        }
    }



    public void PlayInCasacde(float delay, float delayBetweenAnim, UnityAction onFinish)
    {



    }
    public void PlayOutCascade(float delay, float delayBetweenAnim, UnityAction onFinish)
    {




    }



    public void PlayInRandom(float delay, UnityAction onFinish)
    {




    }
    public void PlayOutRandom(float delay, UnityAction onFinish)
    {




    }



}
