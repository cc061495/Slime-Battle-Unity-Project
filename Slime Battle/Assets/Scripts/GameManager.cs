using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public Button buttonStart;
    public Text winText;
    public bool battleIsStart = false;
    public bool battleIsEnd = false;
    public GameObject winPanel;
    public GameObject[] teamA;
    public GameObject[] teamB;

    public void GameStart()
    {
        Debug.Log("Game Start!");
    }

    public void BattleStart()
    {
        PlayerStats.currentState = PlayerStats.State.battle; //set player'state to BATTLE

        InvokeRepeating("UpdateTeam", 0f, 0.5f);
        GameObject[] nodes = GameObject.FindGameObjectsWithTag("node");
        foreach (GameObject node in nodes)
        {
            Node n = node.GetComponent<Node>();
            n.ResetNode();
        }

        battleIsStart = true;
        buttonStart.interactable = false;
    }

    void UpdateTeam()
    {
        if (battleIsStart)
        {
            teamA = GameObject.FindGameObjectsWithTag("TeamA");
            teamB = GameObject.FindGameObjectsWithTag("TeamB");

            if ((teamA.Length == 0 || teamB.Length == 0))
                BattleEnd();
        }
    }

    void BattleEnd()
    {
        PlayerStats.currentState = PlayerStats.State.idle; //set player'state to IDLE

        if (teamB.Length > 0)
            winText.text = "Slime Team B won!";
        else if (teamA.Length > 0)
            winText.text = "Slime Team A won!";
        else
            winText.text = "Draw!";

        battleIsEnd = true;
        winPanel.SetActive(true);
        CancelInvoke("UpdateTeam");
        Invoke("ClearAllSlime", 2f);
    }

    void ClearAllSlime()
    {
        battleIsStart = false;
        battleIsEnd = false;
        buttonStart.interactable = true;

        for (int i = 0; i < teamA.Length; i++)
        {
            if (teamA[i] != null)
                Destroy(teamA[i].transform.parent.gameObject);
        }
        for (int i = 0; i < teamB.Length; i++)
        {
            if (teamB[i] != null)
                Destroy(teamB[i].transform.parent.gameObject);
        }

        winPanel.SetActive(false);
    }
    //checking the disconnection of the players
    private void OnPhotonPlayerDisconnected(PhotonPlayer photonPlayer)
    {
        Debug.Log(photonPlayer + " is disconnected");
        PhotonNetwork.Destroy(GameObject.Find("DDOL"));
        PhotonNetwork.LoadLevel("GameLobby");
        PhotonNetwork.LeaveRoom();
    }

}