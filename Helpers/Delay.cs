using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.Events;

public class Delay
{
    Timer timer;
    UnityAction callback;

    public Delay(float delay, UnityAction callback)
    {
        this.callback = callback;
        timer = new System.Timers.Timer(delay);
        timer.Elapsed += Timer_Elapsed;
        timer.AutoReset = false;
        timer.Start();

    }

    private void Timer_Elapsed(object sender, ElapsedEventArgs e)
    {

        callback.Invoke();
        timer.Stop();
        timer.Dispose();
    }
    
    public static Delay Run(float delay, UnityAction callback)
    {
        var instance = new Delay(delay, callback);
        return instance;
    }

    public void Stop()
    {
        timer.Stop();
    }






}
