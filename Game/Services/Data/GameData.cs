using System;
using System.Runtime.Serialization;

namespace Game.Services.Data {

  #region [GameData class definition]

  /// <summary>
  /// Stores initial game conditions.
  /// </summary>
  [DataContract]
  public class GameData {

    #region Properties

    /// <summary>
    /// Game ID.
    /// </summary>
    [DataMember(Name = "id", Order = 1)]
    public Int32 Id { get; set; }

    /// <summary>
    /// Children count (n).
    /// </summary>
    [DataMember(Name = "children_count", Order = 2)]
    public Int32 ChildrenCount { get; set; }

    /// <summary>
    /// Eliminate every (k).
    /// </summary>
    [DataMember(Name = "eliminate_each", Order = 3)]
    public Int32 EliminateEach { get; set; }

    #endregion Properties

  }

  #endregion [GameData class definition]

}
