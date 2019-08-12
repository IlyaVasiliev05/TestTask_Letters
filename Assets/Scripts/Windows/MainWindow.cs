using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainWindow : AppScreen
{
    [SerializeField] Button openLettersMenuButton;
    [SerializeField] AppScreen lettersWindow;

    void Start()
    {
        openLettersMenuButton.onClick.AddListener(OpenLettersMenu);
    }

    public override void OnScreenOpen(LessonData data = null)
    {
        base.OnScreenOpen(data);
        gameObject.SetActive(true);
        openLettersMenuButton.enabled = true;
        SpriteTweeners.SpriteScaleCrossFromValueToValue(this, openLettersMenuButton.transform, 0, 1, 0.2f);
    }

    public override void OnScreenCloseAndDestroy()
    {
        base.OnScreenCloseAndDestroy();
        Destroy(gameObject);
    }

    public override void OnScreenClose()
    {
        base.OnScreenClose();
        openLettersMenuButton.enabled = false;
        SpriteTweeners.SpriteScaleCrossFromValueToValue(this, openLettersMenuButton.transform, 1, 0, 0.2f);
    }

    void OpenLettersMenu()
    {
        LessonManager.Instance.OpenNewWindow(lettersWindow);
    }



}
