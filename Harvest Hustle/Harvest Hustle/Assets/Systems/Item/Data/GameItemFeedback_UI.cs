using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameItemFeedback_UI : MonoBehaviour
{
    public CanvasGroup elements;
    public Image itemIcon;
    public TMP_Text itemName;

    private List<ItemData> itemsToDisplay = new List<ItemData>();
    private bool displayingItem = false;

    private void Start()
    {
        EventManager.Instance.AddListener<ItemCollectGameEvent>(EnqueueItem);
    }

    private void EnqueueItem(ItemCollectGameEvent collectedItem)
    {
        itemsToDisplay.Add(collectedItem.Item);
        TryDisplayNextItem();
    }

    private void TryDisplayNextItem()
    {
        if (!displayingItem && itemsToDisplay.Count > 0)
        {
            ItemData nextItem = itemsToDisplay[0];
            itemsToDisplay.RemoveAt(0);
            DisplayItem(nextItem);
        }
    }

    private void DisplayItem(ItemData item)
    {
        displayingItem = true;

        itemIcon.sprite = item.itemIcon;
        itemName.text = item.itemName;

        DOTween.To(() => elements.alpha, x => elements.alpha = x, 1f, 0.15f).OnComplete(() =>
            {
                DOVirtual.DelayedCall(1.2f, () =>
                {
                    DOTween.To(() => elements.alpha, x => elements.alpha = x, 0f, 0.15f).OnComplete(() =>
                        {
                            displayingItem = false;
                            TryDisplayNextItem();
                        });
                });
            });
    }
}
