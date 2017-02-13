using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;

using Game.Core;
using Game.Utils;
using Game.Services;
using Game.Services.Data;

namespace Game {

  #region [Program class definition]

  /// <summary>
  /// Main application.
  /// </summary>
  internal static class Program {

    #region Main(..)

    /// <summary>
    /// Application entry point.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    static void Main(String[] args) {

      try {

        //
        // Load application configuration
        //
        var url = ConfigurationManager.AppSettings["GameService.Url"];

        Console.WriteLine();
        Console.WriteLine("Connecting to:");
        Console.WriteLine("   {0}", url);

        //
        // Retrieve game data
        //
        using (var svc = new GameServiceClient(url)) {
          var data = Task.Run(() => svc.GetData()).Result;

          Console.WriteLine();
          Console.WriteLine("Input data:");
          Console.WriteLine("   {0}", data.ToJson());

          //
          // Calculate solution
          //
          var list = (IEnumerable<Int32>)null;
          var res = GameSolver.Solve(data.ChildrenCount, data.EliminateEach, out list);

          //
          // Report results
          //
          var result = new GameResult() {
            Id = data.Id,
            LastChild = res,
            OrderOfElimination = list
          };

          Console.WriteLine();
          Console.WriteLine("Submitting results:");
          Console.WriteLine("   {0}", result.ToJson());

          Task.Run(() => svc.PostResults(result)).Wait();
        }

      } catch (Exception ex) {
        Console.WriteLine();
        Console.WriteLine("[ERROR]: {0}", ex.Message);
        while((ex = ex.InnerException) != null) 
          Console.WriteLine("   {0}", ex.Message);
      }

      Console.WriteLine();
    }

    #endregion Main(..)

  }

  #endregion [Program class definition]
}
