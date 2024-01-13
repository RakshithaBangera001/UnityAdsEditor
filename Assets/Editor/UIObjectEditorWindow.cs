using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIObjectEditorWindow : EditorWindow
{
    SerializedObject serializedObject;
    SerializedProperty uiObjectDataProperty;
    public UIObjectData uiObjectData;
    private string newTemplateName = "NewTemplate";
    private Vector2 scrollPosition;

    private void OnEnable()
    {
        serializedObject = new SerializedObject(this);
        uiObjectDataProperty = serializedObject.FindProperty("uiObjectData");
    }

    [MenuItem("Window/UI Object Editor")]
    public static void ShowWindow()
    {
        GetWindow<UIObjectEditorWindow>("UI Object Editor");
    }

    private void OnGUI()
    {
        serializedObject.Update(); 
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        EditorGUILayout.PropertyField(uiObjectDataProperty, new GUIContent("UI Object Data"), true);
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.EndScrollView();
        if (GUILayout.Button("Load JSON"))
        {
            string jsonPath = EditorUtility.OpenFilePanel("Load JSON", "", "json");
            LoadJson(jsonPath);
        }

        if (GUILayout.Button("Create New Template"))
        {
            CreateNewTemplate();
        }

        if (uiObjectData != null)
        {
            if (GUILayout.Button("Save JSON"))
            {
                SaveToJson();
            }

            if (GUILayout.Button("Instantiate UI"))
            {
                InstantiateUI();
            }
        }
    }

    private void CreateNewTemplate()
    {
        uiObjectData = new UIObjectData
        {
            name = newTemplateName,
            position = Vector2.zero,
            rotation = 0f,
            scale = Vector2.one,
            components = new List<UIComponentData>(),
            children = new List<UIObjectData>()
        };
    }

    private void LoadJson(string jsonPath)
    {
        if (!string.IsNullOrEmpty(jsonPath))
        {
            string json = System.IO.File.ReadAllText(jsonPath);
            uiObjectData = JsonUtility.FromJson<UIObjectData>(json);
        }
    }

    private void SaveToJson()
    {
        if (uiObjectData == null)
        {
            Debug.LogError("UI Object Data is null. Nothing to save.");
            return;
        }

        string jsonPath = EditorUtility.SaveFilePanel("Save JSON", "", newTemplateName, "json");
        if (!string.IsNullOrEmpty(jsonPath))
        {
            string json = JsonUtility.ToJson(uiObjectData, true);
            System.IO.File.WriteAllText(jsonPath, json);
        }
    }

    private void InstantiateUI()
    {
        // Check if the UIObjectData is not null
        if (uiObjectData == null)
        {
            Debug.LogError("UI Object Data is null. Please load or create a template first.");
            return;
        }

        // Create a Canvas GameObject
        GameObject canvasObject = new GameObject("Canvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // Create an EventSystem GameObject if it doesn't exist
        if (!GameObject.FindObjectOfType<EventSystem>())
        {
            GameObject eventSystemObject = new GameObject("EventSystem");
            eventSystemObject.AddComponent<EventSystem>();
            eventSystemObject.AddComponent<StandaloneInputModule>();
        }

        // Instantiate the UI hierarchy recursively
        InstantiateUIObject(canvasObject.transform, uiObjectData);
    }

    private void InstantiateUIObject(Transform parent, UIObjectData uiObjectData)
    {
        GameObject uiObject = new GameObject(uiObjectData.name);
        RectTransform rectTransform = uiObject.AddComponent<RectTransform>();
        uiObject.transform.SetParent(parent);
        rectTransform.anchoredPosition = uiObjectData.position;
        rectTransform.localRotation = Quaternion.Euler(0f, 0f, uiObjectData.rotation);
        rectTransform.localScale = new Vector3(uiObjectData.scale.x, uiObjectData.scale.y, 1f);
        rectTransform.sizeDelta = uiObjectData.size;

        foreach (UIComponentData componentData in uiObjectData.components)
        {
            if (componentData.type == "Image")
            {
                Image imageComponent = uiObject.AddComponent<Image>();
                imageComponent.color = componentData.color;
            }
            else if (componentData.type == "Text")
            {
                Text textComponent = uiObject.AddComponent<Text>();
                textComponent.color = componentData.color;
                textComponent.fontSize = componentData.fontSize;
                textComponent.text = componentData.content;
                textComponent.alignment = componentData.textAnchor;
            }
        }

        foreach (UIObjectData childData in uiObjectData.children)
        {
            InstantiateUIObject(uiObject.transform, childData);
        }
    }

    private void ApplyChangesToInstantiatedUI()
    {
        if (uiObjectData == null)
        {
            Debug.LogError("UI Object Data is null. Please load or create a template first.");
            return;
        }

        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("No Canvas found in the scene. Instantiate UI first.");
            return;
        }

        ApplyChangesToUIObject(canvas.transform, uiObjectData);
    }

    private void ApplyChangesToUIObject(Transform uiObjectTransform, UIObjectData uiObjectData)
    {
        RectTransform rectTransform = uiObjectTransform.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = uiObjectData.position;
            rectTransform.localRotation = Quaternion.Euler(0f, 0f, uiObjectData.rotation);
            rectTransform.localScale = new Vector3(uiObjectData.scale.x, uiObjectData.scale.y, 1f);
        }

        for (int i = 0; i < uiObjectTransform.childCount && i < uiObjectData.children.Count; i++)
        {
            ApplyChangesToUIObject(uiObjectTransform.GetChild(i), uiObjectData.children[i]);
        }
    }
}
