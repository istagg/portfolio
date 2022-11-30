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
/// This file contains the ChatServer GUI code. It enables ease of use with the Networking class which acts as its back end.
/// </summary>


using Communications;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;

namespace ChatServer {
   /// <summary>
   /// Chat server GUI.
   /// </summary>
   public partial class ChatServer : Form {


      private readonly ILogger _logger;
      private readonly Networking _networking;
      private bool _running = false;
      public HashSet<Networking> _connected = new();


      /// <summary>
      /// Constructs a ChatServer.
      /// </summary>
      /// <param name="logger">A logger object to generate server logs.</param>
      public ChatServer(ILogger<ChatServer> logger) {
         InitializeComponent();
         _logger = logger;
         _networking = new Networking(logger, OnConnect, OnDisconnect, OnMessage, '\n');

         TxtBoxServerName.Text = Environment.MachineName;
         TxtBoxServerIP.Text = Networking.GetLocalIPAddress();
      }

      /// <summary>
      /// Updates the participant list GUI element.
      /// </summary>
      private void UpdateParticipantsList() {
         TxtBoxChatParticipants.Text = string.Join("\n", _connected.Select(c=>c.ID));
      }

      /// <summary>
      /// Called every time a client connects to the server. 
      /// </summary>
      /// <param name="channel">The client that connected.</param>
      private void OnConnect(Networking channel) {
         lock (_connected) {
            _connected.Add(channel);
            UpdateParticipantsList();
            Invoke(() => TxtBoxChat.AppendText($"{channel.ID} - connected to server\n"));
         }
      }

      /// <summary>
      /// Called every time a client disconnects.
      /// </summary>
      /// <param name="channel">The client that disconnected.</param>
      private void OnDisconnect(Networking channel) {
         lock (_connected) {
            _connected.Remove(channel);
            UpdateParticipantsList();
         }
      }

      /// <summary>
      /// Called every time a client sends a message.
      /// </summary>
      /// <param name="channel">The client that sent the message.</param>
      /// <param name="message">The message that was sent.</param>
      private void OnMessage(Networking channel, string message) {
         if (message.StartsWith("Command Participants")) {
            _networking.Send($"Command Participants,{string.Join(",", _connected.Select(c=>c.ID))}");
         }
         else if (message.StartsWith("Command Name")) {
            channel.ID = message.Replace("Command Name", string.Empty).Trim();
            UpdateParticipantsList();
         }
         else {
            _networking.Send(message);
         }
         TxtBoxChat.AppendText(message + "\n");
      }

      /// <summary>
      /// Causes the server to toggle between running and stopped.
      /// </summary>
      private void BtnServer_Click(object _, EventArgs __) {
         if (!_running) {
            _networking.WaitForClients(11000, true);
            BtnServer.Text = "Stop Server";
            _running = true;
         }
         else {
            _networking.StopWaitingForClients();
            BtnServer.Text = "Restart Server";
            _running = false;
         }
      }
   }
}