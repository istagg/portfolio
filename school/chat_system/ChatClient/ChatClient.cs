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
/// This file contains the code for the GUI of the chat client.
/// </summary>

using Communications;
using Microsoft.Extensions.Logging;

namespace ChatClient {

    /// <summary>
    /// This class is a partial class for the chat client. This handles all button clicks and events.
    /// </summary>
    public partial class ChatClient : Form {
        private Networking _network;
        private bool _connected = false;
        private ILogger<ChatClient> _logger;

        /// <summary>
        /// Constructor for the chat client.
        /// </summary>
        /// <param name="logger">Custom file logger to use</param>
        public ChatClient(ILogger<ChatClient> logger) {
            InitializeComponent();
            _network = new(logger, OnConnect, OnDisconnect, OnMessage, '\n');
            _logger = logger;
        }

        /// <summary>
        /// Delegate method for the networking object. Notifies user of connection.
        /// </summary>
        /// <param name="network">The networking object who calls this method</param>
        public void OnConnect(Networking network) {
            _network.Send("Command Name " + _network.ID);
            Invoke(() => chatTextBox.AppendText("Connected to server\n"));
        }

        /// <summary>
        /// Delegate method for the networking object. Appends the message to the chat box.
        /// </summary>
        /// <param name="network">Networking object that calls this method</param>
        /// <param name="message">Message that is being sent across the server</param>
        public void OnMessage(Networking network, string message) {
            if (message.StartsWith("Command Participants")) {
                var participants = message.Replace("Command Participants", "").Split(",");
                Invoke(() => participantsTextBox.Text = string.Join("\n", participants.Where(s => s.Trim().Length > 0)));
            } else {
                Invoke(() => chatTextBox.AppendText(message + "\n"));
            }
        }
         /// <summary>
         /// Delegate method for the networking object. Resets the GUI after disconnecting.
         /// </summary>
         /// <param name="network"></param>
        public void OnDisconnect(Networking network) {
            Invoke(() => {
                chatTextBox.AppendText("Disconnected\n");
                connectedLabel.Visible = false;
                connectButton.Visible = true;
            });
        }
         /// <summary>
         /// Handles clicking the connect button. Disables the connect button and starts the process of awaiting messages.
         /// </summary>
        private void connectButton_Click(object sender, EventArgs e) {
            Task.Run(() => {
                if (nameTextBox.Text.Length == 0) {
                    nameMissingLabel.Visible = true;
                } else {
                    Invoke(() => chatTextBox.AppendText("Attempting to connect...\n"));
                    nameMissingLabel.Visible = false;
                    _network.ID = nameTextBox.Text;
                    string host = addressTextBox.Text;
                    try {
                        _network.Connect(host, 11000);
                        _connected = true;
                        Invoke(() => {
                            connectedLabel.Visible = true;
                            connectButton.Visible = false;
                        });
                        _network.ClientAwaitMessagesAsync();
                    } catch (Exception ex) {
                        Invoke(() => chatTextBox.AppendText("Error - " + ex.Message + "\n"));
                    }
                }
            });
        }
         /// <summary>
         /// Disconnects the client from the server when the form is being closed.
         /// </summary>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            _network.Disconnect();
        }
         /// <summary>
         /// Method to detect the enter button being pressed in the message text box. Sends the message in the text box.
         /// </summary>
        private void messageTextBox_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Enter && _connected) {
                string text = messageTextBox.Text;
                Task.Run(() => {
                    Invoke(() => messageTextBox.Enabled = false);
                    _network.Send(text);
                    Invoke(() => {
                        messageTextBox.Clear();
                        messageTextBox.Enabled = true;
                    });
                });
            }
        }

        /// <summary>
        /// Sends a command to the server to get the list of participants
        /// </summary>
        private void participantsButton_Click(object sender, EventArgs e) {
            _network.Send("Command Participants");
        }

        /// <summary>
        /// Helper method to enable the enter key while typing user name in and connecting to server.
        /// </summary>
        private void nameTextBox_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == (char)Keys.Enter && !_connected) {
                connectButton_Click(sender, e);
            }
        }
    }
}