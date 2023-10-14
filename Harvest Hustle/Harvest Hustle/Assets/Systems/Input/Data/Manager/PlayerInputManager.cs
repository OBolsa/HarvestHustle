using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputMap PlayerInput { get; private set; }

    private void Awake()
    {
        PlayerInput = new PlayerInputMap();
        PlayerInput.Enable();
    }
}