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
/// This file contains the World object used in the Agario project.
/// </summary>
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace AgarioModels {

   /// <summary>
   /// Represents the Agario game world.
   /// </summary>
   public class World {
      /// <summary>
      /// The height of the game world.
      /// </summary>
      public static readonly int Height = 5000;

      /// <summary>
      /// The width of the game world.
      /// </summary>
      public static readonly int Width = 5000;

      /// <summary>
      /// Obtains a dictionary of each game object with its id as the key.
      /// </summary>
      public IDictionary<long, GameObject> GameObjectList => _gameObjects;

      /// <summary>
      /// Simple logger to log messages.
      /// </summary>
      private readonly ILogger _logger;

      /// <summary>
      /// Holds a dictionary of this worlds game objects with the ids as the key.
      /// </summary>
      private readonly ConcurrentDictionary<long, GameObject> _gameObjects = new();

      /// <summary>
      /// Constructs a new game world.
      /// </summary>
      /// <param name="logger">Used to log messages.</param>
      public World(ILogger logger) {
         _logger = logger;
         _logger.Log(LogLevel.Debug, "Creating game world.");
      }

      public void AddFood(IEnumerable<Food> foodArray) {
         _logger.LogInformation($"Adding {foodArray.Count()} to the world.");
         Parallel.ForEach(foodArray, (food) => _gameObjects.AddOrUpdate(food.ID, food, (_, _) => food));
      }

      public void RemoveGameObjects(IEnumerable<long> deadPlayers) {
         foreach (long id in deadPlayers) {
            GameObjectList.Remove(id);
            _logger.LogInformation($"Removing object: {id} from the world.");

         }
      }

      public void UpdatePlayers(IEnumerable<Player> players) {
         _logger.LogInformation("Updating player list.");
         Parallel.ForEach(players, (player) => _gameObjects.AddOrUpdate(player.ID, player, (_, _) => player));
      }

      public GameObject GetObjectById(long id) {
         _ = _gameObjects.TryGetValue(id, out var gameObject);
         return gameObject;
      }
   }
}