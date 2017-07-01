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
    public enum State{idle, ready, building, battle_start, battle_end, game_end};
    public State currentState;
    public int currentRound, team_red_score, team_blue_score;
    public Text gameDisplayText, DebugText;
    public GameObject gameDisplayPanel, teamRedSlimeShop, teamBlueSlimeShop;
    public List<GameObject> team_red = new List<GameObject>();
    public List<GameObject> team_blue = new List<GameObject>();
    private bool isRedFinish, isBlueFinish;

    void Update(){
        DebugText.text = "Round: " + currentRound + " / " + totalRoundGame + "\n";
        DebugText.text += "Score: " + team_red_score + " - " + team_blue_score + "\n"; 
        DebugText.text += "Slime: " + team_red.Count + " - " + team_blue.Count + "\n";
        DebugText.text += "Ready: " + isRedFinish + " - " + isBlueFinish + "";
    }

    public void GameStart(){
        currentState = State.idle;  //set game state = idle
        
        StartCoroutine(DisplayGamePanel());
        
        Invoke("GameReady", 1.5f);
    }

    void GameReady(){
        currentState = State.ready;
        currentRound++;
        StartCoroutine(DisplayGamePanel());
        ShowShop();

        Invoke("StateChangeToBuilding", 4.5f);
    }

    void ShowShop(){
        if(PhotonNetwork.isMasterClient)
            teamRedSlimeShop.SetActive(true);
        else
            teamBlueSlimeShop.SetActive(true);
    }
    /* Building  State */
    void StateChangeToBuilding(){
        currentState = State.building;  //set game state = building
        Debug.Log("It's time to build up your team!");
        StartCoroutine(DisplayGamePanel());

        Invoke("StateChangeToBattle", 15f);
    }
    /* Battle Starts State */
    void StateChangeToBattle(){
        currentState = State.battle_start;    //set game state = battle_start
        Debug.Log("BATTLE IS STARTED!");
        StartCoroutine(DisplayGamePanel());    //display the game panel
        CheckAnyEmptyTeam();    //check any empty team when battle started

        SpawnManager.Instance.ClearSlimeToSpawn();
        GameObject[] nodes = GameObject.FindGameObjectsWithTag("node");
        foreach (GameObject node in nodes)
            node.GetComponent<Node>().ResetNode();
    }

    public void CheckAnyEmptyTeam(){
        if ((team_red.Count == 0 || team_blue.Count == 0) && currentState == State.battle_start){
            BattleEnd();
            //Invoke("BattleEnd", 2f);
        }
    }
    /* Battle End State */
    void BattleEnd(){
        currentState = State.battle_end;    //set game state = battle_end
        Debug.Log("battle is ended.");

        if(PhotonNetwork.isMasterClient)
            GetComponent<PhotonView>().RPC ("RPC_RedTeamFinish", PhotonTargets.All);
        else
            GetComponent<PhotonView>().RPC ("RPC_BlueTeamFinish", PhotonTargets.All);
        
        StartCoroutine(CheckTeamFinish());
    }
    IEnumerator CheckTeamFinish(){
        yield return new WaitForSeconds(1f);
        while(!isRedFinish || !isBlueFinish){
            if(!isRedFinish)
                Debug.Log("Red Not Ready");
            if(!isBlueFinish)
                Debug.Log("Blue Not Ready");
            yield return null;
        }
        //yield return new WaitForSeconds(1f);
        StartCoroutine(DisplayGamePanel());
        isRedFinish = false;
        isBlueFinish = false;

        if(currentRound < totalRoundGame)
            Invoke("GameReady", 6f);
        else
            Invoke("GameEnd", 6f);
    }
    [PunRPC]
    private void RPC_RedTeamFinish(){
        isRedFinish = true;
    }
    [PunRPC]
    private void RPC_BlueTeamFinish(){
        isBlueFinish = true;
    }
    void GameEnd(){
        StartCoroutine(DisplayGamePanel());
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

    IEnumerator ClearAllSlime(List<GameObject> team){
        foreach (GameObject slime in team)
            slime.GetComponent<Slime>().stopMoving();

        yield return new WaitForSeconds(2f);
        foreach (GameObject slime in team)
            Destroy(slime.transform.parent.gameObject);

        team.Clear();
    }

    IEnumerator DisplayGamePanel(){
        gameDisplayPanel.SetActive(true);

        switch (currentState)
        {
            case State.idle:
                PhotonPlayer[] photonPlayers = PhotonNetwork.playerList;
                if(photonPlayers.Length == PhotonNetwork.room.MaxPlayers){
                    gameDisplayText.fontSize = 40;
                    gameDisplayText.text = photonPlayers[0].NickName + "\n" + "vs\n" + photonPlayers[1].NickName;
                }
                else{
                    gameDisplayText.fontSize = 40;
                    gameDisplayText.text = photonPlayers[0].NickName + "\n" + "vs\n" + photonPlayers[0].NickName;
                }
                break;
            case State.ready:
                gameDisplayText.fontSize = 50;
                gameDisplayText.text = "-Round "+ currentRound + "-\nReady!";
                yield return new WaitForSeconds(1f);
                gameDisplayText.text = "3";
                yield return new WaitForSeconds(1f);
                gameDisplayText.text = "2";
                yield return new WaitForSeconds(1f);
                gameDisplayText.text = "1";
                break;
            case State.building:
                gameDisplayText.text = "Building Time!";
                break;
            case State.battle_start:
                gameDisplayText.text = "-Battle Start-";
                break;
            case State.battle_end:
                if (team_blue.Count > 0){
                    gameDisplayText.text = "Team Blue\nwon!";
                    team_blue_score++;
                    StartCoroutine(ClearAllSlime(team_blue));
                }
                else if (team_red.Count > 0){
                    gameDisplayText.text = "Team Red\nwon!";
                    team_red_score++;
                    StartCoroutine(ClearAllSlime(team_red));
                }
                else{
                    team_red_score++;
                    team_blue_score++;
                    gameDisplayText.text = "Draw!";
                }
                yield return new WaitForSeconds(2f);
                gameDisplayText.text = " RED | BLUE \n" + team_red_score + " : " + team_blue_score;
                yield return new WaitForSeconds(2f);
                break;
            case State.game_end:
                gameDisplayText.text = "Game End!";
                break;
            default:
                break;
        }

        yield return new WaitForSeconds(1f);
        gameDisplayPanel.SetActive(false);
    }

    //if one of the player left, the game will be ended
    private void OnPhotonPlayerDisconnected(PhotonPlayer photonPlayer){
        Debug.Log(photonPlayer + " is disconnected");
        gameDisplayText.fontSize = 40;
        gameDisplayText.text = photonPlayer + " left the game.";
        gameDisplayPanel.SetActive(true);
        Invoke("LeaveTheRoom", 3f);
    }

    void LeaveTheRoom(){
        gameDisplayPanel.SetActive(false);
        PhotonNetwork.Destroy(GameObject.Find("DDOL"));
        PhotonNetwork.LoadLevel("GameLobby");
        PhotonNetwork.LeaveRoom();
    }
}