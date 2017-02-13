using System;
using System.Linq;
using System.Collections.Generic;

namespace Game.Core {

  #region [GameSolver class definition]

  /// <summary>
  /// Calculates solution for the game.
  /// </summary>
  internal static class GameSolver {

    #region Solve

    /// <summary>
    /// Calculates solution for "Children Game":
    /// <list type="bullet">
    /// <item>n children stand around a circle.</item>
    /// <item>Starting with a given child and working clockwise, each child gets a sequential number, which we will refer to as its id.</item>
    /// <item>Then starting with the first child, they count out from 1 until k. The k-th child is now out and leaves the circle. The count starts again with the child immediately next to the eliminated one.</item>
    /// <item>Children are so removed from the circle one by one. The winner is the last child left standing.</item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <exception cref="ArgumentException">
    /// Throws if any of the input parameters is invalid.
    /// </exception>
    /// <param name="n">Number of elements in a circle.</param>
    /// <param name="k">Elimination counter.</param>
    /// <param name="list">List of eliminated items.</param>
    /// <returns>Remaining element.</returns>
    public static Int32 Solve(Int32 n, Int32 k, out IEnumerable<Int32> list) {
      if (n <= 0) throw new ArgumentException("n");
      if (k <= 0) throw new ArgumentException("k");

      // NOTE:
      //   Selecting optimal (most effective) solution depends a lot on
      //   'n' and 'k' knowledge. Specifically, in real life, how many children
      //   would play this game at the same time? 10, 50, 100... Even if we move
      //   to the virtual space (mobile games), there are at most billion online
      //   users over the world. This would require about 4 GB to keep all
      //   the data in-memory. Switching to binary fields would reduce this
      //   requirement further. Effectively we are not limited with memory here.
      //
      //   Also knowing that 'k' << 'n' (is significantly less than) would let us
      //   favor one approach over another, as O(k*n) would be ~O(n) rather than O(n*n).

      // ASSUMPTIONS:
      //   'n' could be rather large number, but reasonable, to keep all the data in memory.
      //   'k' could be purely random (up to 'k' = 'n-1'). E.g. O(kn) considered as O(n*n).

      // NOTE:
      //   Minimal memory requirement for the solution would be O(Kn) = O(n), 
      //   as we have to store and report back (n-1) eliminated elements in any case.
      //   Here K is a constant representing each item storage overhead.
      var data = new LinkedList<Int32>(Enumerable.Range(1, n));

      // NOTE:
      //   Solution bellow will use O((k/2)*(n-1)) jumps through the list items.
      //   What, in worst case scenario, could lead to O(k*n) = O(n*n) complexity.
      //
      //   Here optimization is possible if we are to utilize more complicated
      //   data structures (such as trees) to optimize indexed navigation
      //   through the remaining items with ability to remove items at a constant time.
      //   Complexity would drop down to O(log(n)*(n-1)).

      // NOTE:
      //   Here, to reduce memory consumption, we would alter original linked list,
      //   rather than copy eliminated items into new list/array. 
      //
      //   So at each step we "move" eliminated item at the end of the list. Our 
      //   "intermediate" list at a step 'm' would look like:
      //
      //   <Item 1> -> .. -> <Item n-m> -> <Eliminated 1> -> .. -> <Eliminated m>
      //
      //   This would cause some constant, but very small, calculation overhead, of course.
      //   Where memory savings are significant (on the scale of GB) for huge values of 'n'.      

      var curr = data.First;
      var last = data.Last;
      var count = data.Count;
      while (last != data.First) {
        
        var skip = (k - 1) % count;
        while (skip-- > 0)
          curr = (curr.Next != last.Next) ? curr.Next : data.First;

        var next = curr.Next;
        if (curr == last) {
          last = last.Previous;
          next = data.First;
        }
        
        data.Remove(curr);
        data.AddLast(curr);
        count--;
        
        curr = next;
      }

      data.Remove(last);
      list = data;

      return last.Value;
    }

    #endregion Solve

  }

  #endregion [GameSolver class definition]

}
