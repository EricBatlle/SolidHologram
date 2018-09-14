using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;
using System.Collections;
using System.Collections.Generic;


namespace Prototype.NetworkLobby
{
    public class LobbyManager : NetworkLobbyManager 
    {
        //ERIC STUFF
        public int avatarIndex;
        public Dictionary<int, int[]> currentPlayers; //characterType, playerType
        public LinkedList<RectTransform> userTrail = new LinkedList<RectTransform>();
        //---
        static short MsgKicked = MsgType.Highest + 1;

        static public LobbyManager s_Singleton;


        [Header("Unity UI Lobby")]
        [Tooltip("Time in second between all players ready & match start")]
        public float prematchCountdown = 5.0f;

        [Space]
        [Header("UI Reference")]
        public LobbyTopPanel topPanel;

        public RectTransform mainMenuPanel;
        public RectTransform lobbyPanel;

        public LobbyInfoPanel infoPanel;
        public LobbyCountdownPanel countdownPanel;
        public GameObject addPlayerButton;

        protected RectTransform currentPanel;

        public Button backButton;
        public Button exitButton;
        public Button musicButton;

        public Text statusInfo;
        public Text hostInfo;

        //Client numPlayers from NetworkManager is always 0, so we count (throught connect/destroy in LobbyPlayer) the number
        //of players, so that even client know how many player there is.
        [HideInInspector]
        public int _playerNumber = 0;

        //used to disconnect a client properly when exiting the matchmaker
        [HideInInspector]
        public bool _isMatchmaking = false;

        protected bool _disconnectServer = false;
        
        protected ulong _currentMatchID;

        protected LobbyHook _lobbyHooks;        

        void Start()
        {
            //ERIC STUFF
            currentPlayers = new Dictionary<int, int[]>();
            //-----
            s_Singleton = this;
            _lobbyHooks = GetComponent<Prototype.NetworkLobby.LobbyHook>();
            currentPanel = mainMenuPanel;
            userTrail.AddLast(currentPanel);
            backButton.gameObject.SetActive(false);
            GetComponent<Canvas>().enabled = true;

            DontDestroyOnLoad(gameObject);

            SetServerInfo("Offline", "None");
        }

        public override void OnLobbyClientSceneChanged(NetworkConnection conn)
        {
            if (SceneManager.GetSceneAt(0).name == lobbyScene)
            {
                if (topPanel.isInGame)
                {
                    ChangeTo(lobbyPanel);
                    if (_isMatchmaking)
                    {
                        if (conn.playerControllers[0].unetView.isServer)
                        {
                            backDelegate = StopHostClbk;
                        }
                        else
                        {
                            backDelegate = StopClientClbk;
                        }
                    }
                    else
                    {
                        if (conn.playerControllers[0].unetView.isClient)
                        {
                            backDelegate = StopHostClbk;
                        }
                        else
                        {
                            backDelegate = StopClientClbk;
                        }
                    }
                }
                else
                {
                    ChangeTo(mainMenuPanel);
                }

                topPanel.ToggleVisibility(true);
                topPanel.isInGame = false;
            }
            else
            {
                ChangeTo(null);

                Destroy(GameObject.Find("MainMenuUI(Clone)"));

                //backDelegate = StopGameClbk;
                topPanel.isInGame = true;

                backButton.gameObject.SetActive(false);
                musicButton.gameObject.SetActive(false);

                topPanel.ToggleVisibility(false);
            }
        }

        public void ChangeTo(RectTransform newPanel)
        {
            if (currentPanel != null)
            {
                currentPanel.gameObject.SetActive(false);
            }

            if (newPanel != null)
            {
                newPanel.gameObject.SetActive(true);
            }
            currentPanel = newPanel;
            userTrail.AddLast(newPanel);            
            if (currentPanel != mainMenuPanel)
            {                
                backButton.gameObject.SetActive(true);
                musicButton.gameObject.SetActive(true);                

                exitButton.gameObject.SetActive(false);
            }
            else
            {
                backButton.gameObject.SetActive(false);
                exitButton.gameObject.SetActive(true);
                musicButton.gameObject.SetActive(true);

                SetServerInfo("Offline", "None");
                _isMatchmaking = false;
            }
        }

        public void BackTo(RectTransform newPanel)
        {
            if (currentPanel != null)
            {
                currentPanel.gameObject.SetActive(false);
            }

            if (newPanel != null)
            {
                newPanel.gameObject.SetActive(true);
            }
            currentPanel = newPanel;            
            if (currentPanel != mainMenuPanel)
            {
                backButton.gameObject.SetActive(true);
                exitButton.gameObject.SetActive(false);
            }
            else
            {
                backButton.gameObject.SetActive(false);
                exitButton.gameObject.SetActive(true);
                musicButton.gameObject.SetActive(true);

                SetServerInfo("Offline", "None");
                _isMatchmaking = false;
            }
        }

        public void DisplayIsConnecting()
        {
            var _this = this;
            infoPanel.Display("Connecting...", "Cancel", () => { _this.backDelegate(); });
        }

        public void SetServerInfo(string status, string host)
        {
            statusInfo.text = status;
            hostInfo.text = host;
        }


        public delegate void BackButtonDelegate();
        public BackButtonDelegate backDelegate;
        public void GoBackButton()
        {
            //ERIC VERSION
            userTrail.RemoveLast();

            BackTo(userTrail.Last.Value);
            if (backDelegate != null)
            {
                backDelegate();
            }
            topPanel.isInGame = false;
        }

        public void OnExitButton()
        {
            Application.Quit();
        }
        public void OnPlayerHasDisconectedMessage()
        {
            infoPanel.infoText.text = "The other Player has disconected";
            infoPanel.gameObject.SetActive(true);
        }
        //Detect when a client disconnects from the Server
        public override void OnServerDisconnect(NetworkConnection connection)
        {
            //Advise the player that someone left the game!
            OnPlayerHasDisconectedMessage();
        }

        // ----------------- Server management

        public void AddLocalPlayer()
        {
            TryToAddPlayer();
        }

        public void RemovePlayer(LobbyPlayer player)
        {
            player.RemovePlayer();
        }

        public void SimpleBackClbk()
        {
            ChangeTo(mainMenuPanel);
        }
                 
        public void StopHostClbk()
        {
            if (_isMatchmaking)
            {
				matchMaker.DestroyMatch((NetworkID)_currentMatchID, 0, OnDestroyMatch);
				_disconnectServer = true;

                //Just in case we want that in the case you left lobby, go back to the menu (the same if you leave the game)
                ChangeTo(mainMenuPanel);
            }
            else
            {
                StopHost();
            }

            //ChangeTo(mainMenuPanel);
        }

        public void StopClientClbk()
        {            
            StopClient();

            if (_isMatchmaking)
            {
                StopMatchMaker();
            }
            ChangeTo(mainMenuPanel);
        }

        public void StopServerClbk()
        {
            StopServer();
            ChangeTo(mainMenuPanel);
        }

        class KickMsg : MessageBase { }
        public void KickPlayer(NetworkConnection conn)
        {
            conn.Send(MsgKicked, new KickMsg());
        }
        
        public void KickedMessageHandler(NetworkMessage netMsg)
        {
            infoPanel.Display("Kicked by Server", "Close", null);
            netMsg.conn.Disconnect();
        }

        //===================

        public override void OnStartHost()
        {
            base.OnStartHost();

            ChangeTo(lobbyPanel);
            backDelegate = StopHostClbk;
            SetServerInfo("Hosting", networkAddress);
        }

		public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
		{
			base.OnMatchCreate(success, extendedInfo, matchInfo);
            _currentMatchID = (System.UInt64)matchInfo.networkId;
		}

		public override void OnDestroyMatch(bool success, string extendedInfo)
		{
			base.OnDestroyMatch(success, extendedInfo);
			if (_disconnectServer)
            {
                StopMatchMaker();
                StopHost();
            }
        }
        
        //allow to handle the (+) button to add/remove player
        public void OnPlayersNumberModified(int count)
        {
            _playerNumber += count;

            int localPlayerCount = 0;
            foreach (PlayerController p in ClientScene.localPlayers)
                localPlayerCount += (p == null || p.playerControllerId == -1) ? 0 : 1;

            addPlayerButton.SetActive(localPlayerCount < maxPlayersPerConnection && _playerNumber < maxPlayers);
        }

        // ----------------- Server callbacks ------------------

        //we want to disable the button JOIN if we don't have enough player
        //But OnLobbyClientConnect isn't called on hosting player. So we override the lobbyPlayer creation
        public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
        {
            //ERIC STUFF
            if (!currentPlayers.ContainsKey(conn.connectionId))
            {
                int[] newInfoPlayer = { 0, 0 };
                currentPlayers.Add(conn.connectionId,newInfoPlayer);
            }
            //------
            GameObject obj = Instantiate(lobbyPlayerPrefab.gameObject) as GameObject;

            LobbyPlayer newPlayer = obj.GetComponent<LobbyPlayer>();
            newPlayer.ToggleJoinButton(numPlayers + 1 >= minPlayers);


            for (int i = 0; i < lobbySlots.Length; ++i)
            {
                LobbyPlayer p = lobbySlots[i] as LobbyPlayer;

                if (p != null)
                {
                    p.RpcUpdateRemoveButton();
                    p.ToggleJoinButton(numPlayers + 1 >= minPlayers);
                }
            }

            return obj;
        }

        public override void OnLobbyServerPlayerRemoved(NetworkConnection conn, short playerControllerId)
        {
            for (int i = 0; i < lobbySlots.Length; ++i)
            {
                LobbyPlayer p = lobbySlots[i] as LobbyPlayer;

                if (p != null)
                {
                    p.RpcUpdateRemoveButton();
                    p.ToggleJoinButton(numPlayers + 1 >= minPlayers);
                }
            }
        }

        public override void OnLobbyServerDisconnect(NetworkConnection conn)
        {
            for (int i = 0; i < lobbySlots.Length; ++i)
            {
                LobbyPlayer p = lobbySlots[i] as LobbyPlayer;

                if (p != null)
                {
                    p.RpcUpdateRemoveButton();
                    p.ToggleJoinButton(numPlayers >= minPlayers);
                }
            }
        }

        public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
        {
            //This hook allows you to apply state data from the lobby-player to the game-player
            //just subclass "LobbyHook" and add it to the lobby object.

            if (_lobbyHooks)
                _lobbyHooks.OnLobbyServerSceneLoadedForPlayer(this, lobbyPlayer, gamePlayer);

            return true;
        }

        // --- Countdown management

        public override void OnLobbyServerPlayersReady()
        {
			bool allready = true;
			for(int i = 0; i < lobbySlots.Length; ++i)
			{
				if(lobbySlots[i] != null)
					allready &= lobbySlots[i].readyToBegin;
			}

            if (allready)
            {
                StartCoroutine(ServerCountdownCoroutine());
                backButton.gameObject.SetActive(false);
                musicButton.gameObject.SetActive(false);
            }
        }

        public IEnumerator ServerCountdownCoroutine()
        {
            float remainingTime = prematchCountdown;
            int floorTime = Mathf.FloorToInt(remainingTime);

            while (remainingTime > 0)
            {
                yield return null;

                remainingTime -= Time.deltaTime;
                int newFloorTime = Mathf.FloorToInt(remainingTime);

                if (newFloorTime != floorTime)
                {//to avoid flooding the network of message, we only send a notice to client when the number of plain seconds change.
                    floorTime = newFloorTime;

                    for (int i = 0; i < lobbySlots.Length; ++i)
                    {
                        if (lobbySlots[i] != null)
                        {//there is maxPlayer slots, so some could be == null, need to test it before accessing!
                            (lobbySlots[i] as LobbyPlayer).RpcUpdateCountdown(floorTime);
                        }
                    }
                }
            }

            for (int i = 0; i < lobbySlots.Length; ++i)
            {
                if (lobbySlots[i] != null)
                {
                    (lobbySlots[i] as LobbyPlayer).RpcUpdateCountdown(0);
                }
            }
            backButton.gameObject.SetActive(false);
            musicButton.gameObject.SetActive(false);
            ServerChangeScene(playScene);
        }

        // ----------------- Client callbacks ------------------

        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);

            infoPanel.gameObject.SetActive(false);

            conn.RegisterHandler(MsgKicked, KickedMessageHandler);

            if (!NetworkServer.active)
            {//only to do on pure client (not self hosting client)
                ChangeTo(lobbyPanel);
                backDelegate = StopClientClbk;
                SetServerInfo("Client", networkAddress);
            }
        }

        public override void OnClientDisconnect(NetworkConnection conn)
        {            
            base.OnClientDisconnect(conn);
            ChangeTo(mainMenuPanel);

            OnPlayerHasDisconectedMessage();
        }        

        public override void OnClientError(NetworkConnection conn, int errorCode)
        {
            ChangeTo(mainMenuPanel);
            infoPanel.Display("Cient error : " + (errorCode == 6 ? "timeout" : errorCode.ToString()), "Close", null);
        }

        // ----------------- ERIC PLAYER SELECTION ------------------
        [Space]
        [Header("Character Selection Manager")]
        [SerializeField] Vector3 player1SpawnPos;
        [SerializeField] Vector3 player2SpawnPos;
        [SerializeField] GameObject character1;
        [SerializeField] GameObject character2;

        public override GameObject OnLobbyServerCreateGamePlayer (NetworkConnection conn, short playerControllerId)
		{
            int[] infoPlayer = currentPlayers[conn.connectionId];
            GameObject chosenCharacter;
            Vector3 chosenSpawnPos;            
            if (infoPlayer[0] == 1) //BENTLEY
            {
                chosenCharacter = character1;
                chosenSpawnPos = player1SpawnPos;
                
            }
            else //BOX
            {
                chosenCharacter = character2;
                chosenSpawnPos = player2SpawnPos;

            }            
            chosenCharacter.GetComponent<PlayerInfo>().playerType = infoPlayer[1];

            GameObject playerPrefab = (GameObject)GameObject.Instantiate (chosenCharacter, chosenSpawnPos, Quaternion.identity);
			return playerPrefab;
		}

        public void SetPlayerTypeLobby(NetworkConnection conn, int characterType, int playerType)
        {
            if (currentPlayers.ContainsKey(conn.connectionId))
            {
                currentPlayers[conn.connectionId][0] = characterType; //0->Box ; 1->Bentley 
                currentPlayers[conn.connectionId][1] = playerType; //0->isClient ; 1->isServer 
            }
        }

        //-----

        // ----------------- ERIC FADE BETWEEN LEVELS ------------------
        #region FadeBetweenScenes
        public void LoadScene(string sceneName)
        {
            musicButton.gameObject.SetActive(false);

            StartCoroutine(FadeToScene(sceneName));
        }

        IEnumerator FadeToScene(string sceneName)
        {
            float fadeTime = Camera.main.GetComponent<Fading>().BeginFade(1);
            yield return new WaitForSeconds(fadeTime);
            ServerChangeScene(sceneName); 
        }
        #endregion
    }
}
