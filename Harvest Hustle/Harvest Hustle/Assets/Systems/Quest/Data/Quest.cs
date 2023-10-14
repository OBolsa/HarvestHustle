using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Quest")]
public class Quest : ScriptableObject
{
    [System.Serializable]
    public struct Info
    {
        public string Title, Description;
    }
    [System.Serializable]
    public struct State
    {
        public int Contribuition;
        public ItemData[] Items;
        public Quest[] Quests;
    }

    [Header("Info")]
    public Info Information;
    [Header("Reward")]
    public State Reward;
    public bool HaveItemReward { get => Reward.Items.Length > 0; }

    public bool Completed { get; private set; }
    public QuestCompletedEvent QuestCompleted;

    public List<QuestGoal> Goals = new List<QuestGoal>();

    private void OnDisable()
    {
        ClearQuest();
    }

    [ContextMenu("Clear Quest")]
    void ClearQuest()
    {
        foreach (var goal in Goals)
        {
            goal.ResetGoal();
        }
    }

    public void Start()
    {
        //GameplayManager.instance.questManager.StartQuest(this);
    }

    public void Start(Action<Quest> onQuestComplete)
    {
        //GameplayManager.instance.questManager.StartQuest(this, onQuestComplete);
    }

    public void Initialize()
    {
        Completed = false;
        QuestCompleted = new QuestCompletedEvent();

        foreach (var goal in Goals)
        {
            goal.Initialize();
            goal.GoalCompleted.AddListener(delegate { CheckGoals(); });
        }

        GameplayManager.instance.questManager.OnStartQuest?.Invoke(this);
    }

    private void CheckGoals()
    {
        Completed = Goals.All(g => g.Completed);

        if (Completed)
        {
            // Give RewardInfos
            GiveReward();
            QuestCompleted.Invoke(this);
            QuestCompleted.RemoveAllListeners();
        }
        else
        {
        }
    }

    private void GiveReward()
    {
        // Give Contribuition

        // Give Items
        if(Reward.Items.Length > 0)
        {
            List<ItemData> itemsToGive = new List<ItemData>();
            itemsToGive = Reward.Items.ToList();
            if(HaveItemReward)
                GameplayManager.instance.playerInventory.container.ItemAdded += OnCollectItem;
            GameplayManager.instance.playerInventory.container.AddItem(itemsToGive);
        }

        // Give Quest
        if(Reward.Quests.Length > 0)
        {
            List<Quest> questsToStart = new List<Quest>();
            questsToStart = Reward.Quests.ToList();

            foreach (var quest in questsToStart)
            {
                GameplayManager.instance.questManager.StartQuest(quest);
            }
        }
    }

    public void OnCollectItem(bool collected)
    {
        if (collected)
        {
            GameplayManager.instance.playerInventory.container.ItemAdded -= OnCollectItem;
        }
    }

    [System.Serializable]
    public class QuestGoal : ScriptableObject
    {
        protected string Description;
        public int CurrentAmount { get; protected set; }
        public int RequiredAmount;
        public string CustomDescription;

        public bool Completed { get; protected set; }
        [HideInInspector] public UnityEvent GoalCompleted;

        public void ResetGoal()
        {
            CurrentAmount = 0;
        }

        public virtual string GetDescription()
        {
            return Description;
        }

        public virtual void Initialize()
        {
            Completed = false;
            GoalCompleted = new UnityEvent();
        }

        protected void Evaluate()
        {
            if(CurrentAmount >= RequiredAmount)
            {
                Complete();
            }

            GameplayManager.instance.questManager.OnUpdateQuest?.Invoke(this);
        }

        private void Complete()
        {
            Completed = true;
            GoalCompleted.Invoke();
            GoalCompleted.RemoveAllListeners();
        }

        public void Skip()
        {
            //
        }
    }
}

public class QuestCompletedEvent : UnityEvent<Quest> { }

#if UNITY_EDITOR
[CustomEditor(typeof(Quest))]
public class QuestEditor : Editor
{
    SerializedProperty m_QuestInfoProperty;
    SerializedProperty m_QuestStatProperty;

    List<string> m_QuestGoalType;
    SerializedProperty m_QuestGoalListProperty;

    [MenuItem("Assets/Quest", priority = 0)]
    public static void CreateQuest()
    {
        var newQuest = CreateInstance<Quest>();

        ProjectWindowUtil.CreateAsset(newQuest, "quest.asset");
    }

    private void OnEnable()
    {
        m_QuestInfoProperty = serializedObject.FindProperty(nameof(Quest.Information));
        m_QuestStatProperty = serializedObject.FindProperty(nameof(Quest.Reward));

        m_QuestGoalListProperty = serializedObject.FindProperty(nameof(Quest.Goals));

        var lookup = typeof(Quest.QuestGoal);
        m_QuestGoalType = System.AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(lookup))
            .Select(type => type.Name)
            .ToList();
    }

    public override void OnInspectorGUI()
    {
        var child = m_QuestInfoProperty.Copy();
        var depth = child.depth;
        child.NextVisible(true);

        EditorGUILayout.LabelField("Quest info", EditorStyles.boldLabel);
        while(child.depth > depth)
        {
            EditorGUILayout.PropertyField(child, true);
            child.NextVisible(false);
        }

        child = m_QuestStatProperty.Copy();
        depth = child.depth;
        child.NextVisible(true);

        EditorGUILayout.LabelField("Quest Reward", EditorStyles.boldLabel);
        while (child.depth > depth)
        {
            EditorGUILayout.PropertyField(child, true);
            child.NextVisible(false);
        }

        int choice = EditorGUILayout.Popup("Add new Quest Goal", -1, m_QuestGoalType.ToArray());

        if(choice != -1)
        {
            var newInstance = ScriptableObject.CreateInstance(m_QuestGoalType[choice]);

            AssetDatabase.AddObjectToAsset(newInstance, target);

            m_QuestGoalListProperty.InsertArrayElementAtIndex(m_QuestGoalListProperty.arraySize);
            m_QuestGoalListProperty.GetArrayElementAtIndex(m_QuestGoalListProperty.arraySize -1)
                .objectReferenceValue = newInstance;
        }

        Editor ed = null;
        int toDelete = -1;
        for (int i = 0; i < m_QuestGoalListProperty.arraySize; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            var item = m_QuestGoalListProperty.GetArrayElementAtIndex(i);
            SerializedObject obj = new SerializedObject(item.objectReferenceValue);

            Editor.CreateCachedEditor(item.objectReferenceValue, null, ref ed);

            ed.OnInspectorGUI();
            EditorGUILayout.EndVertical();

            if(GUILayout.Button("-", GUILayout.Width(32)))
            {
                toDelete = i;
            }
            EditorGUILayout.EndHorizontal();
        }

        if(toDelete != -1)
        {
            var item = m_QuestGoalListProperty.GetArrayElementAtIndex(toDelete).objectReferenceValue;
            DestroyImmediate(item, true);

            // need to do it twice, first time just nullify the entry, second actually remove it
            m_QuestGoalListProperty.DeleteArrayElementAtIndex(toDelete);
            m_QuestGoalListProperty.DeleteArrayElementAtIndex(toDelete);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif