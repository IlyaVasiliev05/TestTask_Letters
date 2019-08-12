using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LettersWindow : AppScreen
{

    [SerializeField] AlphabetData alphabet;
    [SerializeField] GameObject letterPrefab;
    [SerializeField] Transform lettersCenter;
    [SerializeField] Transform alphabetText;

    [SerializeField] AnimationCurve appearCurve;
    [SerializeField] AnimationCurve disappearCurve;
    [SerializeField] float letterDefaultLocalScale = 0.7f;
    [SerializeField] int lettersInRow;
    [SerializeField] int lettersSpacing;

    List<GameObject> currentLetters = new List<GameObject>();
    Vector3 defaultPosForText;
    bool isClosed = false;

    public override void OnScreenOpen(LessonData data = null)
    {
        base.OnScreenOpen();
        isClosed = false;
        if (currentLetters == null || currentLetters.Count == 0)
        {
            Init();
        }

        StopAllCoroutines();
        StartCoroutine(OpenWindowProcess());
    }

    void Init()
    {
        currentLetters = new List<GameObject>();
        for (int i = 0; i < alphabet.lettersList.Count; i++)
        {
            GameObject newLetter = Instantiate(letterPrefab, lettersCenter);
            currentLetters.Add(newLetter);
            newLetter.transform.localPosition = new Vector3((i % lettersInRow - lettersInRow / 2) * lettersSpacing,
                ((alphabet.lettersList.Count / lettersInRow) / 2f - i / lettersInRow) * lettersSpacing, 0);
            newLetter.GetComponentInChildren<Text>().text = alphabet.GetLetterByID(i).letter.ToString();
            int a = i;
            newLetter.GetComponent<Button>().onClick.AddListener(() => { if (!isClosed) LessonManager.Instance.OpenNewLesson(new FindLettersData(a)); }); //add a listener to button
        }
    }

    public override void OnScreenClose()
    {
        base.OnScreenClose();
        isClosed = true;
        StopAllCoroutines();
        StartCoroutine(CloseWindowProcess());
    }

    public override void OnScreenCloseAndDestroy()
    {
        base.OnScreenCloseAndDestroy();
        StopAllCoroutines();
        StartCoroutine(CloseWindowProcess(true));
    }

    #region coroutines

    public IEnumerator OpenWindowProcess()
    {
        foreach (GameObject go in currentLetters)
        {
            go.transform.localScale = Vector3.zero;
        }
        SpriteTweeners.SpriteScaleViaCurve(this, alphabetText.transform, appearCurve, 1, 0.25f);
        foreach (GameObject go in currentLetters)
        {
            SpriteTweeners.SpriteScaleCrossFromValueToValue(this, go.transform, go.transform.localScale, letterDefaultLocalScale * Vector3.one, 0.22f);
            yield return new WaitForSeconds(0.02f);
        }
        yield break;
    }

    public IEnumerator CloseWindowProcess(bool destroy = false)
    {
        SpriteTweeners.SpriteScaleViaCurve(this, alphabetText.transform, disappearCurve, 1, 0.2f);
        foreach (GameObject go in currentLetters)
        {
            SpriteTweeners.SpriteScaleCrossFromValueToValue(this, go.transform, go.transform.localScale, Vector3.zero, 0.1f);
            //yield return new WaitForSeconds(0.001f);
        }
        if (destroy)
            Destroy(gameObject, 0.3f);
        yield break;
    }

    #endregion
}
