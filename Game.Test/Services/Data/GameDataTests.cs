using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Game.Utils;
using System.Text;

namespace Game.Services.Data.Test {

  #region [GameDataTests class definition]

  /// <summary>
  /// Test scenarios for <seealso cref="GameData"/> class.
  /// </summary>
  [TestClass]
  public class GameDataTests {

    #region Properties

    /// <summary>
    /// Verifies that object properties are updated properly.
    /// </summary>
    [TestMethod]
    public void GameData_Set_Id() {
      var data = new GameData();

      data.Id = 1;
      Assert.AreEqual(1, data.Id);

      data.Id = Int32.MaxValue;
      Assert.AreEqual(Int32.MaxValue, data.Id);

      data.Id = 0;
      Assert.AreEqual(0, data.Id);
    }

    /// <summary>
    /// Verifies that object properties are updated properly.
    /// </summary>
    [TestMethod]
    public void GameData_Set_ChildrenCount() {
      var data = new GameData();

      data.ChildrenCount = 1;
      Assert.AreEqual(1, data.ChildrenCount);

      data.ChildrenCount = Int32.MaxValue;
      Assert.AreEqual(Int32.MaxValue, data.ChildrenCount);

      data.ChildrenCount = 0;
      Assert.AreEqual(0, data.ChildrenCount);
    }

    /// <summary>
    /// Verifies that object properties are updated properly.
    /// </summary>
    [TestMethod]
    public void GameData_Set_EliminateEach() {
      var data = new GameData();

      data.EliminateEach = 1;
      Assert.AreEqual(1, data.EliminateEach);

      data.EliminateEach = Int32.MaxValue;
      Assert.AreEqual(Int32.MaxValue, data.EliminateEach);

      data.EliminateEach = 0;
      Assert.AreEqual(0, data.EliminateEach);
    }

    #endregion Properties

    #region JSON Serialization

    /// <summary>
    /// Verifies that object's serialization attributes are set properly.
    /// </summary>
    [TestMethod]
    public void GameData_Serialize_To_JSON() {
      var data = new GameData() {
        Id = 0,
        ChildrenCount = 3,
        EliminateEach = 1
      };
      var expectedStr = "{\"id\":0,\"children_count\":3,\"eliminate_each\":1}";
      
      var jsonStr = data.ToJson();

      Assert.AreEqual(expectedStr, jsonStr);
    }

    /// <summary>
    /// Verifies that object's serialization attributes are set properly.
    /// </summary>
    [TestMethod]
    public void GameData_Serialize_From_JSON() {
      var data = new GameData() {
        Id = 0,
        ChildrenCount = 3,
        EliminateEach = 1
      };
      var jsonStr = "{\"id\":0,\"children_count\":3,\"eliminate_each\":1}";

      var res = (GameData)null;
      using (var stream = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(jsonStr))) {
        res = (GameData)stream.ParseJson(typeof(GameData));
      }

      Assert.IsNotNull(res);
      Assert.AreEqual(data.Id, res.Id);
      Assert.AreEqual(data.ChildrenCount, res.ChildrenCount);
      Assert.AreEqual(data.EliminateEach, res.EliminateEach);
    }

    #endregion JSON Serialization
  }

  #endregion [GameDataTests class definition]
}
