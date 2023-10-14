#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class TSVLoader
{
    public string objectNamePrefix;
    public string savePath;
    public string tsvUrl;  // URL of the Google Sheets web published TSV

    public IEnumerator Start()
    {
        UnityWebRequest www = UnityWebRequest.Get(tsvUrl);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error fetching TSV data: " + www.error);
        }
        else
        {
            string[] lines = www.downloadHandler.text.Split('\n');
            List<ItemData> itemDataList = new List<ItemData>();

            for (int i = 1; i < lines.Length; i++)  // Skip header line
            {
                string[] fields = lines[i].Split('\t');  // Assuming tab-separated values

                // Skip the line if the first element is blank
                if (fields.Length >= 1 && !string.IsNullOrWhiteSpace(fields[0]))
                {
                    ItemData newItem = CreateItemData(fields);
                    if (newItem != null)
                    {
                        itemDataList.Add(newItem);
                    }
                }
            }

            foreach (ItemData itemData in itemDataList)
            {
                string objectName = objectNamePrefix + itemData.itemName;
                string fullPath = Path.Combine(savePath, objectName + ".asset");

                ItemData existingItem = AssetDatabase.LoadAssetAtPath<ItemData>(fullPath);
                if (existingItem != null)
                {
                    // Update the existing Scriptable Object's properties if spreadsheet fields aren't blank
                    if (!string.IsNullOrWhiteSpace(itemData.itemDescription))
                        existingItem.itemDescription = itemData.itemDescription;
                    if (itemData.maxStack > 0)
                        existingItem.maxStack = itemData.maxStack;
                    if (itemData.itemType != ItemType.Generic)
                        existingItem.itemType = itemData.itemType;
                    if (itemData.toolType != ToolType.None)
                        existingItem.toolType = itemData.toolType;
                }
                else
                {
                    // Create and save a new Scriptable Object
                    AssetDatabase.CreateAsset(itemData, fullPath);
                }

                Debug.Log("Processed: " + fullPath);
            }

            // Refresh the Asset Database to ensure changes are saved
            AssetDatabase.Refresh();
        }
    }

    ItemData CreateItemData(string[] fields)
    {
        ItemData newItem = ScriptableObject.CreateInstance<ItemData>();

        newItem.itemName = fields[0];
        newItem.itemDescription = fields[1];
        newItem.maxStack = string.IsNullOrWhiteSpace(fields[2]) ? 0 : int.Parse(fields[2]);
        newItem.itemType = (ItemType)System.Enum.Parse(typeof(ItemType), fields[3]);
        newItem.toolType = (ToolType)System.Enum.Parse(typeof(ToolType), fields[4]);

        return newItem;
    }
}
#endif