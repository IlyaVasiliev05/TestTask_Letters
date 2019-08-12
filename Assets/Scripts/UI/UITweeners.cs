using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITweeners {

    public static UITweeners Instance
    {
        get
        {
            if (instance == null)
                instance = new UITweeners();
            return instance;
        }
    }

    private static UITweeners instance;

    #region Image
    //image alpha
    public void ImCrossFromTransparentToCurrent(MonoBehaviour monoBeh, Image image, float time)
	{
		monoBeh.StartCoroutine(CrossFromTransparentToCurrent(image,time));
	}

	public void ImCrossFromCurrentToTransparent(MonoBehaviour monoBeh, Image image, float time)
	{
		monoBeh.StartCoroutine(CrossFromCurrentToTransparent(image,time));
	}

    public void ImAlphaCrossFromCurrentToValue(MonoBehaviour monoBeh, Image image, float alphaValue, float time)
    {
        monoBeh.StartCoroutine(AlphaCrossFromCurrentToValue(image, alphaValue, time));
    }

    public void ImAlphaCrossFromValueToValue(MonoBehaviour monoBeh, Image image, float startAlphaValue, float endAlphaValue, float time, bool scaledTime = false)
    {
        monoBeh.StartCoroutine(AlphaCrossFromValueToValue(image, startAlphaValue, endAlphaValue, time, scaledTime));
    }

    //image scale
    public void ImScaleCrossFromCurrentToOne(MonoBehaviour monoBeh, Image image, float startScale, float time)
	{
		monoBeh.StartCoroutine(SetImageFromToOne(image, startScale, time));
	}

	public void ImScaleCrossFromCurrentToScale(MonoBehaviour monoBeh, RectTransform image,float targetSize, float time)
	{
		monoBeh.StartCoroutine(SetImageToSize(image, targetSize, time));
	}

    public void ImScaleCrossFromValueToValue(MonoBehaviour monoBeh, RectTransform image, float startSize, float targetSize, float time)
    {
        monoBeh.StartCoroutine(SetImageFromScaleToScale(image, startSize, targetSize, time));
    }

    public void ImScaleViaCuvre(MonoBehaviour monoBeh, Image im, float sizeMult , AnimationCurve animCurve, float time, bool scaledTime = false)
    {
        monoBeh.StartCoroutine(ImageScaleViaCurve(im, animCurve, sizeMult, time, scaledTime));
    }


    #endregion

    #region Text
    // text alpha
    public void TextCrossFromTransparentToCurrent(MonoBehaviour monoBeh, Text text, float time)
	{
		monoBeh.StartCoroutine(TextCrossFromTransparentToCurrent(text, time));
	}

	public void TextCrossFromCurrentToTransparent(MonoBehaviour monoBeh, Text text, float time)
	{
		monoBeh.StartCoroutine(TextCrossFromCurrentToTransparent(text, time));
	}

    public void TextCrossFromValueToValue(MonoBehaviour monoBeh, Text text, float startAlphaValue, float endAlphaValue, float time)
    {
        monoBeh.StartCoroutine(TextAlphaCrossFromCurrentToValue(text, startAlphaValue, endAlphaValue, time));
    }
    #endregion

    #region Arrays Images
    // Arrays
    public void ArrayImageAlphaCrossFromValueToValue(MonoBehaviour monoBeh, List<Image> imageList, float startAlphaValue, float endAlphaValue, float time)
    {
        monoBeh.StartCoroutine(ArrayImageAlphaCrossFromValueToValueProcess(imageList, startAlphaValue, endAlphaValue, time));
    }

    #endregion

    #region Arrays Texts
    // Arrays
    public void ArrayTextAlphaCrossFromValueToValue(MonoBehaviour monoBeh, List<Text> textList, float startAlphaValue, float endAlphaValue, float time)
    {
        monoBeh.StartCoroutine(ArrayTextAlphaCrossFromValueToValueProcess(textList, startAlphaValue, endAlphaValue, time));
    }

    #endregion

    #region Coroutines

    IEnumerator CrossFromTransparentToCurrent(Image im, float time)
	{
		Color32 defColor = im.color;
		float speed = (float)defColor.a/(time);
		byte targetAlpha = (byte)defColor.a;
		defColor.a = 0;
		im.color = defColor;
		float a = 0;

		while(a < targetAlpha)
		{
			defColor.a = (byte) a;
			im.color = defColor;
			a += Time.unscaledDeltaTime * speed;
			yield return new WaitForEndOfFrame();
		}
		defColor.a = (byte)targetAlpha;
		im.color = defColor;

		yield break;
	}

	IEnumerator CrossFromCurrentToTransparent(Image im, float time)
	{
		Color32 defColor = im.color;
		float a = defColor.a;
		float speed = (float)defColor.a/(time);
		while(a > 0)
		{
			defColor.a = (byte) a;
			im.color = defColor;
			a -= Time.unscaledDeltaTime * speed;
			yield return new WaitForEndOfFrame();
		}
		defColor.a = (byte)0;
		im.color = defColor;

		yield break;
	}

    IEnumerator AlphaCrossFromCurrentToValue(Image im, float alphaValue ,float time)
    {
        Color32 defColor = im.color;
        float a = defColor.a - 255*alphaValue;
        float speed = (float)defColor.a / (time);
        while (a > 0)
        {
            defColor.a = (byte)a;
            im.color = defColor;
            a -= Time.unscaledDeltaTime * speed;
            yield return new WaitForEndOfFrame();
        }
        defColor.a = (byte)(255*alphaValue);
        im.color = defColor;

        yield break;
    }

    IEnumerator AlphaCrossFromValueToValue(Image im, float startAlpha, float endAlpha, float time, bool scaledTime)
    {
        Color defColor = im.color;
        float a = startAlpha;
        float speed = (endAlpha - startAlpha) / (time);
        //WaitForEndOfFrame wait = new WaitForEndOfFrame();
        if (speed > 0)
        {
            while (a < endAlpha)
            {
                defColor.a = a;
                im.color = defColor;
                a += scaledTime ? Time.deltaTime * speed : Time.unscaledDeltaTime * speed;
                yield return null;
            }
        }
        else
        {
            while (a > endAlpha)
            {
                defColor.a = a;
                im.color = defColor;
                a += Time.unscaledDeltaTime * speed;
                yield return null;
            }
        }
        defColor.a = endAlpha;
        im.color = defColor;


        yield break;
    }


    //IMAGE SIZES
    IEnumerator SetImageFromToOne(Image rectTransforn, float startSize, float time)
	{
		float t = startSize;
		float speed = Mathf.Abs(rectTransforn.rectTransform.localScale.x - startSize)/time;
		while(t < 1)
		{
			rectTransforn.rectTransform.localScale = Vector3.one*t;
			t += Time.unscaledDeltaTime * speed;
			yield return new WaitForEndOfFrame();
		}
		rectTransforn.rectTransform.localScale = Vector3.one;
		yield break;
	}

    IEnumerator ImageScaleViaCurve(Image im, AnimationCurve curve, float sizeMult, float time, bool scaledTime)
    {
        float t = 0;
        float speed = 1f / time;
        while (t < 1)
        {
            im.rectTransform.localScale = Vector3.one * sizeMult * curve.Evaluate(t);
            t += scaledTime? Time.deltaTime * speed : Time.unscaledDeltaTime * speed;
            yield return null;
        }
        im.rectTransform.localScale = Vector3.one * curve.keys[curve.keys.Length-1].value * sizeMult;
        yield break;
    }

    IEnumerator SetImageToSize(RectTransform im, float targetSize, float time)
	{
		float t = im.localScale.x;
		float speed = Mathf.Abs(im.localScale.x - targetSize)/time;
		if(t < targetSize)
		{
			while(t < targetSize)
			{
				im.localScale = Vector3.one*t;
				t += Time.unscaledDeltaTime * speed;
				yield return new WaitForEndOfFrame();
			}
		}
		else
		{
			while(t > targetSize)
			{
				im.localScale = Vector3.one*t;
				t -= Time.unscaledDeltaTime * speed;
				yield return null;
			}
		}
		im.localScale = Vector3.one * targetSize;

		yield break;
	}

    public IEnumerator SetImageFromScaleToScale(RectTransform im, float startSize, float targetSize, float time)
    {
        float t = 0;
        float speed = 1 / time;
        while (t < 1)
        {
            im.localScale = Vector3.one * Mathf.Lerp(startSize, targetSize, t);
            t += Time.unscaledDeltaTime * speed;
            yield return new WaitForEndOfFrame();
        }
        im.localScale = Vector3.one * targetSize;

        yield break;
    }

    //TEXT ALPHA
    IEnumerator TextCrossFromTransparentToCurrent(Text im, float time)
	{
		Color32 defColor = im.color;
		float speed = (float)defColor.a/(time);
		byte targetAlpha = (byte)defColor.a;
		defColor.a = 0;
		im.color = defColor;
		float a = 0;

		while(a < targetAlpha)
		{
			defColor.a = (byte) a;
			im.color = defColor;
			a += Time.unscaledDeltaTime * speed;
			yield return new WaitForEndOfFrame();
		}
		defColor.a = (byte)targetAlpha;
		im.color = defColor;

		yield break;
	}

	IEnumerator TextCrossFromCurrentToTransparent(Text im, float time)
	{
		Color32 defColor = im.color;
		float a = defColor.a;
		float speed = (float)defColor.a/(time);
		while(a > 0)
		{
			defColor.a = (byte) a;
			im.color = defColor;
			a -= Time.unscaledDeltaTime * speed;
			yield return new WaitForEndOfFrame();
		}
		defColor.a = (byte)0;
		im.color = defColor;

		yield break;
	}

    public IEnumerator TextAlphaCrossFromCurrentToValue(Text text, float startAlpha, float endAlpha, float time)
    {
        Color defColor = text.color;
        float a = startAlpha;
        float speed = (endAlpha - startAlpha) / (time);
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        if (speed > 0)
        {
            while (a < endAlpha)
            {
                defColor.a = a;
                text.color = defColor;
                a += Time.unscaledDeltaTime * speed;
                yield return wait;
            }
        }
        else
        {
            while (a > endAlpha)
            {
                defColor.a = a;
                text.color = defColor;
                a += Time.unscaledDeltaTime * speed;
                yield return wait;
            }
        }
        defColor.a = endAlpha;
        text.color = defColor;


        yield break;
    }

    // ARRAYS
    IEnumerator ArrayImageAlphaCrossFromValueToValueProcess(List<Image> imList, float startAlpha, float endAlpha, float time)
    {
        Color [] defColorList = new Color [imList.Count];
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        for (int i = 0; i < imList.Count; i++)
        {
            defColorList[i] = imList[i].color;
        }
        float a = startAlpha;
        float speed = (endAlpha - startAlpha) / (time);
        if (speed > 0)
        {
            while (a < endAlpha)
            {
                for (int i = 0; i < imList.Count; i++)
                {
                    defColorList[i].a = a;
                    imList[i].color = defColorList[i];
                }
                a += Time.unscaledDeltaTime * speed;
                yield return waitForEndOfFrame;
            }
        }
        else
        {
            while (a > endAlpha)
            {
                for (int i = 0; i < imList.Count; i++)
                {
                    defColorList[i].a = a;
                    imList[i].color = defColorList[i];
                }
                a += Time.unscaledDeltaTime * speed;
                yield return waitForEndOfFrame;
            }
        }
        for (int i = 0; i < imList.Count; i++)
        {
            defColorList[i].a = endAlpha;
            imList[i].color = defColorList[i];
        }
        yield break;
    }

    IEnumerator ArrayTextAlphaCrossFromValueToValueProcess(List<Text> textList, float startAlpha, float endAlpha, float time)
    {
        Color[] defColorList = new Color[textList.Count];
        for (int i = 0; i < textList.Count; i++)
        {
            defColorList[i] = textList[i].color;
        }
        float a = startAlpha;
        float speed = (endAlpha - startAlpha) / (time);
        if (speed > 0)
        {
            while (a < endAlpha)
            {
                for (int i = 0; i < textList.Count; i++)
                {
                    defColorList[i].a = a;
                    textList[i].color = defColorList[i];
                }
                a += Time.unscaledDeltaTime * speed;
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (a > endAlpha)
            {
                for (int i = 0; i < textList.Count; i++)
                {
                    defColorList[i].a = a;
                    textList[i].color = defColorList[i];
                }
                a += Time.unscaledDeltaTime * speed;
                yield return new WaitForEndOfFrame();
            }
        }
        for (int i = 0; i < textList.Count; i++)
        {
            defColorList[i].a = endAlpha;
            textList[i].color = defColorList[i];
        }
        yield break;
    }

    #endregion
}
