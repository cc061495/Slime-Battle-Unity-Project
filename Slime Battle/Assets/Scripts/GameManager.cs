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
    const int totalRoundGame = 5;
    public enum State{idle, ready, build_start, build_end, battle_start, battle_end, game_end};
    public State currentState;
    public int currentRound, team_red_score, team_blue_score;
    public Text gameDisplayText, DebugText;
    public GameObject gameDisplayPanel, teamRedSlimeShop, teamBlueSlimeShop;
    public List<Transform> team_red = new List<Transform>();
    public List<Transform> team_blue = new List<Transform>();
    private bool isRedFinish, isBlueFinish;

    private float mDeltaTime = 0.0f;
    private float mFPS = 0.0f;
    PhotonView photonView;

    void Update(){
        DebugText.text = "Round: " + currentRound + " / " + totalRoundGame + "\n";
        DebugText.text += "Score: " + team_red_score + " - " + team_blue_score + "\n"; 
        DebugText.text += "Slime: " + team_red.Count + " - " + team_blue.Count + "\n";

        mDeltaTime += (Time.deltaTime - mDeltaTime) * 0.1f;
        float msec = mDeltaTime * 1000.0f;
        mFPS = 1.0f / mDeltaTime;
        DebugText.text += string.Format("{0:0.0} ms ({1:0.} fps)", msec, mFPS);
    }
    void Start(){
        photonView = GetComponent<PhotonView>();
        if(!PhotonNetwork.connected){
            //Start single mode
            Debug.Log("HELLO");
        }
    }
    /* Game Start State */
    public void GameStart(){
        currentState = State.idle;  //set game state = idle
        
        StartCoroutine(DisplayGamePanel());
        
        Invoke("GameReady", 2f);
    }
    /* Game Ready State */
    void GameReady(){
        currentState = State.ready;
        Debug.Log("Game Ready!");
        currentRound++;
        StartCoroutine(DisplayGamePanel());

        Invoke("BuildStart", 5f);
    }
    /* Build Start State */
    void BuildStart(){
        currentState = State.build_start;  //set game state = building
        Debug.Log("Build Start!");
        TimerManager.Instance.setBuildingTime();
        ShopDisplay(true);
        PlayerStats.Instance.PlayerInfoPanelDisplay(true);
        StartCoroutine(DisplayGamePanel());
    }
    /* Build End State */
    public void BuildEnd(){
        currentState = State.build_end;
        Debug.Log("Build End!");
        GetComponent<CameraManager>().CamMove_Battle();
        ShopDisplay(false);
        PlayerStats.Instance.PlayerInfoPanelDisplay(false);

        DisplayTeam(team_red);
        DisplayTeam(team_blue);

        StartCoroutine(DisplayGamePanel());

        GameObject[] nodes = GameObject.FindGameObjectsWithTag("node");
        foreach (GameObject node in nodes)
            node.GetComponent<Node>().ResetNode();

        Invoke("BattleStart", 4f);
    }

    /* Battle Starts State */
    void BattleStart(){
        currentState = State.battle_start;    //set game state = battle_start
        Debug.Log("Battle!!!");
        StartCoroutine(DisplayGamePanel());    //display the game panel

        CheckAnyEmptyTeam();    //check any empty team when battle started

        SpawnManager.Instance.ClearSlimeToSpawn();
    }

    public void CheckAnyEmptyTeam(){
        if ((team_red.Count == 0 || team_blue.Count == 0) && currentState == State.battle_start){
            BattleEnd();
        }
    }
    /* Battle End State */
    void BattleEnd(){
        currentState = State.battle_end;    //set game state = battle_end
        Debug.Log("Battle End!");
        if(PhotonNetwork.isMasterClient)
            photonView.RPC ("RPC_RedTeamFinish", PhotonTargets.All);
        else
            photonView.RPC ("RPC_BlueTeamFinish", PhotonTargets.All);
        
        StartCoroutine(CheckTeamFinish());
    }
    IEnumerator CheckTeamFinish(){
        while(!isRedFinish || !isBlueFinish){
            if(!isRedFinish)
                Debug.Log("Red Not Ready");
            if(!isBlueFinish)
                Debug.Log("Blue Not Ready");
            yield return new WaitForSeconds(0.1f);
        }
        StartCoroutine(DisplayGamePanel());
        isRedFinish = false;
        isBlueFinish = false;

        if(currentRound < totalRoundGame){
            PlayerStats.Instance.NewRoundCostUpdate();
            Invoke("GameReady", 5f);
        }
        else
            Invoke("GameEnd", 5f);
    }
    void GameEnd(){
        currentState = State.game_end;
        StartCoroutine(DisplayGamePanel());
        Invoke("LeaveTheRoom", 3f);
    }

    IEnumerator ClearAllSlime(List<Transform> team){
        yield return new WaitForSeconds(2f);
        if(PhotonNetwork.isMasterClient)
            PhotonNetwork.DestroyAll();
        // foreach (Transform slime in team){
        //     if(slime.parent.GetComponent<PhotonView>().isMine)
        //         PhotonNetwork.Destroy(slime.parent.gameObject);
        // }
        team.Clear();
    }

    IEnumerator DisplayGamePanel(){
        switch (currentState)
        {
            case State.idle:
                gameDisplayText.fontSize = 40;
                gameDisplayText.text = PhotonNetwork.playerName + "\n" + "vs\n" + PhotonNetwork.otherPlayers[0].NickName;
                gameDisplayPanel.SetActive(true);
                yield return new WaitForSeconds(1f);
                break;
            case State.ready:
                gameDisplayText.fontSize = 50;
                gameDisplayText.text = "-Round "+ currentRound + "-\nReady!";
                gameDisplayPanel.SetActive(true);
                yield return new WaitForSeconds(1.5f);
                GetComponent<CameraManager>().CamMove_Build();
                gameDisplayText.text = "3";
                yield return new WaitForSeconds(1f);
                gameDisplayText.text = "2";
                yield return new WaitForSeconds(1f);
                gameDisplayText.text = "1";
                yield return new WaitForSeconds(1f);
                break;
            case State.build_start:
                gameDisplayText.text = "Building Time!";
                gameDisplayPanel.SetActive(true);
                yield return new WaitForSeconds(1f);
                break;
            case State.build_end:
                gameDisplayText.text = "STOP\nBUILDING!";
                gameDisplayPanel.SetActive(true);
                yield return new WaitForSeconds(1f);
                break;
            case State.battle_start:
                gameDisplayText.text = "-Battle Start-";
                gameDisplayPanel.SetActive(true);
                yield return new WaitForSeconds(1f);
                break;
            case State.battle_end:
                if (team_blue.Count > 0){
                    gameDisplayText.color = Color.cyan;
                    gameDisplayText.text = "Team Blue\nwon!";
                    team_blue_score++;
                    StartCoroutine(ClearAllSlime(team_blue));
                }
                else if (team_red.Count > 0){
                    gameDisplayText.color = Color.red;
                    gameDisplayText.text = "Team Red\nwon!";
                    team_red_score++;
                    StartCoroutine(ClearAllSlime(team_red));
                }
                else{
                    team_red_score++;
                    team_blue_score++;
                    gameDisplayText.text = "Draw!";
                }
                gameDisplayPanel.SetActive(true);
                yield return new WaitForSeconds(2f);
                gameDisplayText.color = Color.white;
                gameDisplayText.text = " RED | BLUE \n" + team_red_score + " : " + team_blue_score;
                yield return new WaitForSeconds(2f);
                break;
            case State.game_end:
                gameDisplayText.text = "Game End!";
                gameDisplayPanel.SetActive(true);
                if(team_red_score > team_blue_score){
                    gameDisplayText.color = Color.red;
                    gameDisplayText.text = "-Winner-\nTeam Red";
                }
                else if(team_red_score < team_blue_score){
                    gameDisplayText.color = Color.cyan;
                    gameDisplayText.text = "-Winner-\nTeam Blue";
                }
                else{
                    gameDisplayText.text = "Draw!!!";
                }
                yield return new WaitForSeconds(1f);
                gameDisplayText.color = Color.white;
                gameDisplayText.text = "Good Game~";
                yield return new WaitForSeconds(1f);
                break;
            default:
                break;
        }
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

    private void LeaveTheRoom(){
        //gameDisplayPanel.SetActive(false);
        Destroy(GameObject.Find("DDOL"));
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("GameLobby");
    }

    private void ShopDisplay(bool shopDisplay){
        if(PhotonNetwork.isMasterClient)
            teamRedSlimeShop.SetActive(shopDisplay);
        else
            teamBlueSlimeShop.SetActive(shopDisplay);
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

    [PunRPC]
    private void RPC_RedTeamFinish(){
        isRedFinish = true;
    }
    [PunRPC]
    private void RPC_BlueTeamFinish(){
        isBlueFinish = true;
    }

    private void DisplayTeam(List<Transform> team){
        foreach (Transform slime in team){
            SlimeHealth h = slime.parent.GetComponent<SlimeHealth>();
            h.DisplaySlime(true);
        }
    }
}