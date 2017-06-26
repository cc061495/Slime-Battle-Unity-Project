using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvasManager : MonoBehaviour
{

    public static MainCanvasManager Instance;

    [SerializeField]
    private LobbyCanvas _lobbyCanvas;
    public LobbyCanvas lobbyCanvas
    {
        get { return _lobbyCanvas; }
    }

    [SerializeField]
    private CurrentRoomCanvas _currentRoomCanvas;
    public CurrentRoomCanvas CurrentRoomCanvas
    {
        get { return _currentRoomCanvas; }
    }

    private void Awake()
    {
        Instance = this;
    }
}
