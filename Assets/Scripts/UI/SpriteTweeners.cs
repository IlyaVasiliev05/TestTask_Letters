using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTweeners
{

    public static void SpriteAlphaCrossFromValueToValue(MonoBehaviour monoBeh, SpriteRenderer sprite, float startAlpha, float endAlpha, float time)
    {
        monoBeh.StartCoroutine(AlphaCrossFromValueToValue(sprite, startAlpha, endAlpha, time));
    }

    public static Coroutine SpriteScaleCrossFromValueToValue(MonoBehaviour monoBeh, Transform sprite, float startScale, float endScale, float time)
    {
        return monoBeh.StartCoroutine(ScaleCrossFromValueToValue(sprite, startScale, endScale, time));
    }

    public static void SpriteScaleViaCurve(MonoBehaviour monoBeh, Transform sprite, AnimationCurve curve, float mult, float time)
    {
        monoBeh.StartCoroutine(ScaleSpriteViaCurve(sprite, curve, mult, time));
    }

    public static Coroutine SpriteScaleViaCurveLoop(MonoBehaviour monoBeh, Transform sprite, AnimationCurve curve, float mult, float time)
    {
        return monoBeh.StartCoroutine(ScaleSpriteViaCurveLoop(sprite, curve, mult, time));
    }

    public static void SpriteScaleCrossFromValueToValue(MonoBehaviour monoBeh, Transform sprite, Vector3 startScale, Vector3 endScale, float time)
    {
        monoBeh.StartCoroutine(ScaleCrossFromValueToValue(sprite, startScale, endScale, time));
    }

    public static void SpriteMoveToPoint(MonoBehaviour monoBeh, Transform sprite, Vector3 targetPos, float time)
    {
        monoBeh.StartCoroutine(MoveToPoint(sprite, targetPos, time));
    }

    public static void SpriteMoveToPointViaCurve(MonoBehaviour monoBeh, Transform sprite, Vector3 targetPos, AnimationCurve curve, float time, bool scaledTime = false)
    {
        monoBeh.StartCoroutine(MoveToPointViaCurve(sprite, targetPos, curve, time, scaledTime));
    }

    public static void SpriteRotateFromToGlobal(MonoBehaviour monoBeh, Transform sprite, Quaternion from, Quaternion to, float time, bool scaledTime = true)
    {
        monoBeh.StartCoroutine(RotateGlobalFromTo(sprite, from, to, time, scaledTime));
    }

    #region SpriteScale

    static IEnumerator ScaleCrossFromValueToValue(Transform sprite, float startScale, float endScale, float time)
    {
        Vector3 startScaleV = Vector3.one * startScale;
        Vector3 endScaleV = Vector3.one * endScale;

        float t = 0;
        float speed = 1 / time;
        while (t < 1)
        {
            sprite.localScale = Vector3.Lerp(startScaleV, endScaleV, t);
            t += Time.deltaTime * speed;
            yield return null;
        }
        sprite.localScale = endScaleV;
        yield break;
    }

    static IEnumerator ScaleSpriteViaCurve(Transform sprite, AnimationCurve curve, float mult, float time)
    {
        float t = 0;
        float speed = 1 / time;
        while (t < 1)
        {
            sprite.localScale = Vector3.one * curve.Evaluate(t) * mult;
            t += Time.unscaledDeltaTime * speed;
            yield return new WaitForEndOfFrame();
        }
        sprite.localScale = Vector3.one * curve.keys[curve.keys.Length - 1].value * mult;
        yield break;
    }

    static IEnumerator ScaleSpriteViaCurveLoop(Transform sprite, AnimationCurve curve, float mult, float time)
    {
        float t = 0;
        float speed = 1 / time;
        while (true)
        {
            t = 0;
            while (t < 1)
            {
                sprite.localScale = Vector3.one * curve.Evaluate(t) * mult;
                t += Time.unscaledDeltaTime * speed;
                yield return new WaitForEndOfFrame();
            }
            sprite.localScale = Vector3.one * curve.keys[curve.keys.Length - 1].value * mult;
        }
    }

    static IEnumerator ScaleCrossFromValueToValue(Transform sprite, Vector3 startScale, Vector3 endScale, float time)
    {
        Vector3 startScaleV = startScale;
        Vector3 endScaleV = endScale;

        float t = 0;
        float speed = 1 / time;
        while (t < 1)
        {
            sprite.localScale = Vector3.Lerp(startScaleV, endScaleV, t);
            t += Time.deltaTime * speed;
            yield return null;
        }
        sprite.localScale = endScaleV;
        yield break;
    }

    #endregion

    #region SpriteColorAlpha

    static IEnumerator AlphaCrossFromValueToValue(SpriteRenderer sprite, float startAlpha, float endAlpha, float time)
    {
        Color defColor = sprite.color;
        float a = startAlpha;
        float speed = (endAlpha - startAlpha) / (time);
        if (speed > 0)
        {
            while (a < endAlpha)
            {
                defColor.a = a;
                sprite.color = defColor;
                a += Time.unscaledDeltaTime * speed;
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (a > endAlpha)
            {
                defColor.a = a;
                sprite.color = defColor;
                a += Time.unscaledDeltaTime * speed;
                yield return new WaitForEndOfFrame();
            }
        }
        defColor.a = endAlpha;
        sprite.color = defColor;


        yield break;
    }

    #endregion

    #region SpriteMove

    static IEnumerator MoveToPoint(Transform sprite, Vector3 targetPos, float time)
    {
        Vector3 startPos = sprite.position;
        float t = 0;
        float speed = 1 / time;

        while (t < 1)
        {
            sprite.position = Vector3.Lerp(startPos, targetPos, t);
            t += Time.deltaTime * speed;
            yield return null;
        }
        sprite.position = targetPos;

        yield break;
    }

    private static IEnumerator MoveToPointViaCurve(Transform sprite, Vector3 targetPos, AnimationCurve curve, float time, bool scaledTime)
    {
        Vector3 startPos = sprite.position;
        float t = 0;
        float speed = 1f / time;
        while (t < 1)
        {
            sprite.position = Vector3.LerpUnclamped(startPos, targetPos, curve.Evaluate(t));
            t += scaledTime? Time.deltaTime * speed : Time.unscaledDeltaTime * speed;
            yield return null;
        }
        sprite.position = targetPos;
        yield break;
    }

    #endregion

    #region SpriteRotation

    private static IEnumerator RotateGlobalFromTo(Transform sprite, Quaternion from, Quaternion to, float time, bool scaledTime)
    {
        float speed = 1f / time;
        float t = 0;
        while (t < 1)
        {
            sprite.transform.rotation = Quaternion.Lerp(from, to, t);
            t += scaledTime? Time.deltaTime * speed : Time.unscaledDeltaTime * speed;
            yield return null;
        }
        sprite.transform.rotation = to;

        yield break;
    }

    #endregion

}
