using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenSprite : MonoBehaviour {

    [SerializeField] AnimationCurve scaleCurve;
    [SerializeField] Transform scalableSprite;
    [SerializeField] SpriteRenderer[] allSprites;
    [SerializeField] float appearTime = 0.3f;
    [SerializeField] float disappearTime = 0.2f;
    [SerializeField] float defaultSpriteScale = 1;
    [SerializeField] float disableScale = 0.01f;

    void Start()
    {
        //defaultSpriteScale = scalableSprite.localScale.x;
    }

    void OnEnable()
    {
        StopAllCoroutines();
        foreach (SpriteRenderer sprt in allSprites)
        {
            SpriteTweeners.SpriteAlphaCrossFromValueToValue(this, sprt, 0, 1, appearTime /2f);
        }
        SpriteTweeners.SpriteScaleViaCurve(this, scalableSprite, scaleCurve, defaultSpriteScale, appearTime);
    }

    public void EnableSelf()
    {
        StopAllCoroutines();
        gameObject.SetActive(true);
        StopAllCoroutines();
        foreach (SpriteRenderer sprt in allSprites)
        {
            SpriteTweeners.SpriteAlphaCrossFromValueToValue(this, sprt, 0, 1, appearTime / 2f);
        }
        SpriteTweeners.SpriteScaleViaCurve(this, scalableSprite, scaleCurve, defaultSpriteScale, appearTime);
    }

    public void DisableSelf(bool disableGO = false)
    {
        if (!gameObject.activeInHierarchy)
            return;
        StopAllCoroutines();
        foreach (SpriteRenderer sprt in allSprites)
        {
            SpriteTweeners.SpriteAlphaCrossFromValueToValue(this, sprt, 1, 0, disappearTime);
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
