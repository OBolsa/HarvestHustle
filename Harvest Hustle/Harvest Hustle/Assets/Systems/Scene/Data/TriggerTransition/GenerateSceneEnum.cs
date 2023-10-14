using System.Collections.Generic;
using System.IO;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class GenerateSceneEnum : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Custom/Generate SceneNames")]
    public static void GenerateSceneEnumFile()
    {
        string scriptPath = EditorUtility.SaveFilePanel("Save Scene Enum", "Assets/Systems/Scene/Data/TriggerTransition", "GameScenesEnum.cs", "cs");

        if (!string.IsNullOrEmpty(scriptPath))
        {
            List<GameScene> sceneNames = FindObjectsOfType<GameScene>(true).ToList();

            using (StreamWriter writer = new StreamWriter(scriptPath))
            {
                writer.WriteLine("public enum GameScenesEnum");
                writer.WriteLine("{");

                for (int i = 0; i < sceneNames.Count; i++)
                {
                    string name = SanitizeEnumValue(sceneNames[i].SceneName);
                    writer.WriteLine($"    {name},");
                }

                writer.WriteLine("}");
            }

            AssetDatabase.Refresh();
        }
    }

    private static string SanitizeEnumValue(string input)
    {
        // Remove special characters and replace spaces with underscores
        string sanitizedInput = input.Replace(" ", "_");
        sanitizedInput = System.Text.RegularExpressions.Regex.Replace(sanitizedInput, @"[^A-Za-z0-9_]", "");

        // Ensure the result starts with a letter
        if (!char.IsLetter(sanitizedInput[0]))
        {
            sanitizedInput = "_" + sanitizedInput;
        }

        return sanitizedInput;
    }
#endif
}