using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TweenUIElement : MonoBehaviour {

    public static UITweeners UiTweeners = new UITweeners();
    [SerializeField] AnimationCurve scaleCurve;
    [SerializeField] Transform scalableSprite;
    [SerializeField] Image [] allImages;
    [SerializeField] float appearTime = 0.3f;
    [SerializeField] float disappearTime = 0.2f;
    [SerializeField] float defaultSpriteScale = 1;
    [SerializeField] float disableScale = 0.01f;
    [SerializeField] bool useStartAlpha = false;

    void Start()
    {
        //defaultSpriteScale = scalableSprite.localScale.x;
    }

    void OnEnable()
    {
        StopAllCoroutines();
        foreach (Image im in allImages)
        {
            UiTweeners.ImAlphaCrossFromValueToValue(this, im, 0, useStartAlpha? im.color.a : 1, appearTime / 2f);
        }
        SpriteTweeners.SpriteScaleViaCurve(this, scalableSprite, scaleCurve, defaultSpriteScale, appearTime);
    }

    public void EnableSelf()
    {
        StopAllCoroutines();
        gameObject.SetActive(true);
        StopAllCoroutines();
        foreach (Image im in allImages)
        {
            UiTweeners.ImAlphaCrossFromValueToValue(this, im, 0, 1, appearTime / 2f);
        }
        SpriteTweeners.SpriteScaleViaCurve(this, scalableSprite, scaleCurve, defaultSpriteScale, appearTime);
    }

    public void DisableSelf(bool disableGO = false)
    {
        if (!gameObject.activeInHierarchy)
            return;
        StopAllCoroutines();
        foreach (Image im in allImages)
        {
            UiTweeners.ImAlphaCrossFromValueToValue(this, im, im.color.a, 0, disappearTime);
        }
        SpriteTweeners.SpriteScaleCrossFromValueToValue(this, scalableSprite, scalableSprite.localScale.x, disableScale, disappearTime);
        if (disableGO)
            StartCoroutine(DisableProcess());
    }

    IEnumerator DisableProcess()
    {
        yield return new WaitForSeconds(disappearTime + 0.2f);
        gameObject.SetActive(false);
        yield break;
    }

}
