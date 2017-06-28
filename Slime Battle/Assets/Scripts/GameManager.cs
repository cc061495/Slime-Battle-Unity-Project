using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Awake(){
        Instance = this;
    }
    const int totalRoundGame = 100;
    public enum State{idle, building, battle_start, battle_end, game_end};
    public State currentState;
    public int roundOfGame, team_red_score, team_blue_score;
    public Text gameDisplayText;
    public GameObject gameDisplayPanel;
    public List<GameObject> team_red = new List<GameObject>();
    public List<GameObject> team_blue = new List<GameObject>();
    //public GameObject[] team_red, team_blue;
   

    public void GameStart(){
        currentState = State.idle;  //set game state = idle
        Debug.Log("Game will be started in 1 second!");
        DisplayGamePanel();
        roundOfGame++;

        Invoke("StateChangeToBuilding", 1f);
    }

    void Start(){
        Application.targetFrameRate = 30;
        GameStart();
    }


    void StateChangeToBuilding(){
        currentState = State.building;  //set game state = building
        Debug.Log("It's time to build up your team!");
        DisplayGamePanel();

        Invoke("StateChangeToBattle", 15f);
    }

    void StateChangeToBattle(){
        currentState = State.battle_start;    //set game state = battle_start
        Debug.Log("BATTLE IS STARTED!");
        DisplayGamePanel();

        GameObject[] nodes = GameObject.FindGameObjectsWithTag("node");
        foreach (GameObject node in nodes)
            node.GetComponent<Node>().ResetNode();
    }

    public IEnumerator BattleEnd(List<GameObject> red, List<GameObject> blue){
        yield return new WaitForSeconds(0.5f);  //Delay 0.5s
        currentState = State.battle_end;    //set game state = battle_end
        Debug.Log("battle is ended.");
        
        if (blue.Count > 0){
            gameDisplayText.text = "Team Blue won!";
            team_blue_score++;
            StartCoroutine(ClearAllSlime(blue));
        }
        else if (red.Count > 0){
            gameDisplayText.text = "Team Red won!";
            team_red_score++;
            StartCoroutine(ClearAllSlime(red));
        }
        else
            gameDisplayText.text = "Draw!";

        gameDisplayPanel.SetActive(true);

        if(roundOfGame < totalRoundGame)
            Invoke("GameStart", 5f);
        else
            Invoke("GameEnd", 5f);
    }

    IEnumerator ClearAllSlime(List<GameObject> team){
        foreach (GameObject slime in team)
            slime.GetComponent<Slime>().stopMoving();

        yield return new WaitForSeconds(3f);

        foreach (GameObject slime in team)
            Destroy(slime.transform.parent.gameObject);

        team.Clear();
    }

    void GameEnd(){
        DisplayGamePanel();
        if(team_red_score > team_blue_score){
            Debug.Log("FINALLY, Team red won!");
        }
        else if(team_red_score < team_blue_score){
            Debug.Log("FINALLY, Team blue won!");
        }
        else{
            Debug.Log("Draw game");
        }
    }

    void DisplayGamePanel(){
        switch (currentState)
        {
            case State.idle:
                gameDisplayText.text = "Build will be started in 3 second!";
                break;
            case State.building:
                gameDisplayText.text = "Battle will be started in 30 second!";
                break;
            case State.battle_start:
                gameDisplayText.text = "Battle Start!";
                break;
            case State.game_end:
                gameDisplayText.text = "Game End!";
                break;
            default:
                break;
        }
        gameDisplayPanel.SetActive(true);
        Invoke("FadeGameDisplayPanel", 1f);
    }

    void FadeGameDisplayPanel(){
        gameDisplayPanel.SetActive(false);
    }

    //checking the disconnection of the players
    private void OnPhotonPlayerDisconnected(PhotonPlayer photonPlayer){
        Debug.Log(photonPlayer + " is disconnected");
        PhotonNetwork.Destroy(GameObject.Find("DDOL"));
        PhotonNetwork.LoadLevel("GameLobby");
        PhotonNetwork.LeaveRoom();
    }
}