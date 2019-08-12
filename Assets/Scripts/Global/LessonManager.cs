using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LessonManager : MonoBehaviour
{

    #region Lazy unity singleton

    public static LessonManager Instance
    {
        get
        {
            return instance;
        }
    }

    private static LessonManager instance;

    #endregion

    #region MonoBehaviour

    [SerializeField] Button returnButton;
    [SerializeField] Canvas mainCanvas;

    [Header("Lessons pool")]
    [SerializeField] List<AppScreen> lessonsPool;

    [Header("First window")]
    [SerializeField] AppScreen firstWindow;

    void Awake()
    {
        instance = this;
        windows = new List<AppScreen>();
    }

    void Start()
    {
        OpenNewWindow(firstWindow);
        returnButton.onClick.AddListener(CloseWindowAndOpenPrevious);
    }

    #endregion

    #region Windows

    List<AppScreen> windows;
    AppScreen currentLesson;

    public void OpenNewWindow(AppScreen window)
    {
        if (windows.Count > 0)
            windows[windows.Count - 1].OnScreenClose();
        window = Instantiate(window.gameObject, mainCanvas.transform).GetComponent<AppScreen>();
        windows.Add(window);
        window.transform.SetAsLastSibling();
        window.OnScreenOpen();
        ManageBackButton();
    }

    public void CloseWindowAndOpenPrevious()
    {
        if (currentLesson != null)
        {
            CloseCurrentLesson();
        }
        else
        {
            windows[windows.Count - 1].OnScreenCloseAndDestroy();
            windows.Remove(windows[windows.Count - 1]);
        }
        windows[windows.Count - 1].OnScreenOpen();
        ManageBackButton();
    }

    void ManageBackButton()
    {
        if (windows.Count > 1 && !returnButton.gameObject.activeSelf || currentLesson != null)
        {
            returnButton.GetComponent<TweenUIElement>().EnableSelf();
        }
        if (windows.Count <= 1 && returnButton.gameObject.activeSelf && currentLesson == null)
        {
            returnButton.GetComponent<TweenUIElement>().DisableSelf(true);
        }
    }

    #endregion

    #region Lessons

    public void OpenNewLesson(LessonData data)
    {
        windows[windows.Count - 1].OnScreenClose();
        currentLesson = Instantiate(GetLessonPrefabByType(data.type), Vector3.zero, Quaternion.identity).GetComponent<AppScreen>();
        currentLesson.OnScreenOpen(data);
    }

    public void CloseCurrentLesson()
    {
        currentLesson.OnScreenClose();
        currentLesson = null;
    }

    GameObject GetLessonPrefabByType(LessonType type)
    {
        foreach (AppScreen l in lessonsPool)
        {
            if (l.type == type)
                return l.gameObject;
        }
        return null;
    }

    #endregion

}
