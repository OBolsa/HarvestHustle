using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Dialogue"), System.Serializable]
public class Dialogue : ScriptableObject
{
    [Header("Information")]
    [TextArea(0, 8)] public string Description;

    // Dialogue Tracker
    private int ConversationIndex;
    private int ConversationLastIndex { get => Conversation.Count - 1; }

    [Header("Quest Requirement")]
    public bool RequiresQuest;
    public Quest QuestRequired;
    public Dialogue DontHaveTheQuestDialogue;

    [Header("Speeches")]
    public List<Speech> Conversation = new List<Speech>();
    private Speech CurrentSpeech { get => Conversation[ConversationIndex]; }

    public void Initialize()
    {
        // Se ele precisa de quest pra começar, então vamo ver se esse bananão tem a quest.
        if (RequiresQuest)
        {
            bool haveQuest = GameplayManager.instance.questManager.HasCompletedQuest(QuestRequired);

            // IH ALA, O CORNO N TEM A QUEST
            if (!haveQuest)
            {
                DontHaveTheQuestDialogue.ConversationIndex = 0;
                PlayerInputManager.PlayerInput.World.PrimaryButton.performed += DontHaveTheQuestDialogue.NextSpeech;
                DontHaveTheQuestDialogue.CurrentSpeech.IsLetteringDone = false;
                GameplayManager.instance.dialogueDisplayer.DisplaySpeech(DontHaveTheQuestDialogue.Conversation[DontHaveTheQuestDialogue.ConversationIndex]);
                return;
            }
        }

        // Ok, desculpe-me por duvidar de você. Você tem a quest. Vamo bater papo
        ConversationIndex = 0;
        PlayerInputManager.PlayerInput.World.PrimaryButton.performed += NextSpeech;
        CurrentSpeech.IsLetteringDone = false;
        GameplayManager.instance.dialogueDisplayer.DisplaySpeech(Conversation[ConversationIndex]);
    }

    public void NextSpeech(InputAction.CallbackContext context)
    {
        // Se já acabou de fazer letrinha
        if (CurrentSpeech.IsLetteringDone)
        {
            // Vamo pro próximo?
            ConversationIndex++;

            // Ih rapaz. N tem próximo...
            if (ConversationIndex > ConversationLastIndex)
            {
                // Então finaliza essa budega;
                ConversationIndex = ConversationLastIndex;
                //GiveReward();
                EndDialogue();
                return;
            }

            // Vamo começar a mostrar esse próximo diáligo?
            GameplayManager.instance.dialogueDisplayer.DisplaySpeech(CurrentSpeech);

            // Além disso, entrega as quests dele e as recompensas também. Passa tudo pro pai.
            GiveReward();
            return;
        }
        else // Se ainda ta fazendo as letrinhas
        {
            // Apenas finalize, irmão.
            GameplayManager.instance.dialogueDisplayer.EndSpeech();
        }
    }

    private void GiveReward()
    {
        // Ih mané, nem tem recompensa. kkkkk
        if (!CurrentSpeech.HaveRewards())
        {
            CurrentSpeech.StartQuests();
            CurrentSpeech.SetNextDialogue();
            return;
        }

        // Você tem o que é preciso para esmagares a minha rata?
        GameplayManager.instance.playerInventory.container.ItemAdded += OnCollectRewards;
        GameplayManager.instance.playerInventory.container.AddItem(CurrentSpeech.GetRewardList());
    }

    private void OnCollectRewards(bool collected)
    {
        // Eu tinha... Eu tinha o que era preciso para esmagar a rata dela...
        if (collected)
        {
            CurrentSpeech.StartQuests();
            CurrentSpeech.SetNextDialogue();
            GameplayManager.instance.playerInventory.container.ItemAdded -= OnCollectRewards;
        }
        else // Eu n tinha, então né... que merda.
        {
            // Mostrar aqui um diálogo default de que o jogador está sem espaço.
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        GameplayManager.instance.dialogueDisplayer.CloseDialogue();
        PlayerInputManager.PlayerInput.World.PrimaryButton.performed -= NextSpeech;
        //GameplayManager.instance.playerInventory.container.ItemAdded -= OnCollectRewards;
    }

    [System.Serializable]
    public class Speech
    {
        #region Structs
        [System.Serializable]
        public struct Stat
        {
            public ItemData Item;
            public int Amount;
        }
        [System.Serializable]
        public struct QuestInfo
        {
            public Quest StartQuest;
        }
        [System.Serializable]
        public struct DialogueInfo
        {
            public NPCData NPC;
            public Dialogue NextDialogue;

            public void SetNewDialogue() => NPC.CurrentDialogue = NextDialogue;
        }
        #endregion

        public NPCData Speaker;
        [TextArea(0, 10)] public string Text;
        public TextSpeedType TextSpeed = TextSpeedType.Medium;
        public bool IsLetteringDone { get; set; }

        public Stat[] RewardInfos;
        public QuestInfo[] QuestInfos;
        public DialogueInfo[] NextDialogueInfos;

        public List<ItemData> GetRewardList()
        {
            List<ItemData> rewards = new List<ItemData>();
            int count = 0;

            foreach (var reward in RewardInfos)
            {
                int quantity = reward.Amount;

                while (quantity > 0)
                {
                    rewards.Add(reward.Item);
                    count++;
                    quantity--;
                }
            }

            Debug.Log(count);
            return rewards;
        }

        public bool HaveRewards() => RewardInfos.Length > 0;
        public bool HaveQuests()
        {
            foreach (var info in QuestInfos)
            {
                if (info.StartQuest != null)
                {
                    return true;
                }
            }

            return false;
        }
        public bool HaveNextDialogues() => NextDialogueInfos.Length > 0;

        public void StartQuests()
        {
            if (!HaveQuests())
            {
                return;
            }

            foreach (var info in QuestInfos)
            {
                if (info.StartQuest != null)
                {
                    GameplayManager.instance.questManager.StartQuest(info.StartQuest);
                }
            }
        }

        public void SetNextDialogue()
        {
            if (!HaveNextDialogues()) return;

            foreach (var info in NextDialogueInfos)
            {
                info.SetNewDialogue();
            }
        }
    }
}

public enum TextSpeedType
{
    Slow,
    Medium,
    Fast
}