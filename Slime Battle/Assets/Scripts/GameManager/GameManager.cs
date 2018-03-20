/* Copyright (c) cc061495 */
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    void Awake(){
        Instance = this; 
        // fix the fps = 60 in the menu screen
		Application.targetFrameRate = 60;
    }

    public enum State{idle, ready, build_start, build_end, battle_start, battle_end, game_end};
    public State currentState;
    public int currentRound, team_red_score, team_blue_score;
    public Text gameDisplayText, DebugText;
    public GameObject shopPanel;
    public SceneFader sceneFader;
    public RewardsPanel rewardsPanel;
    public NextRoundCost nextRoundCostPanel;
    public RectTransform canvasForHealthBar, healthBarParent;
    public List<Transform> team_red = new List<Transform>();    //team with building, not including invisible gameobject
    public List<Transform> team_red2 = new List<Transform>();   //team without building
    public List<Transform> team_blue = new List<Transform>();   //team with building, not including invisible gameobject
    public List<Transform> team_blue2 = new List<Transform>();  //team without building
    public List<Transform> team_invisible = new List<Transform>();
    public List<Node> nodeList = new List<Node>();
    public PhotonPlayer masterPlayer;

    private int totalRoundGame, matchPoint, winPoint;
    //private float mDeltaTime = 0.0f;
    //private float mFPS = 0.0f;
    PhotonView photonView;
    CameraManager camManager;
    PlayerStats playerStats;
    TeamController teamController;
    ChattingPanel chattingPanel;

    void Update(){
        // DebugText.text = "Round: " + currentRound + " / " + totalRoundGame + "\n";
        // DebugText.text += "Score: " + team_red_score + " - " + team_blue_score + "\n"; 
        // DebugText.text += "Slime: " + team_red2.Count + " - " + team_blue2.Count + "\n";

        // mDeltaTime += (Time.deltaTime - mDeltaTime) * 0.1f;
        // float msec = mDeltaTime * 1000.0f;
        // mFPS = 1.0f / mDeltaTime;
        // DebugText.text += string.Format("{0:0.0} ms ({1:0.} fps)", msec, mFPS);
    }
    
    void Start(){
        photonView = GetComponent<PhotonView>();
        camManager = GetComponent<CameraManager>();
        masterPlayer = PhotonNetwork.masterClient;
        playerStats = PlayerStats.Instance;
        teamController = TeamController.Instance;
        chattingPanel = ChattingPanel.Instance;

        totalRoundGame = (int) PhotonNetwork.room.CustomProperties["Round"];
        matchPoint = totalRoundGame / 2;
        winPoint = matchPoint + 1;
    }
    /* Game Start State */
    public void GameStart(){
        currentState = State.idle;  //set game state = idle
        StartCoroutine(DisplayGamePanel());
    }
    /* Game Ready State */
    [PunRPC]
    private void RPC_GameReady(){
        currentState = State.ready;
        Debug.Log("Game Ready!");
        currentRound++;
        StartCoroutine(DisplayGamePanel());
    }
    /* Build Start State */
    [PunRPC]
    private void RPC_BuildStart(){
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
        camManager.SetCameraMovement(false);

        ShopDisplay(false);

        SellingUI.Instance.SellingPanelDisplay(false);
        playerStats.PlayerInfoPanelDisplay(false);

		/* Clear the selected node */
        SpawnManager.Instance.ClearSelectedNode();
        /* Clear the selected slime to spawn */
        SpawnManager.Instance.ClearSlimeToSpawn();

        DisplayTeam(team_red);
        DisplayTeam(team_blue);
        DisplayInvisibleTeam(team_invisible);

        StartCoroutine(DisplayGamePanel());

        for(int i=0;i<nodeList.Count;i++){
            nodeList[i].NodeResetting(nodeList[i]);
        }
        nodeList.Clear();
    }

    /* Battle Starts State */
    [PunRPC]
    private void RPC_BattleStart(){
        currentState = State.battle_start;    //set game state = battle_start
        Debug.Log("Battle!!!");
        ResetShopTextDisplay();

        ChangeBuildingLayer();

        // DisplayTeamHealthBar(team_blue);
        // DisplayTeamHealthBar(team_red);
        teamController.SetControlButtonDisplay(true);
        chattingPanel.SetChatButtonDisplay(true);

        StartCoroutine(DisplayGamePanel());    //display the game panel

        Invoke("CheckAnyEmptyTeam", 1f);    //check any empty team when battle started
    }

    public void CheckAnyEmptyTeam(){
        Invoke("AnyEmptyTeam", 0.5f);
    }

    private void AnyEmptyTeam(){
        if(currentState == State.battle_start){
            if (team_red2.Count == 0 || team_blue2.Count == 0){     
                /* sync the Battle Start */
                if(PhotonNetwork.isMasterClient)
                    photonView.RPC("RPC_BattleEnd", PhotonTargets.All);
            }
        }
    }
    /* Battle End State */
    [PunRPC]
    private void RPC_BattleEnd(){
        currentState = State.battle_end;    //set game state = battle_end
        Debug.Log("Battle End!");

        BuildingUI.Instance.BuildingPanelDisplay(false);

        StartCoroutine(DisplayGamePanel());
    }
    /* Game End State */
    [PunRPC]
    private void RPC_GameEnd(){
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
        if(team_invisible.Count > 0)
            team_invisible.Clear();
    }

    IEnumerator DisplayGamePanel(){
        if(currentState == State.idle){
            yield return new WaitForSeconds(1f);
            gameDisplayText.text = "<size=60><color=#ff0000ff>" + PhotonNetwork.playerName + "</color>\n" + "<color=#ffff00ff>vs</color>\n<color=#00ffffff>" + PhotonNetwork.otherPlayers[0].NickName + "</color></size>";
            gameDisplayText.enabled = true;
            yield return new WaitForSeconds(3f);
            gameDisplayText.enabled = false;
            yield return new WaitForSeconds(0.5f);
            /* sync the GameReady */
            if(PhotonNetwork.isMasterClient)
                photonView.RPC("RPC_GameReady", PhotonTargets.All);
        }
        else if(currentState == State.ready){
            gameDisplayText.enabled = true;
            if(team_red_score == matchPoint || team_blue_score == matchPoint){
                gameDisplayText.text = "<size=70><color=#ffff00ff>Match Point</color></size>";
                yield return new WaitForSeconds(1.5f);
            }
            if(currentRound == totalRoundGame)
                gameDisplayText.text = "<size=60>Final Round"+ "\nReady!</size>";
            else
                gameDisplayText.text = "Round "+ currentRound + "\nReady!";
            yield return new WaitForSeconds(1.5f);
            camManager.SetCameraMovement(true);
            gameDisplayText.text = "<size=100>3</size>";
            yield return new WaitForSeconds(1f);
            gameDisplayText.text = "<size=100>2</size>";
            yield return new WaitForSeconds(1f);
            gameDisplayText.text = "<size=100>1</size>";
            yield return new WaitForSeconds(1f);
            gameDisplayText.enabled = false;

            yield return new WaitForSeconds(0.5f);
            /* sync the building timer */
            if(PhotonNetwork.isMasterClient)
                photonView.RPC("RPC_BuildStart", PhotonTargets.All);
        }
        else if(currentState == State.build_start){
            gameDisplayText.text = "Building\nTime!";
            gameDisplayText.enabled = true;
            yield return new WaitForSeconds(1f);
            gameDisplayText.enabled = false;
        }
        else if(currentState == State.build_end){
            gameDisplayText.text = "Stop\nBuilding!";
            gameDisplayText.enabled = true;
            yield return new WaitForSeconds(1f);
            gameDisplayText.enabled = false;

            yield return new WaitForSeconds(3f);
            /* sync the Battle Start */
            if(PhotonNetwork.isMasterClient)
                photonView.RPC("RPC_BattleStart", PhotonTargets.All);
        }
        else if(currentState == State.battle_start){
            gameDisplayText.text = "Battle Start!";
            gameDisplayText.enabled = true;
            yield return new WaitForSeconds(1f);
            gameDisplayText.enabled = false;
        }
        else if(currentState == State.battle_end){
            yield return new WaitForSeconds(1f);
            teamController.SetControlPanelDisplay(false);
            teamController.SetControlButtonDisplay(false);
            chattingPanel.SetChatButtonDisplay(false);
            chattingPanel.SetChatPanelDisplay(false);
            teamController.SetToDefaultSearchMode();
            
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
            gameDisplayText.enabled = true;
            yield return new WaitForSeconds(2f);
            gameDisplayText.text = " <color=#ff0000ff>RED</color> | <color=#00ffffff>BLUE</color> \n" + team_red_score + " : " + team_blue_score;
            yield return new WaitForSeconds(2f);
            gameDisplayText.enabled = false;

            if(team_red_score < winPoint && team_blue_score < winPoint){
                yield return new WaitForSeconds(0.5f);
                /* Display Rewards Panel */
                nextRoundCostPanel.gameObject.SetActive(true);
                nextRoundCostPanel.TextSetting();
                yield return new WaitForSeconds(9f);
                nextRoundCostPanel.gameObject.SetActive(false);

                playerStats.NewRoundCostUpdate();
                PlayerShop.Instance.ButtonsUpdate();
                yield return new WaitForSeconds(0.5f);   
                /* sync the GameReady */
                if(PhotonNetwork.isMasterClient)
                    photonView.RPC("RPC_GameReady", PhotonTargets.All);
            }
            else{
                yield return new WaitForSeconds(0.5f);
                /* sync the GameReady */
                if(PhotonNetwork.isMasterClient)
                    photonView.RPC("RPC_GameEnd", PhotonTargets.All);
            }
        }
        else if(currentState == State.game_end){
            string gameWinner = "";
            gameDisplayText.text = "<size=75>Game End!</size>";
            gameDisplayText.enabled = true;
            yield return new WaitForSeconds(2f);
            if(team_red_score > team_blue_score){
                gameDisplayText.text = "Winner\n<size=70><color=#ff0000ff>Team Red</color></size>";
                gameWinner = "red";
            }
            else if(team_red_score < team_blue_score){
                gameDisplayText.text = "Winner\n<size=70><color=#00ffffff>Team Blue</color></size>";
                gameWinner = "blue";
            }
            else{
                gameDisplayText.text = "<size=75><color=#ffff00ff>Draw Game</color></size>";
                gameWinner = "draw";
            }
            yield return new WaitForSeconds(3f);
            gameDisplayText.enabled = false;
            rewardsPanel.gameObject.SetActive(true);
            rewardsPanel.TextSetting();
            rewardsPanel.SetUpWinBouns(gameWinner);
        }
    }

    //if one of the player left, the game will be ended
    private void OnPhotonPlayerDisconnected(PhotonPlayer photonPlayer){
        if(currentState != State.game_end){
            ClearAllInTheTeam(photonPlayer);
            Debug.Log(photonPlayer + " is disconnected");
            gameDisplayText.text = "<color=#ffff00ff><size=60>" + photonPlayer + "\nleft the game.</size></color>";
            gameDisplayText.enabled = true;
            Invoke("LeaveTheRoom", 3f);
        }
    }

    private bool isPlayerLeft;

    public void LeaveTheRoom(){
        if(!isPlayerLeft){
            isPlayerLeft = true;
            Destroy(GameObject.Find("DDOL"));
            sceneFader.FadeToWithPhotonNetwork("GameLobby");
        }
    }

    public void ShopDisplay(bool shopDisplay){
        shopPanel.SetActive(shopDisplay);
    }

    private void ResetShopTextDisplay(){
        shopPanel.GetComponent<PlayerShop>().ResetShopText();
    }

    private void DisplayTeam(List<Transform> team){
        for(int i=0;i<team.Count;i++){
            Slime s = team[i].root.GetComponent<Slime>();
            if(!s.GetSlimeClass().isInvisible)
                s.DisplaySlime(true, true);

            if(s.GetSlimeClass().canCarve)
                s.GetObstacle().carving = true;
        }
    }

    private void DisplayInvisibleTeam(List<Transform> team){
        for(int i=0;i<team.Count;i++){
            Slime s = team[i].root.GetComponent<Slime>();
            s.DisplaySlime(false, false);
        }
    }
    
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

    private void ClearAllInTheTeam(PhotonPlayer player){
        if(player == masterPlayer){
            team_red.Clear();
            team_red2.Clear();
        }
        else{
            team_blue.Clear();
            team_red2.Clear();
        }
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

    public List<Transform> GetMyTeamWithoutBuilding(Transform slime){
        if(slime.tag == "Team_RED")
            return team_red2;
        else
            return team_blue2;  
    }
}