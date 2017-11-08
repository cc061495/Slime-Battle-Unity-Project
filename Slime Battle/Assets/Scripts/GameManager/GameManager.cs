﻿/* Copyright (c) cc061495 */
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    void Awake(){
        Instance = this; 
    }

    public enum State{idle, ready, build_start, build_end, battle_start, battle_end, game_end};
    public State currentState;
    public int currentRound, team_red_score, team_blue_score;
    public Text gameDisplayText, DebugText;
    public GameObject gameDisplayPanel, teamRedSlimeShop, teamBlueSlimeShop, teamControlButton, teamControlPanel, chatButton;
    public SceneFader sceneFader;
    public RewardsPanel rewardsPanel;
    public NextRoundCost nextRoundCostPanel;
    public RectTransform canvasForHealthBar, healthBarParent;
    public List<Transform> team_red = new List<Transform>();    //team with building
    public List<Transform> team_red2 = new List<Transform>();   //team without building
    public List<Transform> team_blue = new List<Transform>();   //team with building
    public List<Transform> team_blue2 = new List<Transform>();  //team without building
    public List<Node> nodeList = new List<Node>();
    public PhotonPlayer masterPlayer;

    private int totalRoundGame, matchPoint, winPoint;
    //private bool isRedFinish, isBlueFinish;
    private float mDeltaTime = 0.0f;
    private float mFPS = 0.0f;
    //PhotonView photonView;
    CameraManager camManager;
    PlayerStats playerStats;

    void Update(){
        DebugText.text = "Round: " + currentRound + " / " + totalRoundGame + "\n";
        DebugText.text += "Score: " + team_red_score + " - " + team_blue_score + "\n"; 
        DebugText.text += "Slime: " + team_red2.Count + " - " + team_blue2.Count + "\n";

        mDeltaTime += (Time.deltaTime - mDeltaTime) * 0.1f;
        float msec = mDeltaTime * 1000.0f;
        mFPS = 1.0f / mDeltaTime;
        DebugText.text += string.Format("{0:0.0} ms ({1:0.} fps)", msec, mFPS);
    }
    
    void Start(){
        //photonView = GetComponent<PhotonView>();
        camManager = GetComponent<CameraManager>();
        masterPlayer = PhotonNetwork.masterClient;
        playerStats = PlayerStats.Instance;

        totalRoundGame = (int) PhotonNetwork.room.CustomProperties["Round"];
        matchPoint = totalRoundGame / 2;
        winPoint = matchPoint + 1;

        if(!PhotonNetwork.connected){
            //Start single mode
            Debug.Log("HELLO");
        }
    }
    /* Game Start State */
    public void GameStart(){
        currentState = State.idle;  //set game state = idle
        
        StartCoroutine(DisplayGamePanel());
        
        //Invoke("GameReady", 3f);
    }
    /* Game Ready State */
    void GameReady(){
        currentState = State.ready;
        Debug.Log("Game Ready!");
        currentRound++;
        StartCoroutine(DisplayGamePanel());

        //Invoke("BuildStart", 5f);
    }
    /* Build Start State */
    void BuildStart(){
        currentState = State.build_start;  //set game state = building
        Debug.Log("Build Start!");
        TimerManager.Instance.setBuildingTime();
        ShopDisplay(true);
        playerStats.PlayerInfoPanelDisplay(true);
        StartCoroutine(DisplayGamePanel());
    }
    /* Build End State */
    public void BuildEnd(){
        //currentState = State.build_end; (Moved to TimerManager.cs)
        Debug.Log("Build End!");
        camManager.CamMove_Battle();

        ShopDisplay(false);

        SellingUI.Instance.SellingPanelDisplay(false);
        playerStats.PlayerInfoPanelDisplay(false);

		/* Clear the selected node */
        SpawnManager.Instance.ClearSelectedNode();
        /* Clear the selected slime to spawn */
        SpawnManager.Instance.ClearSlimeToSpawn();

        DisplayTeam(team_red);
        DisplayTeam(team_blue);

        StartCoroutine(DisplayGamePanel());

        for(int i=0;i<nodeList.Count;i++){
            nodeList[i].NodeResetting(nodeList[i]);
        }
        nodeList.Clear();

        //Invoke("BattleStart", 4f);
    }

    /* Battle Starts State */
    void BattleStart(){
        currentState = State.battle_start;    //set game state = battle_start
        Debug.Log("Battle!!!");
        ResetShopTextDisplay();

        ChangeBuildingLayer();

        // DisplayTeamHealthBar(team_blue);
        // DisplayTeamHealthBar(team_red);
        teamControlButton.SetActive(true);
        chatButton.SetActive(true);

        StartCoroutine(DisplayGamePanel());    //display the game panel

        Invoke("CheckAnyEmptyTeam", 2f);    //check any empty team when battle started
    }

    public void CheckAnyEmptyTeam(){
        if(currentState == State.battle_start){
            if (team_red2.Count == 0 || team_blue2.Count == 0){
                BattleEnd();
            }
        }
    }
    /* Battle End State */
    private void BattleEnd(){
        currentState = State.battle_end;    //set game state = battle_end
        Debug.Log("Battle End!");

        BuildingUI.Instance.BuildingPanelDisplay(false);

        StartCoroutine(DisplayGamePanel());
        //Synchronize for ending game
        // if(PhotonNetwork.isMasterClient)
        //     photonView.RPC ("RPC_RedTeamFinish", PhotonTargets.All);
        // else
        //     photonView.RPC ("RPC_BlueTeamFinish", PhotonTargets.All);
        
        // StartCoroutine(CheckTeamFinish());
    }

    // IEnumerator CheckTeamFinish(){
    //     while(!isRedFinish || !isBlueFinish){
    //         yield return new WaitForSeconds(0.1f);

    //         if(!isRedFinish)
    //             Debug.Log("Red Not Ready");
    //         if(!isBlueFinish)
    //             Debug.Log("Blue Not Ready");
    //     }
    //     //Close and reset the team control panel, when the battle is ended
    //     teamControlPanel.SetActive(false);
    //     TeamController.Instance.SetToDefaultSearchMode();
    //     BuildingUI.Instance.BuildingPanelDisplay(false);

    //     StartCoroutine(DisplayGamePanel());
    //     isRedFinish = false;
    //     isBlueFinish = false;
    // }
    /* Game End State */
    void GameEnd(){
        currentState = State.game_end;
        StartCoroutine(DisplayGamePanel());
    }

    IEnumerator ClearAllSlime(){
        yield return new WaitForSeconds(2f);
        if(PhotonNetwork.isMasterClient)
            PhotonNetwork.DestroyAll();
            
        //Clear all the teams with building
        if(team_red.Count > 0)
            team_red.Clear();
        if(team_blue.Count > 0)
            team_blue.Clear();
    }

    IEnumerator DisplayGamePanel(){
        if(currentState == State.idle){
            yield return new WaitForSeconds(1f);
            gameDisplayText.text = "<size=60><color=#ff0000ff>" + PhotonNetwork.playerName + "</color>\n" + "<color=#ffff00ff>vs</color>\n<color=#00ffffff>" + PhotonNetwork.otherPlayers[0].NickName + "</color></size>";
            gameDisplayPanel.SetActive(true);
            yield return new WaitForSeconds(3f);
            gameDisplayPanel.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            GameReady();
        }
        else if(currentState == State.ready){
            gameDisplayPanel.SetActive(true);
            if(team_red_score == matchPoint || team_blue_score == matchPoint){
                gameDisplayText.text = "<size=70><color=#ffff00ff>Match Point</color></size>";
                yield return new WaitForSeconds(1.5f);
            }
            if(currentRound == totalRoundGame)
                gameDisplayText.text = "<size=60>Final Round"+ "\nReady!</size>";
            else
                gameDisplayText.text = "Round "+ currentRound + "\nReady!";
            yield return new WaitForSeconds(1.5f);
            camManager.CamMove_Build();
            gameDisplayText.text = "<size=100>3</size>";
            yield return new WaitForSeconds(1f);
            gameDisplayText.text = "<size=100>2</size>";
            yield return new WaitForSeconds(1f);
            gameDisplayText.text = "<size=100>1</size>";
            yield return new WaitForSeconds(1f);
            gameDisplayPanel.SetActive(false);

            yield return new WaitForSeconds(0.5f);
            BuildStart();
        }
        else if(currentState == State.build_start){
            gameDisplayText.text = "Building\nTime!";
            gameDisplayPanel.SetActive(true);
            yield return new WaitForSeconds(1f);
            gameDisplayPanel.SetActive(false);
        }
        else if(currentState == State.build_end){
            gameDisplayText.text = "Stop\nBuilding!";
            gameDisplayPanel.SetActive(true);
            yield return new WaitForSeconds(1f);
            gameDisplayPanel.SetActive(false);

            yield return new WaitForSeconds(3f);
            BattleStart();
        }
        else if(currentState == State.battle_start){
            gameDisplayText.text = "Battle Start!";
            gameDisplayPanel.SetActive(true);
            yield return new WaitForSeconds(1f);
            gameDisplayPanel.SetActive(false);
        }
        else if(currentState == State.battle_end){
            yield return new WaitForSeconds(1f);
            teamControlButton.SetActive(false);
            teamControlPanel.SetActive(false);
            chatButton.SetActive(false);
            TeamController.Instance.SetToDefaultSearchMode();
            
            if (team_blue2.Count > 0){
                gameDisplayText.text = "<color=#00ffffff>Team Blue</color>\nwon!";
                team_blue_score++;
                team_blue2.Clear();
            }
            else if (team_red2.Count > 0){
                gameDisplayText.text = "<color=#ff0000ff>Team Red</color>\nwon!";
                team_red_score++;
                team_red2.Clear();
            }
            else{
                team_red_score++;
                team_blue_score++;
                gameDisplayText.text = "<color=#ffff00ff>Draw!</color>";
            }
            StartCoroutine(ClearAllSlime());
            gameDisplayPanel.SetActive(true);
            yield return new WaitForSeconds(2f);
            gameDisplayText.text = " <color=#ff0000ff>RED</color> | <color=#00ffffff>BLUE</color> \n" + team_red_score + " : " + team_blue_score;
            yield return new WaitForSeconds(2f);
            gameDisplayPanel.SetActive(false);

            if(team_red_score < winPoint && team_blue_score < winPoint){
                yield return new WaitForSeconds(0.5f);
                /* Display Rewards Panel */
                nextRoundCostPanel.gameObject.SetActive(true);
                nextRoundCostPanel.TextSetting();
                yield return new WaitForSeconds(7f);
                nextRoundCostPanel.gameObject.SetActive(false);

                playerStats.NewRoundCostUpdate();
                PlayerShop.Instance.ButtonsUpdate();
                yield return new WaitForSeconds(0.5f);
                GameReady();
            }
            else{
                yield return new WaitForSeconds(0.5f);
                GameEnd();
            }
        }
        else if(currentState == State.game_end){
            gameDisplayText.text = "<size=75>Game End!</size>";
            gameDisplayPanel.SetActive(true);
            yield return new WaitForSeconds(2f);
            if(team_red_score > team_blue_score){
                gameDisplayText.text = "Winner\n<size=70><color=#ff0000ff>Team Red</color></size>";
            }
            else if(team_red_score < team_blue_score){
                gameDisplayText.text = "Winner\n<size=70><color=#00ffffff>Team Blue</color></size>";
            }
            else{
                gameDisplayText.text = "<size=75><color=#ffff00ff>Draw Game</color></size>";
            }
            yield return new WaitForSeconds(3f);
            gameDisplayText.text = "<size=75>Good Game!</size>";
            yield return new WaitForSeconds(2f);
            gameDisplayPanel.SetActive(false);
            yield return new WaitForSeconds(1f);
            LeaveTheRoom();
        }
    }

    //if one of the player left, the game will be ended
    private void OnPhotonPlayerDisconnected(PhotonPlayer photonPlayer){
        if(currentState != State.game_end){
            Debug.Log(photonPlayer + " is disconnected");
            gameDisplayText.text = "<size=60>" + photonPlayer + "\nleft the game.</size>";
            gameDisplayPanel.SetActive(true);
            Invoke("LeaveTheRoom", 3f);
        }
    }

    public void LeaveTheRoom(){
        Destroy(GameObject.Find("DDOL"));
        //PhotonNetwork.LeaveRoom();
        sceneFader.FadeToWithPhotonNetwork("GameLobby");
    }

    public void ShopDisplay(bool shopDisplay){
        if(PhotonNetwork.isMasterClient)
            teamRedSlimeShop.SetActive(shopDisplay);
        else
            teamBlueSlimeShop.SetActive(shopDisplay);
    }

    private void ResetShopTextDisplay(){
        if(PhotonNetwork.isMasterClient)
            teamRedSlimeShop.GetComponent<PlayerShop>().ResetShopText();
        else
            teamBlueSlimeShop.GetComponent<PlayerShop>().ResetShopText();
    }

    public List<Transform> GetEnemies(Transform slime){
        if(slime.tag == "Team_RED")
            return team_blue;
        else
            return team_red;
    }

    public List<Transform> GetMyTeam(Transform slime){
        if(slime.tag == "Team_RED")
            return team_red;
        else
            return team_blue;
    }

    // [PunRPC]
    // private void RPC_RedTeamFinish(){
    //     isRedFinish = true;
    // }
    
    // [PunRPC]
    // private void RPC_BlueTeamFinish(){
    //     isBlueFinish = true;
    // }

    private void DisplayTeam(List<Transform> team){
        for(int i=0;i<team.Count;i++){
            Slime s = team[i].parent.GetComponent<Slime>();
            s.DisplaySlime(true);
        }
    }

    // private void DisplayTeamHealthBar(List<Transform> team){
    //     for(int i=0;i<team.Count;i++){
    //         SlimeHealth h = team[i].parent.GetComponent<SlimeHealth>();
    //         h.DisplayHealthBar(true);
    //     }
    // }

    private void ChangeBuildingLayer(){
        //team that ONLY including building
        List<Transform> team;

        if(PhotonNetwork.isMasterClient)
            team = team_red.Except(team_red2).ToList();
        else
            team = team_blue.Except(team_blue2).ToList();

        for(int i=0;i<team.Count;i++){
            team[i].gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
}