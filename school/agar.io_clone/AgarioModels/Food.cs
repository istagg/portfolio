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
/// This file contains the Food object used in the Agario project.
/// </summary>

namespace AgarioModels {

   /// <summary>
   /// Represents a food game object.
   /// </summary>
   public class Food : GameObject {


      /// <summary>
      /// Basic constructor that forwards its arguments to the base class <see cref="GameObject"/>.
      /// </summary>
      /// <param name="X">The X position in the game world.</param>
      /// <param name="Y">The Y position in the game world.</param>
      /// <param name="ID">The ID of the game object.</param>
      /// <param name="ARGBColor">The color of the game object.</param>
      /// <param name="Mass">The mass of the game object.</param>
      public Food(float X, float Y, long ID, int ARGBColor, float Mass) : base(X, Y, ID, ARGBColor, Mass) { }
   }
}