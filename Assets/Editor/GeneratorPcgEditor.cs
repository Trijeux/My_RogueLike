using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GeneratorPcg))]
public class GeneratorPcgEditor : Editor
{ 
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var generatorPcg = (GeneratorPcg)target;

        if (GUILayout.Button("Generate"))
        {
            generatorPcg.StartGeneration();
        }
    }
}
