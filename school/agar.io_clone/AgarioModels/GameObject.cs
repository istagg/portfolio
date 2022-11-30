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
/// This file contains the base game object used in the Agario project.
/// </summary>

using System.Drawing;
using System.Text.Json.Serialization;

namespace AgarioModels {

   /// <summary>
   /// Represents a game object in the Agario game.
   /// </summary>
    public class GameObject {
      /// <summary>
      /// The ID of the game object.
      /// </summary>
        public long ID { get; set; }

      /// <summary>
      /// The location of the game object as a <see cref="PointF"/>.
      /// </summary>
        public PointF Location { get; set; }

      /// <summary>
      /// The X coordinate of the game object.
      /// </summary>
        public float X => Location.X;

      /// <summary>
      /// The Y coordinate of the game object.
      /// </summary>
        public float Y => Location.Y;

      /// <summary>
      /// The color of the game object as an ARGB value.
      /// </summary>
        public int ARGBColor { get; set; }

      /// <summary>
      /// The mass of the game object.
      /// </summary>
        public float Mass { get; set; }

      /// <summary>
      /// Basic constructor to construct empty game object.
      /// </summary>
        public GameObject() {
        }

      /// <summary>
      /// Constructor used by Json de-serialization.
      /// </summary>
      /// <param name="X">The X position in the game world.</param>
      /// <param name="Y">The Y position in the game world.</param>
      /// <param name="ID">The ID of the game object.</param>
      /// <param name="ARGBColor">The color of the game object.</param>
      /// <param name="Mass">The mass of the game object.</param>
      [JsonConstructor]
        public GameObject(float X, float Y, long ID, int ARGBColor, float Mass) {
            Location = new PointF(X, Y);
            this.ID = ID;
            this.ARGBColor = ARGBColor;
            this.Mass = Mass;
        }
    }
}