using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DonationManager : MonoBehaviour
{
    // Packages Manager
    public Dictionary<string, int> packages = new Dictionary<string, int>();

    [Header("Display Components")]
    public TMP_Text donationDisplay;
    private int donationCurrentAmount = 0;

    private void Start()
    {
        donationDisplay.text = 0.ToString();
    }

    public void RegisterPackage(string packageId, int packagePoints)
    {
        packages.Add(packageId, packagePoints);
        Debug.Log($"Package <{packageId}> registred with <{packagePoints}> points");

        foreach (KeyValuePair<string, int> entry in packages)
        {
            Debug.Log($"Package: <{entry.Key}> // Value: <{entry.Value}>");
        }
    }

    public void ComputePackage(string packageId)
    {
        int packagePoints = 0;

        if(packages.ContainsKey(packageId))
        {
            Debug.Log("true");
            packagePoints = packages[packageId];
        }
        else
        {
            Debug.Log("false");
        }

        donationCurrentAmount += packagePoints;
        donationDisplay.text = donationCurrentAmount.ToString();

        packages.Remove(packageId);
    }
}