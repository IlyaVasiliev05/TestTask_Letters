using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppScreen: MonoBehaviour
{
    public LessonType type;
    public LessonData data;


    public virtual void OnScreenOpen(LessonData data = null)
    {
        this.data = data;
    }

    public virtual void OnScreenClose()
    {

    }

    public virtual void OnScreenCloseAndDestroy()
    {

    }

}

public enum LessonType { FindLettersLesson }