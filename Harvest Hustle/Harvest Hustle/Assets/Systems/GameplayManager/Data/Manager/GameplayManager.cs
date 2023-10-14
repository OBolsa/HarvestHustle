using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System.Collections.Generic;
using DG.Tweening;
using System;

public enum FreeLookCameraType { Top, Middle, Bottom }
public class GameplayManager : GameStateMachine
{
    public static GameplayManager instance;
    public GlobalConfigs globalConfigs;

    [Header("Global Components")]
    public Container_UI playerInventory;
    public PlayerController player;
    public CinemachineBrain cameraBrain;
    [SerializeReference] public List<CinemachineVirtualCameraBase> virtualCameras = new List<CinemachineVirtualCameraBase>();
    public Tooltip tooltip;
    public InteractableInstigator interactableInstigator;
    public FarmingManager farmingManager;
    public ItemManager itemManager;
    public ModalManager modalManager;
    public ClimateManager climateManager;
    public QuestManager questManager;
    public DialogueDisplayer dialogueDisplayer;
    public SceneTransitionManager sceneManager;
    public ProgressBar_UI progressBar;
    public SceneSaver sceneSaver;
    public DonationManager donationManager;

    private GameObject modalToOpen;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CinemachineVirtualCameraBase[] cameras = FindObjectsOfType<CinemachineVirtualCameraBase>();
        foreach (var cam in cameras) { virtualCameras.Add(cam); }

        ChangeState(new Gameplay_GameState(this));
    }

    private void OnEnable()
    {
        PlayerInputManager.PlayerInput.World.Inventory.performed += OpenInventory;
    }

    private void OnDisable()
    {
        PlayerInputManager.PlayerInput.World.Inventory.performed -= OpenInventory;
    }

    public void ChangeFreeLookCamera(FreeLookCameraType type)
    {
        CinemachineFreeLook camera = virtualCameras.Find(c => c as CinemachineFreeLook) as CinemachineFreeLook;

        if (camera == null)
            return;

        switch (type)
        {
            case FreeLookCameraType.Top:
                DOTween.To(() => camera.m_YAxis.Value, x => camera.m_YAxis.Value = x, 1f, 1f);
                camera.m_XAxis.m_MaxSpeed = 0;
                break;
            default:
            case FreeLookCameraType.Middle:
                DOTween.To(() => camera.m_YAxis.Value, x => camera.m_YAxis.Value = x, 0.5f, 1f);
                camera.m_XAxis.m_MaxSpeed = 300;
                break;
            case FreeLookCameraType.Bottom:
                DOTween.To(() => camera.m_YAxis.Value, x => camera.m_YAxis.Value = x, 0f, 1f);
                camera.m_XAxis.m_MaxSpeed = 0;
                break;
        }
    }

    public void ChangeCamera(string cameraName)
    {
        foreach (var camera in virtualCameras)
        {
            camera.Priority = camera.name == cameraName ? 1 : 0;
        }
    }

    public void ChangeCamera(CinemachineVirtualCameraBase newCamera)
    {
        foreach (var camera in virtualCameras)
        {
            camera.Priority = 0;
        }

        newCamera.Priority = 1;
    }

    public void CallForModal(Modal modal)
    {
        modalManager.CloseModal();
        StartCoroutine(CurrentState.CallForModal(modal));
    }

    public void OpenInventory(InputAction.CallbackContext context)
    {
        StartCoroutine(CurrentState.CallForInventory());
    }
}