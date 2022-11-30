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
/// This file contains the Player object used in the Agario project.
/// </summary>

using System.Text.Json.Serialization;

namespace AgarioModels {

   /// <summary>
   /// Represents a player object used by the Agario project.
   /// </summary>
   public class Player : GameObject {

      /// <summary>
      /// The name of this player.
      /// </summary>
      public string Name { get; }

      /// <summary>
      /// Used to de-serialize a player from a json string.
      /// </summary>
      /// <param name="X">The X position in the game world.</param>
      /// <param name="Y">The Y position in the game world.</param>
      /// <param name="ID">The ID of the game object.</param>
      /// <param name="ARGBColor">The color of the game object.</param>
      /// <param name="Mass">The mass of the game object.</param>
      /// <param name="Name">The name of this player.</param>
      [JsonConstructor]
      public Player(float X, float Y, long ID, int ARGBColor, float Mass, string Name) : base(X, Y, ID, ARGBColor, Mass) {
         this.Name = Name;
      }
   }
}