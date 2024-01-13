using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UIObjectData
{
    public string name;
    public Vector2 position;
    public float rotation;
    public Vector2 scale;
    public Vector2 size;
    public List<UIComponentData> components;
    public List<UIObjectData> children;
}

[System.Serializable]
public class UIComponentData
{
    public string type;
    public string source;
    public string content;
    public int fontSize;
    public TextAnchor textAnchor;
    public Color color;
}
