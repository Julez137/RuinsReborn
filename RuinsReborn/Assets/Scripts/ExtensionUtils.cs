using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionUtils
{
    /// <summary>
    /// Delay the given callback by the given duration in seconds
    /// </summary>
    /// <param name="monoBehaviour"></param>
    /// <param name="duration"></param>
    /// <param name="callback"></param>
    public static void DelaySeconds(this MonoBehaviour monoBehaviour, float duration, Action callback)
    {
        monoBehaviour.StartCoroutine(DelaySecondsRoutine(duration, callback));
    }
    static IEnumerator DelaySecondsRoutine(float duration, Action callback)
    {
        float time = 0;
        while (true)
        {
            if (time >= duration)
            {
                callback();
                break;
            }
            else
            {
                time += Time.deltaTime;
                yield return null;
            }
        }
    }

    /// <summary>
    /// Delay the given callback by the given frames
    /// </summary>
    /// <param name="monoBehaviour"></param>
    /// <param name="frames"></param>
    /// <param name="callback"></param>
    public static void DelayFrames(this MonoBehaviour monoBehaviour, int frames, Action callback = null)
    {
        monoBehaviour.StartCoroutine(DelayFramesRoutine(frames, callback));
    }
    static IEnumerator DelayFramesRoutine(int frames, Action callback)
    {
        for (int loop = 0; loop < frames; loop++)
        {
            yield return null;
        }

        callback?.Invoke();
        yield break;
    }
}
