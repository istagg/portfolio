/// <summary>
/// Author:    Isaac Stagg
/// Partner:   Nate Tripp
/// Date:      4/8/2022
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and Isaac Stagg and Nate Tripp - This work may not be copied for use in Academic Coursework.
///
/// I, Isaac Stagg and Nate Tripp, certify that I wrote this code from scratch and did not copy it in part or whole from
/// another source.  All references used in the completion of the assignment are cited in my README file.
///
/// File Contents
/// This file contains the controller for the agario project.
/// </summary>

using AgarioModels;
using Communications;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ClientGUI {

    public class ClientController {

        public delegate void OnConnectCallback();
        public delegate void OnDisconnectCallback();
        public delegate void OnDeathCallback();

      public World World { get; private set; }
        public Player Player => (Player)World.GetObjectById(_playerID);

        private Networking _network;
        private ILogger _logger;
        private bool _connected;
        private string _playerName;
        private long _playerID;

        private OnConnectCallback _onConnect;
        private OnDisconnectCallback _onDisconnect;
        private OnDeathCallback _onDeathCallback;

        public ClientController(ILogger logger, OnConnectCallback onConnect, OnDisconnectCallback onDisconnect, OnDeathCallback onDeathCallback) {
            _logger = logger;
            _onConnect = onConnect;
            _onDisconnect = onDisconnect;
            _onDeathCallback = onDeathCallback;
            _network = new(logger, OnConnect, OnDisconnect, OnMessage, '\n');
        }

        /// <summary>
        /// This function connects the client to the server and begins the game
        /// </summary>
        /// <param name="name">User's name</param>
        /// <param name="host">Address to connect to. Default is localhost</param>
        public void Connect(string name, string host) {
            if (!_connected) {
                _playerName = name;
                World = new(_logger);
                _network.Connect(host, 11000);
                _logger.Log(LogLevel.Information, _playerName + " connected to the server.");
            }
        }

        /// <summary>
        /// This function sends a move command to the server indicating for the player to move.
        /// </summary>
        /// <param name="x">The x coordinate to move towards</param>
        /// <param name="y">The y coordinate to move towards</param>
        public void Move(int x, int y) {
            x = Math.Clamp(x, 0, World.Width);
            y = Math.Clamp(y, 0, World.Height);
            var message = string.Format(Protocols.CMD_Move, x, y);
            _network.Send(message);
        }

        /// <summary>
        /// This function sends a split command to the server where the player splits in half.
        /// </summary>
        /// <param name="x">The x coordinate to split towards</param>
        /// <param name="y">The y coordinate to split towards</param>
        public void Split(int x, int y) {
            x = Math.Clamp(x, 0, World.Width);
            y = Math.Clamp(y, 0, World.Height);
            var message = string.Format(Protocols.CMD_Split, x, y);
            _network.Send(message);
            _logger.Log(LogLevel.Information, _playerName + " split.");
        }

        /// <summary>
        /// This function is a helper to start a game
        /// </summary>
        public void StartGame() {
             CMDStartGame();
            _onConnect();
            _network.ClientAwaitMessagesAsync();
        }

        /// <summary>
        /// This method is called when a client connects to the server.
        /// </summary>
        /// <param name="network">The networking object used for the connection</param>
        private void OnConnect(Networking network) {
            _connected = true;
            StartGame();
        }

        /// <summary>
        /// This method is called when a client disconnects from the server.
        /// </summary>
        /// <param name="network">The networking object used for the connection</param>
        private void OnDisconnect(Networking network) {
            _connected = false;
            _onDisconnect();
            _logger.Log(LogLevel.Information, _playerName + " has disconnected.");
        }

        /// <summary>
        /// This method is called when the client receives a message from the server.
        /// </summary>
        /// <param name="network">The networking object used for the connection</param>
        /// <param name="message">The message sent from the server</param>
        private void OnMessage(Networking network, string message) {
            var commandLength = (message.IndexOf('}') + 1);
            var command = message[..commandLength];
            switch (command) {
                case Protocols.CMD_Player_Object:
                    CMDPlayerObject(message.Remove(0, commandLength));
                    break;

                case Protocols.CMD_Food:
                    CMDFood(message.Remove(0, commandLength));
                    break;

                case Protocols.CMD_Dead_Players:
                case Protocols.CMD_Eaten_Food:
                    RemoveObjects(message.Remove(0, commandLength));
                    break;

                case Protocols.CMD_HeartBeat:
                    break;

                case Protocols.CMD_Update_Players:
                    CMDUpdatePlayers(message.Remove(0, commandLength));
                    break;
            }
        }

        /// <summary>
        /// A helper method used when the start game command is received from the server
        /// </summary>
        private void CMDStartGame() {
            _network.Send(string.Format(Protocols.CMD_Start_Game, _playerName));
        }

        /// <summary>
        /// A helper method used the player object command is sent from the server
        /// </summary>
        /// <param name="message">The JSON of the player's ID</param>
        private void CMDPlayerObject(string message) {
            _playerID = long.Parse(message);
        }

        /// <summary>
        /// A helper method used when the server sends out a list of game objects
        /// </summary>
        /// <param name="message">The JSON of the list of game objects</param>
        private void CMDFood(string message) {
            var foodArray = JsonSerializer.Deserialize<Food[]>(message);
            World?.AddFood(foodArray ?? Array.Empty<Food>());
        }

        /// <summary>
        /// A helper method used when the server sends out a list of game objects to remove.
        /// </summary>
        /// <param name="message">A JSON list of game objects to remove</param>
        private void RemoveObjects(string message) {
            var playerIds = JsonSerializer.Deserialize<long[]>(message);
            if(playerIds?.Contains(_playerID) ?? false){
               _onDeathCallback();
            }
            
            World?.RemoveGameObjects(playerIds ?? Array.Empty<long>());
        }

        /// <summary>
        /// A helper method used when the server sends out the update players command
        /// </summary>
        /// <param name="message">The JSON list of players</param>
        private void CMDUpdatePlayers(string message) {
            var playerArray = JsonSerializer.Deserialize<Player[]>(message);
            World?.UpdatePlayers(playerArray ?? Array.Empty<Player>());
        }
    }
}