using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(DrawButton))]
public class DrawButtonEditor : Editor {

	public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawButton t = (DrawButton)target;
    }
}
