using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindLettersLesson : AppScreen
{

    #region serialized fields

    [Header("All letters data")]
    [SerializeField] AlphabetData alphabetData;

    [SerializeField] Letter[] lettersOnTopObjects;
    [SerializeField] bool debugStart = false;
    [SerializeField] GameObject letterPrefab;
    [SerializeField] float borderOffset = 1;
    [SerializeField] float radius = 1;
    [SerializeField] List<Font> fonts;
    [SerializeField] float defaultLocalScaleForLetter = 0.8f;
    [SerializeField] AnimationCurve moveCurve1;
    [SerializeField] AnimationCurve moveCurve2;
    [SerializeField] AnimationCurve moveCurve3;
    [SerializeField] float flyTime = 1;
    [SerializeField] string layerName = "Main";

    [Header("Audio")]
    [SerializeField] AudioClip taskAC;
    [SerializeField] AudioClip[] correctVoice;
    [SerializeField] AudioClip[] incorrectVoiceSequence;
    [SerializeField] AudioClip winVoice;

    [Header("Effects")]
    [SerializeField] ParticleSystem winStarsEffect;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        if (debugStart)
            OnScreenOpen(new FindLettersData(0));
    }

    #endregion


    FindLettersData myData => (FindLettersData)data;
    bool lockTouchLetters = false;
    LetterData currentLeterData;
    int correctLetterID;
    int curLettersPointsCount;
    List<Vector2> points;
    List<int> correctLetters;
    int completedLettersCount = 0;

    List<Letter> currentLetters;


    public override void OnScreenOpen(LessonData data)
    {
        base.OnScreenOpen(data);
        lockTouchLetters = true;
        correctLetterID = myData.letterID;
        currentLeterData = alphabetData.GetLetterByID(correctLetterID);
        completedLettersCount = 0;

        StartCoroutine(TutorPartProcess());
    }

    IEnumerator TutorPartProcess()
    {
        for (int i =0; i < lettersOnTopObjects.Length; i++)
        {
            TextMesh tm = lettersOnTopObjects[i].GetComponentInChildren<TextMesh>();
            tm.color = Color.red;
            tm.text = currentLeterData.letter.ToString();
            if (i % 2 == 1)
                tm.text = tm.text.ToLower();
            lettersOnTopObjects[i].gameObject.SetActive(false);
            lettersOnTopObjects[i].SetDrawOrder(layerName, 0);
            lettersOnTopObjects[i].id = correctLetterID;
        }
        yield return null;
        AudioManager.PlayVoiceOneShot(taskAC, true);
        foreach (Letter l in lettersOnTopObjects)
        {
            l.gameObject.SetActive(true);
            l.SetAnimationState(Letter.LetterAnimState.Appear);
            yield return new WaitForSeconds(0.2f);
        }
        currentLetters = new List<Letter>();

        //generating points
        Bounds bounds = new Bounds();
        Vector3 tempPos;
        tempPos = Camera.main.ScreenToWorldPoint(Screen.height * Vector3.up + Screen.width * Vector3.right);
        Vector3 max = new Vector3(tempPos.x - borderOffset, lettersOnTopObjects[0].transform.position.y - borderOffset - 0.7f, 0);
        tempPos = Camera.main.ScreenToWorldPoint(Vector2.zero);
        Vector3 min = new Vector3(tempPos.x + borderOffset, tempPos.y + borderOffset, 0);
        bounds.SetMinMax(min, max);

        points = null;
        GeneratePoints(ref points, curLettersPointsCount, bounds);
        
        //generation ended

        for(int i =0; i < points.Count; i++) // spawn all letters on positions
        {
            Letter newLetter = Instantiate(letterPrefab, points[i], Quaternion.identity).GetComponent<Letter>();
            currentLetters.Add(newLetter);
            int letterID = 0;
            do { letterID = Random.Range(0, alphabetData.lettersList.Count); } while (letterID == correctLetterID);

            newLetter.Init(this, letterID, alphabetData.GetLetterByID(letterID).letter, 
                fonts[Random.Range(0, fonts.Count)], Random.Range(0, 2) == 0);
            newLetter.transform.localScale = Vector3.one * defaultLocalScaleForLetter;
            newLetter.SetDrawOrder(layerName, 0);
            newLetter.interactable = true;
            newLetter.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(.3f);

        correctLetters = new List<int>();

        for (int i = 0; i < lettersOnTopObjects.Length; i++) // generating Correct Letters and moving them to start positions
        {
            int indexForCorrectLetter;
            do
            {
                indexForCorrectLetter = Random.Range(0, currentLetters.Count);
            } while (correctLetters.Contains(indexForCorrectLetter));
            correctLetters.Add(indexForCorrectLetter);
            Letter l = currentLetters[indexForCorrectLetter];
            l.SetParentLetter(lettersOnTopObjects[i]);
            l.gameObject.SetActive(true);
            l.transform.position = lettersOnTopObjects[i].transform.position;
        }
        yield return null;

        for (int i = 0; i < correctLetters.Count; i++) //smoothly move letters to positions
        {
            lettersOnTopObjects[i].SetColor(Color.white, 0);
            currentLetters[correctLetters[i]].SetDrawOrder(layerName, 1);
            SpriteTweeners.SpriteMoveToPointViaCurve(this, currentLetters[correctLetters[i]].transform, points[correctLetters[i]], moveCurve1, flyTime, false);
            currentLetters[correctLetters[i]].SetColor(Color.gray, flyTime);    
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(0.8f);
        for (int i = 0; i < currentLetters.Count; i++) // other letters appear
        {
            if (currentLetters[i].id != correctLetterID)
            {
                currentLetters[i].gameObject.SetActive(true);
                currentLetters[i].SetColor(Color.gray, 0);
                currentLetters[i].SetAnimationState(Letter.LetterAnimState.Appear);
            }
        }
        MixAllPoints();
        yield return new WaitForSeconds(1);
        for (int i = 0; i < currentLetters.Count; i++) // mix letters
        {
            SpriteTweeners.SpriteMoveToPointViaCurve(this, currentLetters[i].transform, points[i], moveCurve2, 0.7f);
        }

        lockTouchLetters = false;
        yield break;
    }

    void MixAllPoints()
    {
        for (int i = 0; i < points.Count; i++) // mix array of points
        {
            int rndIndex = Random.Range(0, points.Count);
            Vector2 tmp = points[i];
            points[i] = points[rndIndex];
            points[rndIndex] = tmp;
        }
    }

    void MirrorPoints()
    {
        bool mirrorX = Random.Range(0, 2) == 0;
        for (int i = 0; i < points.Count; i++) // mirror array of points
        {
            points[i] = new Vector2(mirrorX ? -points[i].x : points[i].x, points[i].y);
        }
    }

    void GeneratePoints(ref List<Vector2> points, int pointsNumber, Bounds bounds)
    {
        List<Vector2> tempPoints = new List<Vector2>();
        points = new List<Vector2>();
        int iterator = 35;
        for (int i = 0; i < iterator; i++)
        {
            for (int j = 0; j < iterator; j++)
            {
                Vector2 newPoint;
                newPoint = new Vector2(Mathf.Lerp(bounds.min.x, bounds.max.x, i/(float)iterator), Mathf.Lerp(bounds.min.y, bounds.max.y, j / (float)iterator));
                Debug.DrawLine(newPoint, newPoint + Vector2.up * 0.1f, Color.red, 25);
                tempPoints.Add(newPoint);
            }
        }

        for (int i = 0; i < tempPoints.Count; i++) // mix array of points
        {
            int rndIndex = Random.Range(0, tempPoints.Count);
            Vector2 tmp = tempPoints[i];
            tempPoints[i] = tempPoints[rndIndex];
            tempPoints[rndIndex] = tmp;
        }

        for (int i = 0; i < 1000; i++) // find points and its max count
        {
            Vector2 newPoint;
            bool pointIsFit = false;
            int p = 0;//i==0? Random.Range(0,tempPoints.Count) : 0; // index of point to try
            do
            {
                newPoint = tempPoints[p];
                p++;
                pointIsFit = true;
                foreach (Vector2 point in points)
                {
                    if ((point - newPoint).sqrMagnitude < radius * radius)
                    {
                        pointIsFit = false;
                        break;
                    }
                }
                if (p >= tempPoints.Count)
                {
                    curLettersPointsCount = i;
                    Debug.Log(curLettersPointsCount);
                    return;
                }
            } while (!pointIsFit);
            points.Add(newPoint);
        }
    }

    Coroutine playCorrectLetterRoutine;
    public void OnClickOnLetter(Letter l)
    {
        if (lockTouchLetters)
            return;
        bool correct = l.id == correctLetterID;
        l.SetAnimationState(Letter.LetterAnimState.Click);
        if (correct)
        {
            completedLettersCount++;
            SpriteTweeners.SpriteMoveToPointViaCurve(this, l.transform, l.parentLetter.transform.position, moveCurve2, flyTime);
            l.SetColor(Color.red, flyTime);
            l.interactable = false;
            if (completedLettersCount == lettersOnTopObjects.Length)
                OnWin();
        }
        else
        {
            StopAllCoroutines();
            l.ColorOnFail();
            StartCoroutine(RestartLevelOnFail());
        }
        if (playCorrectLetterRoutine != null)
            StopCoroutine(playCorrectLetterRoutine);
        playCorrectLetterRoutine = StartCoroutine(PlayCorrectLetter(alphabetData.GetLetterByID(l.id).ac, completedLettersCount, correct));
    }

    IEnumerator PlayCorrectLetter(AudioClip ac, int completedLettersCount, bool correct)
    {
        AudioManager.PlayVoiceOneShot(ac);
        yield return new WaitForSeconds(ac.length);
        if (correct)
            AudioManager.PlayVoiceOneShot(correctVoice[completedLettersCount == 1 ? 0 : Random.Range(0, correctVoice.Length)], true);
        else
        {
            AudioManager.PlayVoiceOneShot(incorrectVoiceSequence[0]);
            yield return new WaitForSeconds(incorrectVoiceSequence[0].length);
            AudioManager.PlayVoiceOneShot(incorrectVoiceSequence[1]);
        }
        yield break;
    }

    IEnumerator RestartLevelOnFail()
    {
        lockTouchLetters = true;
        completedLettersCount = 0;
        for (int i = 0; i < correctLetters.Count; i++)
        {
            currentLetters[correctLetters[i]].interactable = true;
            currentLetters[correctLetters[i]].SetColor(Color.gray, flyTime);
            SpriteTweeners.SpriteMoveToPointViaCurve(this, currentLetters[correctLetters[i]].transform, points[correctLetters[i]], moveCurve2, flyTime);
        }
        yield return new WaitForSeconds(1.2f);
        MixAllPoints();
        MirrorPoints();
        for (int i = 0; i < currentLetters.Count; i++) //smoothly move letters to positions
        {
            SpriteTweeners.SpriteMoveToPointViaCurve(this, currentLetters[i].transform, points[i], moveCurve3, flyTime, false);
        }
        yield return new WaitForSeconds(flyTime + 2);
        lockTouchLetters = false;

        yield break;
    }

    void OnWin()
    {
        StartCoroutine(WinProcess());
    }

    IEnumerator WinProcess()
    {
        lockTouchLetters = true;
        yield return new WaitForSeconds(1);
        winStarsEffect.Play();
        AudioManager.PlayVoiceOneShot(winVoice);
        yield return new WaitForSeconds(winVoice.length);
        LessonManager.Instance.CloseWindowAndOpenPrevious();
        yield break;
    }

    #region Close operations

    public override void OnScreenClose()
    {
        base.OnScreenClose();
        AudioManager.StopVoice();
        StopAllCoroutines();
        StartCoroutine(CloseProcess());
    }

    IEnumerator CloseProcess()
    {
        foreach (Letter l in lettersOnTopObjects)
        {
            SpriteTweeners.SpriteScaleCrossFromValueToValue(this, l.transform, 1, 0, 0.12f);
        }
        if (currentLetters != null)
            foreach (Letter l in currentLetters)
            {
                if (l != null)
                    SpriteTweeners.SpriteScaleCrossFromValueToValue(this, l.transform, l.transform.localScale.x, 0, 0.2f);
            }
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    #endregion

}
