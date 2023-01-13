using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SampleSceneControl : MonoBehaviour
{
    private int clickCount;

    private static readonly List<string> sceneNames = new()
    {
        "SampleScene",
        "SampleSceneWithoutCustomEventSystem",
    };

    private void Start()
    {
        UIDocument uiDocument = FindObjectOfType<UIDocument>();
        Button countButton = uiDocument.rootVisualElement.Q<Button>("countButton");
        Button resetButton = uiDocument.rootVisualElement.Q<Button>("resetButton");
        Button changeSceneButton = uiDocument.rootVisualElement.Q<Button>("changeSceneButton");

        RegisterCallbackButtonTriggered(countButton, () =>
        {
            clickCount++;
            countButton.text = "Click Count: " + clickCount;
        });
        RegisterCallbackButtonTriggered(resetButton, () =>
        {
            clickCount = 0;
            countButton.text = "Click Me";
        });

        changeSceneButton.text = "Go to scene\n" + GetNextSceneName();
        RegisterCallbackButtonTriggered(changeSceneButton, () =>
        {
            SceneManager.LoadScene(GetNextSceneName());
        });
    }

    private static void RegisterCallbackButtonTriggered(Button button, Action action)
    {
        // Neither event works when Samsung DeX (Desktop Experience) is active.
        button.RegisterCallback<ClickEvent>(evt => action());
        button.RegisterCallback<NavigationSubmitEvent>(evt => action());
    }

    private static int GetNextSceneNameIndex()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        int currentSceneNameIndex = sceneNames.IndexOf(currentSceneName);
        if (currentSceneNameIndex + 1 < sceneNames.Count)
        {
            return currentSceneNameIndex + 1;
        }
        else
        {
            return 0;
        }
    }

    private static string GetNextSceneName()
    {
        return sceneNames[GetNextSceneNameIndex()];
    }
}