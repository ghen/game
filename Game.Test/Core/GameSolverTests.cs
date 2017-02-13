using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Game.Core;

namespace Game.Core.Test {

  #region [GameSolverTests class definition]

  /// <summary>
  /// Test scenarios for <seealso cref="GameSolver"/> class.
  /// </summary>
  [TestClass]
  public class GameSolverTests {

    #region Solve

    /// <summary>
    /// Invalid argument should throw an exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GameSolver_Solve_ArgumentException_On_N_Is_Zero() {
      var list = (IEnumerable<Int32>)null;
      GameSolver.Solve(0, 1, out list);
    }

    /// <summary>
    /// Invalid argument should throw an exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GameSolver_Solve_ArgumentException_On_N_Is_Negative() {
      var list = (IEnumerable<Int32>)null;
      GameSolver.Solve(-1, 1, out list);
    }

    /// <summary>
    /// Invalid argument should throw an exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GameSolver_Solve_ArgumentException_On_K_Is_Zero() {
      var list = (IEnumerable<Int32>)null;
      GameSolver.Solve(1, 0, out list);
    }

    /// <summary>
    /// Invalid argument should throw an exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GameSolver_Solve_ArgumentException_On_K_Is_Negative() {
      var list = (IEnumerable<Int32>)null;
      GameSolver.Solve(1, -1, out list);
    }

    /// <summary>
    /// Proper game results should be returned for various input parameters.
    /// </summary>
    [TestMethod]
    public void GameSolver_Solve_Calculates_Solution_Properly() {
      
      // NOTE:
      //   We could/should use DataRow in VS 2015/2016 instead.
      //   Implementation bellow would stop on the first invalid output.

      var data = new[] {
        new { n =  1,  k =  1,  res =  1,  list = new Int32[] {} },
        new { n =  1,  k =  2,  res =  1,  list = new Int32[] {} },
        new { n =  1,  k = 10,  res =  1,  list = new Int32[] {} },

        new { n =  2,  k =  1,  res =  2,  list = new Int32[] { 1 } },
        new { n =  2,  k =  2,  res =  1,  list = new Int32[] { 2 } },
        new { n =  2,  k = 10,  res =  1,  list = new Int32[] { 2 } },

        new { n =  5,  k =  1,  res =  5,  list = new Int32[] { 1,  2,  3,  4 } },
        new { n =  5,  k =  4,  res =  1,  list = new Int32[] { 4,  3,  5,  2 } },
        new { n =  5,  k =  5,  res =  2,  list = new Int32[] { 5,  1,  3,  4 } },
        new { n =  5,  k =  6,  res =  4,  list = new Int32[] { 1,  3,  2,  5 } },
        new { n =  5,  k = 10,  res =  4,  list = new Int32[] { 5,  2,  3,  1 } },

        new { n = 10,  k =  1,  res = 10,  list = new Int32[] { 1,  2,  3,  4,  5,  6,  7,  8,  9 } },
        new { n = 10,  k =  2,  res =  5,  list = new Int32[] { 2,  4,  6,  8, 10,  3,  7,  1,  9 } },
        new { n = 10,  k =  9,  res =  7,  list = new Int32[] { 9,  8, 10,  2,  5,  3,  4,  1,  6 } },
        new { n = 10,  k = 10,  res =  8,  list = new Int32[] { 10, 1,  3,  6,  2,  9,  5,  7,  4 } }
      };

      foreach(var test in data) {
        var list = (IEnumerable<Int32>)null;
        var res = GameSolver.Solve(test.n, test.k, out list);

        Assert.AreEqual(test.res, res, "Method result does not match.");

        Assert.IsNotNull(list);
        Assert.IsTrue(list.SequenceEqual(test.list),
          "Elimination sequence does not match expectation.\nExpect: [ {0} ].\nActual: [ {1} ].",
          String.Join<String>(", ", test.list.Select(i => String.Format("{0,2}", i))),
          String.Join<String>(", ", list.Select(i => String.Format("{0,2}", i))));
      }
    }

    #endregion Solve
  }

  #endregion [GameSolverTests class definition]
}
