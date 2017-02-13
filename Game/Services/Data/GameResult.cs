using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Game.Services.Data {

  #region [GameResult class definition]

  /// <summary>
  /// Stores game result details.
  /// </summary>
  [DataContract]
  public class GameResult {

    #region Properties

    /// <summary>
    /// Game ID.
    /// </summary>
    [DataMember(Name = "id", Order = 1)]
    public Int32 Id { get; set; }

    /// <summary>
    /// Last child.
    /// </summary>
    [DataMember(Name = "last_child", Order = 2)]
    public Int32 LastChild { get; set; }

    /// <summary>
    /// Elimination sequence.
    /// </summary>
    [DataMember(Name = "order_of_elimination", Order = 3)]
    public IEnumerable<Int32> OrderOfElimination { get; set; }

    #endregion Properties

  }

  #endregion [GameResult class definition]

}
