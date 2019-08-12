using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : MonoBehaviour
{
    public int id;
    public TextMesh textMesh;
    [SerializeField] Animator anim;
    FindLettersLesson myLesson;
    public Letter parentLetter;
    public bool interactable = false;

    public void Init(FindLettersLesson lesson, int id, char letter, Font font, bool upper)
    {
        this.id = id;
        textMesh.text = upper ? letter.ToString().ToUpper() : letter.ToString().ToLower();
        textMesh.font = font;
        textMesh.GetComponent<MeshRenderer>().material = font.material;
        myLesson = lesson;
        textMesh.gameObject.SetActive(false);
        textMesh.gameObject.SetActive(true);
    }

    public void SetParentLetter(Letter l)
    {
        parentLetter = l;
        this.id = l.id;
        this.textMesh.text = l.textMesh.text;
        textMesh.font = l.textMesh.font;
        textMesh.GetComponent<MeshRenderer>().material = textMesh.font.material;
        textMesh.color = l.textMesh.color;
    }


    public enum LetterAnimState { Appear, Click}
    public void SetAnimationState(LetterAnimState state)
    {
        switch (state)
        {
            case LetterAnimState.Appear:
                anim.SetTrigger("Appear");
                break;
            case LetterAnimState.Click:
                anim.SetTrigger("Click");
                break;
        }
    }

    public void SetDrawOrder(string sortingLayer, int id)
    {
        textMesh.GetComponent<MeshRenderer>().sortingLayerName = sortingLayer;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = id;
    }

    public void OnClickLetter()
    {
        if (!interactable)
            return;
        myLesson.OnClickOnLetter(this);
    }

    Coroutine colorRoutine = null;
    public void SetColor(Color newColor, float time, float delay = 0)
    {
        if (colorRoutine != null)
            StopCoroutine(colorRoutine);

        colorRoutine = StartCoroutine(SetColorProcess(newColor, time, delay));
    }

    public void ColorOnFail()
    {
        StartCoroutine(SetColorProcess(Color.black, 0.2f, 0));
        StartCoroutine(SetColorProcess(Color.gray, 0.1f, 0.21f));
        StartCoroutine(SetColorProcess(Color.black, 0.1f, 0.32f));
        StartCoroutine(SetColorProcess(Color.gray, 0.1f, 0.43f));
        StartCoroutine(SetColorProcess(Color.black, 0.1f, 0.55f));
        StartCoroutine(SetColorProcess(Color.gray, 0.1f, 0.66f));
    }

    IEnumerator SetColorProcess(Color c, float time, float delay)
    {
        if(delay > 0.001f)
        yield return new WaitForSeconds(delay);
        Color defColor = textMesh.color;
        float t = 0;
        float speed = 1f / time;
        while (t < 1)
        {
            textMesh.color = Color.Lerp(defColor, c, t);
            t += Time.deltaTime * speed;
            yield return null;
        }
        textMesh.color = c;
        yield break;
    }

}
