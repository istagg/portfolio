/// <summary>
/// Author:    Isaac Stagg
/// Partner:   Nate Tripp
/// Date:      4/15/2022
/// Course:    CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Isaac Stagg and Nate Tripp - This work may not be copied for use in Academic Coursework.
///
/// I, Isaac Stagg and Nate Tripp, certify that I wrote this code from scratch and did not copy it in part or whole from
/// another source.  All references used in the completion of the assignment are cited in my README file.
///
/// File Contents
/// This file contains the GUI control functionality.
/// </summary>
using AgarioModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Reflection;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ClientGUI {

    /// <summary>
    /// Contains the GUI custom functionality.
    /// </summary>
    public partial class ClientGui : Form {

        /// <summary>
        /// Reference to a Client Controller to handle game events.
        /// </summary>
        private readonly ClientController _controller;

        /// <summary>
        /// Indicates whether connected to a game session or not.
        /// </summary>
        private bool _connected = false;

        /// <summary>
        /// Timer object used to tick the game server forward.
        /// </summary>
        private readonly System.Windows.Forms.Timer _timer;

        /// <summary>
        /// Indicates the desired game scale.
        /// </summary>
        private double _desiredScale = 5.0F;

        /// <summary>
        /// Indicates the current game scale.
        /// </summary>
        private double _currentScale = 5.0F;

        /// <summary>
        /// Indicates the desired game window transform.
        /// </summary>
        private double _desiredTransform = 50F;

        /// <summary>
        /// Indicates the current game window transform.
        /// </summary>
        private double _currentTransform = 50F;

        /// <summary>
        /// Indicates whether or not the player is dead.
        /// </summary>
        private bool _dead = true;

        /// <summary>
        /// Used to log information.
        /// </summary>
        private readonly ILogger _logger;

        private DateTime _startTime;

        /// <summary>
        /// Constructs a Client GUI object.
        /// </summary>
        /// <param name="logger">Used to log information.</param>
        public ClientGui() {
            InitializeComponent();
            _controller = new ClientController(NullLogger.Instance, OnConnectCallback, OnDisconnectCallback, OnDeathCallback);
            _logger = NullLogger.Instance;
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, GamePanel, new object[] { true });

            _timer = new() {
                Interval = 1000 / 30
            };
            _timer.Tick += (_, _) => GamePanel.Invalidate();
        }

        /// <summary>
        /// Called when the controller connects to a game session.
        /// </summary>
        private void OnConnectCallback() {
            _startTime = DateTime.Now;
            _logger.LogInformation("Connected to game session.");
            _dead = false;
            PanelStartUp.Visible = false;
            _connected = true;
            _timer.Start();
        }

        /// <summary>
        /// Called when the client is disconnected from server.
        /// </summary>
        private void OnDisconnectCallback() {
            _logger.LogInformation("Disconnected from game server.");
            PanelStartUp.Visible = true;
            _connected = false;
            _timer.Stop();
        }

        /// <summary>
        /// Called when the player dies.
        /// </summary>
        private void OnDeathCallback() {
            _logger.LogInformation("Player has died.");
            _dead = true;
            PanelStartUp.Visible = true;
            TxtBoxName.Enabled = false;
            TxtBoxServer.Enabled = false;
            BtnStart.Text = "Restart";
            _timer.Stop();
            RecordPlayerStatsToDb();
        }

        /// <summary>
        /// Draws the game world onto the screen.
        /// </summary>
        private void Draw_Scene(object _, PaintEventArgs e) {
            if (_connected) {
                SolidBrush brush = new(Color.LightGray);
                e.Graphics.FillRectangle(brush, GamePanel.ClientRectangle);
                if (_controller.Player != null) {
                    LblStatsX.Text = "X: " + _controller.Player.X;
                    LblStatsY.Text = "Y: " + _controller.Player.Y;
                    LblStatsMass.Text = "Mass: " + _controller.Player.Mass;

                    DrawFullWorld(e);

                    var position = CalculateMouseToPlayer();
                    _controller.Move(position.X, position.Y);
                }
            }
        }

        /// <summary>
        /// Calculates the mouse position relative to the player and returns a Point object.
        /// </summary>
        /// <returns>A point object representing the mouse position relative to the player.</returns>
        private Point CalculateMouseToPlayer() {
            if (_controller.Player == null) { return new Point(0, 0); }

            var playerPosition = _controller.Player.Location;
            var mousePosition = Cursor.Position;
            mousePosition = GamePanel.PointToClient(mousePosition);

            mousePosition.X -= (GamePanel.Width / 2);
            mousePosition.Y -= (GamePanel.Height / 2);
            mousePosition.X = Math.Clamp(mousePosition.X, -GamePanel.Width / 2, GamePanel.Width / 2);
            mousePosition.Y = Math.Clamp(mousePosition.Y, -GamePanel.Height / 2, GamePanel.Height / 2);

            return new Point((int)(playerPosition.X + mousePosition.X), (int)(playerPosition.Y + mousePosition.Y));
        }

        /// <summary>
        /// Calculates the radius of the game object according to its mass.
        /// </summary>
        /// <param name="gameObj">The game object to calculate the radius of.</param>
        /// <returns>The radius of the game object.</returns>
        private static double CalculateRadiusOfGameObject(GameObject gameObj) { return Math.Sqrt(gameObj.Mass / Math.PI); }

        /// <summary>
        /// Draws the world onto the <see cref="GamePanel"/>.
        /// </summary>
        /// <param name="e">Used to draw onto the game world.</param>
        private void DrawFullWorld(PaintEventArgs e) {
            _logger.LogInformation("Drawing gamne world.");
            var state = e.Graphics.Save();

            var player = _controller.Player;

            CalculateTransforms();
            var playerX = player.X / World.Width * GamePanel.Width;
            var playerY = player.Y / World.Height * GamePanel.Height;
            e.Graphics.ScaleTransform((float)_currentScale, (float)_currentScale);
            e.Graphics.TranslateTransform((float)-(playerX - _desiredTransform), (float)-(playerY - _desiredTransform));

            FontFamily fontFamily = new("Arial");
            Font font = new(
               fontFamily,
               10,
               FontStyle.Regular,
               GraphicsUnit.Pixel);

            foreach (var gameObjectPair in _controller.World.GameObjectList) {
                var gameObject = gameObjectPair.Value;
                SolidBrush objectBrush = new(Color.FromArgb(gameObject.ARGBColor));

                var screen_x = (int)(gameObject.X / World.Width * GamePanel.Width);
                var screen_y = (int)(gameObject.Y / World.Height * GamePanel.Height);

                float radius = (float)(CalculateRadiusOfGameObject(gameObject) / 5F); // Scales game object back a bit.

                e.Graphics.FillEllipse(objectBrush, (screen_x - radius), (screen_y - radius), (2 * radius), (2 * radius));

                if (gameObject is Player p) {
                    e.Graphics.DrawString(p.Name, font, new SolidBrush(Color.Black), new PointF(screen_x, (float)(screen_y + radius)));
                }
            }
            e.Graphics.Restore(state);
        }

        /// <summary>
        /// Calculates the world transforms to center the game around the player.
        /// </summary>
        private void CalculateTransforms() {
            var player = _controller.Player;

            // These values were calculated by determining desired scale for the player mass being equal
            // to 50 and player mass being equal to 5000. When the mass is 50 our scale should be 5 and
            // when the mass is 5000 our scale should equal 1. Using these values we can derive the
            // slope by allowing the mass to equal the Y values and the scale to equal the X values.
            // Then by using the slope formula we can determine the value of M for y = mx + b and then
            // simply solve for b using one of our inputs.
            const double SCALE_M = -1237;
            const double SCALE_Y_B = 6237;
            _desiredScale = Math.Max((player.Mass - SCALE_Y_B) / SCALE_M, 2); // Clamp scale to no less then 2.

            // These values were calculated by determining desired transform values for a variety of
            // player masses. Using those we were able to plot points on a graph and determine the
            // rough for the translate transform. We determined this equation to be y = mx^2 + b and were
            // able to solve for the values M and b listed below.
            const double TRANSFORM_M = 0.000007;
            const double TRANSFORM_B = 50;
            _desiredTransform = (TRANSFORM_M * Math.Pow(player.Mass, 2)) + TRANSFORM_B;

            if (_currentScale > _desiredScale) {
                _currentScale -= 0.01F;
            } else if (_currentScale < _desiredScale) {
                _currentScale += 0.01F;
            }

            if (_currentTransform > _desiredTransform) {
                _currentTransform -= 0.1F;
            } else if (_currentTransform < _desiredTransform) {
                _currentTransform = 0.1F;
            }
        }

        /// <summary>
        /// Starts a game session.
        /// </summary>
        private void BtnStart_Click(object sender, EventArgs e) {
            try {
                if (_dead) {
                    if (string.IsNullOrWhiteSpace(TxtBoxName.Text)) {
                        LblErrorMessages.Text = "Name field required!";
                    } else if (string.IsNullOrWhiteSpace(TxtBoxServer.Text)) {
                        LblErrorMessages.Text = "Server field required!";
                    } else {
                        LblErrorMessages.Text = string.Empty;
                        _logger.LogInformation("Starting game.");
                        if (!_connected) {
                            _controller.Connect(TxtBoxName.Text, TxtBoxServer.Text);
                        } else {
                            _controller.StartGame();
                        }
                    }
                }
            } catch (Exception ex) {
                LblErrorMessages.Text = $"Failed to connect to server! {ex.Message}";
                _logger.LogError($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles key down events.
        /// </summary>
        private void ClientGui_KeyDown(object sender, KeyEventArgs e) {
            _logger.LogDebug($"{e.KeyCode} key pressed.");
            if (e.KeyCode == Keys.Space) {
                var mousePoint = CalculateMouseToPlayer();
                _controller.Split(mousePoint.X, mousePoint.Y);
            } else if (e.KeyCode == Keys.Enter && !_connected) {
                BtnStart_Click(sender, e);
            }
        }

        /// <summary>
        /// Builts the connection string required to access the database
        /// </summary>
        /// <returns>SqlConnectionStringBuilder</returns>
        private static string BuildConnectionString() {
            var builder = new ConfigurationBuilder();

            builder.AddUserSecrets<ClientGui>();
            IConfigurationRoot Configuration = builder.Build();

            return new SqlConnectionStringBuilder() {
                DataSource = Configuration.GetValue<string>("ServerURL"),
                InitialCatalog = Configuration.GetValue<string>("DBName"),
                UserID = Configuration.GetValue<string>("UserName"),
                Password = Configuration.GetValue<string>("DBPassword")
            }.ConnectionString;
        }

        /// <summary>
        /// Helper method to add player statistics to database.
        /// </summary>
        private void RecordPlayerStatsToDb() {
            try {
                using SqlConnection con = new(BuildConnectionString());
                con.Open();
                // Instead of passing in a query, we programmed a sproc directly into the database.
                // Here we are simply executing the sproc and passing in the required information.
                // The sproc then handles the heavy lifting and actually runs the insert commands.
                // This makes it easier to modify in the future.
                string query = @$"EXEC	[dbo].[insertGameRecord]
                                  @player_name = N'{_controller.Player.Name}',
		                          @player_score = {(int)_controller.Player.Mass},
		                          @start_time = N'{_startTime}',
		                          @end_time = N'{DateTime.Now}'";
                using SqlCommand command = new(query, con);
                command.ExecuteNonQuery();
            } catch (SqlException exception) {
                Console.WriteLine($"Error in SQL connection: {exception.Message}");
            }
        }
    }
}