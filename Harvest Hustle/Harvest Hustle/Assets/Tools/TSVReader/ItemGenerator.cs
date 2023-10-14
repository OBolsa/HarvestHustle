#if UNITY_EDITOR
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    public TSVLoader[] ItemsToGenerate;

    [ContextMenu("Create Items")]
    public void CreateItems()
    {
        foreach(var item in ItemsToGenerate)
        {
            StartCoroutine(item.Start());
        }
    }
}
#endif