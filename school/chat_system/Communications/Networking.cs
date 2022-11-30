/// <summary>
/// Author:    Isaac Stagg
/// Partner:   Nate Tripp
/// Date:      4/2/2022
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and Isaac Stagg and Nate Tripp - This work may not be copied for use in Academic Coursework. 
/// 
/// I, Isaac Stagg and Nate Tripp, certify that I wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in my README file. 
/// 
/// File Contents 
/// This file contains code for TCP connections between a server and a client. This code allows the clients to send messages between each
/// other and to be able to issue basic commands such as giving themselves a name.
/// </summary>

using Microsoft.Extensions.Logging;
using System.Net.Sockets;
using System.Text;
using System;
using System.Net;
using FileLogger;


namespace Communications {

    /// <summary>
    /// Represents a network connection, or a list of network connections if this object is behaving as a server.
    /// </summary>
    public class Networking {
      
        /// <summary>
        /// Message arrived delegate. Called when a client receives a new message.
        /// </summary>
        /// <param name="channel">The client that received the message.</param>
        /// <param name="message">The message received.</param>
        public delegate void ReportMessageArrived(Networking channel, string message);

        /// <summary>
        /// Disconnected delegate. Called when a client disconnects.
        /// </summary>
        /// <param name="channel">The client that disconnected.</param>
        public delegate void ReportDisconnect(Networking channel);

        /// <summary>
        /// Client connected delegate. Called when a connection is established.
        /// </summary>
        /// <param name="channel">The connected client.</param>
        public delegate void ReportConnectionEstablished(Networking channel);

        /// <summary>
        /// The ID of this Networking object.
        /// </summary>
        public string ID { get; set; } = GetLocalIPAddress();

        private readonly List<Networking> _tcpClients = new();

        private readonly ReportConnectionEstablished _onConnect;
        private readonly ReportMessageArrived _onMessageArrived;
        private readonly ReportDisconnect _onDisconnect;
        private readonly char _terminationCharacter;
        private readonly ILogger _logger;

        private TcpListener _networkListener;
        private bool _shouldListen = false;
        private TcpClient _client;

        private CancellationTokenSource _cancellationTokenSource = new();
        
        /// <summary>
        /// Constructs a networking object to use with TCP connections.
        /// </summary>
        /// <param name="logger">A logger to record TCP related messages.</param>
        /// <param name="onConnect">Called every time a connection is established.</param>
        /// <param name="onDisconnect">Called every time a connection is disconnected.</param>
        /// <param name="onMessage">Called every time a message is received.</param>
        /// <param name="terminationCharacter">The character for which a message is terminated with.</param>
        public Networking(ILogger logger, ReportConnectionEstablished onConnect, ReportDisconnect onDisconnect, ReportMessageArrived onMessage, char terminationCharacter) {
            _onConnect = onConnect;
            _onMessageArrived = onMessage;
            _onDisconnect = onDisconnect;
            _terminationCharacter = terminationCharacter;
            _logger = logger;
        }

        /// <summary>
        /// Causes this object to connect to the specified host and port.
        /// </summary>
        /// <param name="host">The host to connect to.</param>
        /// <param name="port">The port to connect to.</param>
        /// <exception cref="Exception">Thrown if the client is unable to connect.</exception>
        public void Connect(string host, int port) {
            _client = new TcpClient(host, port);
            _tcpClients.Add(this);
            if (_client.Connected) {
                _logger.Log(LogLevel.Information, $"Connected to server {host}:{port}");
                _shouldListen = true;
                _onConnect(this);
            } else {
                throw new Exception("unspecified connection error");
            }
        }

        /// <summary>
        /// Tells the client to wait for messages from the server.
        /// </summary>
        /// <param name="infinite">True if this should run forever, false for one message request.</param>
        public async void ClientAwaitMessagesAsync(bool infinite = true) {
            _cancellationTokenSource = new();
            _shouldListen = true;
            StringBuilder dataBacklog = new();
            byte[] buffer = new byte[4096];
            NetworkStream stream = _client.GetStream();

            if (stream == null) return;

            try {
            
                while (infinite && _shouldListen) {
                    int total = await stream.ReadAsync(buffer, _cancellationTokenSource.Token);
                    string currentData = Encoding.UTF8.GetString(buffer, 0, total);
                    dataBacklog.Append(currentData);
                    var intermittentMessage = dataBacklog.ToString();
                    while (intermittentMessage.Contains(_terminationCharacter)) {
                        string message = intermittentMessage[..intermittentMessage.IndexOf(_terminationCharacter)].Trim();
                        _onMessageArrived(this, dataBacklog.ToString().Replace("\n", ""));
                        dataBacklog.Remove(0, intermittentMessage.IndexOf(_terminationCharacter) + 1);
                        intermittentMessage = dataBacklog.ToString();
                    }
                }
            }
            catch {
                Disconnect();
            }
        }
        
        /// <summary>
        /// Causes a server to wait and setup connections from clients requesting to connect.
        /// </summary>
        /// <param name="port">The port they should connect on.</param>
        /// <param name="infinite">True if this should run forever, false for one message request.</param>
        public async void WaitForClients(int port, bool infinite) {
            try {
               _cancellationTokenSource = new();
               _networkListener = new TcpListener(IPAddress.Any, port);
               _networkListener.Start();
               _shouldListen = true;
               while (infinite && _shouldListen) {
                   var connection = await _networkListener.AcceptTcpClientAsync(_cancellationTokenSource.Token);
                   
                   var networking = new Networking(_logger, _onConnect, _onDisconnect, _onMessageArrived, _terminationCharacter);

                   _logger.Log(LogLevel.Information, $"New client connected from {networking.ID}");
                   _onConnect(networking);
                   lock (_tcpClients) {
                       _tcpClients.Add(networking);
                   }
                   networking._client = connection;
                   networking.ClientAwaitMessagesAsync();
               }
            }
            catch(Exception ex) {
                _logger.Log(LogLevel.Debug, ex.Message);
                _networkListener.Stop();
            }
        }

        /// <summary>
        /// Stops the server from waiting for new clients. 
        /// </summary>
        public void StopWaitingForClients() {
            _logger.Log(LogLevel.Information, "Shutting down server.");
            _cancellationTokenSource.Cancel();
            _shouldListen = false;
            foreach(var networking in _tcpClients) {
                networking._client?.Close();
            }
        }

        /// <summary>
        /// Causes the client to disconnect from the server.
        /// </summary>
        public void Disconnect() {
            _cancellationTokenSource.Cancel();
            _logger.Log(LogLevel.Information, $"Client {ID} disconnected.");
            if (_client != null) {
                _tcpClients.Remove(this);
                _client.Close();
                _onDisconnect(this);
            }
            _shouldListen=false;
        }

        /// <summary>
        /// Sends a message to all clients that are managed by this networking object.
        /// </summary>
        /// <param name="text">The text to send.</param>
        public async void Send(string text) {
            if(!_shouldListen)return;
            List<Networking> toRemove = new();
            List<Networking> toSendTo = new();

            if (text.Contains(_terminationCharacter)) {
                text = text.Replace(_terminationCharacter.ToString(), string.Empty);
            }

            text += _terminationCharacter;

            byte[] messageBytes = Encoding.UTF8.GetBytes(text);

            lock (_tcpClients) {
                foreach (var client in _tcpClients) {
                    toSendTo.Add(client);
                }
            }


            foreach (var client in toSendTo) {
                try {
                    await client._client.GetStream().WriteAsync(messageBytes);
                } catch {
                    toRemove.Add(client);
                }
            }

            lock (_tcpClients) {
                foreach (var client in toRemove) {
                    _tcpClients.Remove(client);
                   _logger.Log(LogLevel.Information, $"Client disconnected {client.ID}");
                }
            }
        }
        
        /// <summary>
        /// Obtains this computers local IP address.
        /// </summary>
        /// <returns>The computers local IP address.</returns>
        /// <exception cref="Exception">If there are no network adapters found.</exception>
        /// <remarks>Copied from https://stackoverflow.com/a/6803109/9604892</remarks>
        public static string GetLocalIPAddress() {
           var host = Dns.GetHostEntry(Dns.GetHostName());
           foreach (var ip in host.AddressList) {
              if (ip.AddressFamily == AddressFamily.InterNetwork) {
                 return ip.ToString();
              }
           }
           throw new Exception("No network adapters with an IPv4 address in the system!");
        }
   }
}