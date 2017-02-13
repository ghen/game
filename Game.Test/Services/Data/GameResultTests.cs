using System;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Game.Utils;

namespace Game.Services.Data.Test {

  #region [GameResultTests class definition]

  /// <summary>
  /// Test scenarios for <seealso cref="GameResult"/> class.
  /// </summary>
  [TestClass]
  public class GameResultTests {

    #region Properties

    /// <summary>
    /// Verifies that object properties are updated properly.
    /// </summary>
    [TestMethod]
    public void GameResult_Set_Id() {
      var data = new GameResult();

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
    public void GameResult_Set_LastChild() {
      var data = new GameResult();

      data.LastChild = 1;
      Assert.AreEqual(1, data.LastChild);

      data.LastChild = Int32.MaxValue;
      Assert.AreEqual(Int32.MaxValue, data.LastChild);

      data.LastChild = 0;
      Assert.AreEqual(0, data.LastChild);
    }

    /// <summary>
    /// Verifies that object properties are updated properly.
    /// </summary>
    [TestMethod]
    public void GameResult_Set_OrderOfElimination() {
      var data = new GameResult();

      var list = new Int32[0];
      data.OrderOfElimination = list;
      Assert.IsTrue(list.SequenceEqual(data.OrderOfElimination));

      list = new Int32[] { 0, 1, 2 };
      data.OrderOfElimination = list;
      Assert.IsTrue(list.SequenceEqual(data.OrderOfElimination));
    }

    #endregion Properties

    #region JSON Serialization

    /// <summary>
    /// Verifies that object's serialization attributes are set properly.
    /// </summary>
    [TestMethod]
    public void GameResult_Serialize_To_JSON() {
      var data = new GameResult() {
        Id = 0,
        LastChild = 3,
        OrderOfElimination = new Int32[] { 1, 2 }
      };
      var expectedStr = "{\"id\":0,\"last_child\":3,\"order_of_elimination\":[1,2]}";
      
      var jsonStr = data.ToJson();

      Assert.AreEqual(expectedStr, jsonStr);
    }

    /// <summary>
    /// Verifies that object's serialization attributes are set properly.
    /// </summary>
    [TestMethod]
    public void GameResult_Serialize_From_JSON() {
      var data = new GameResult() {
        Id = 0,
        LastChild = 3,
        OrderOfElimination = new Int32[] { 1, 2 }
      };
      var jsonStr = "{\"id\":0,\"last_child\":3,\"order_of_elimination\":[1,2]}";

      var res = (GameResult)null;
      using (var stream = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(jsonStr))) {
        res = (GameResult)stream.ParseJson(typeof(GameResult));
      }

      Assert.IsNotNull(res);
      Assert.AreEqual(data.Id, res.Id);
      Assert.AreEqual(data.LastChild, res.LastChild);
      Assert.IsTrue(data.OrderOfElimination.SequenceEqual(res.OrderOfElimination), "OrderOfElimination does not match.");
    }

    #endregion JSON Serialization
  }

  #endregion [GameResultTests class definition]
}
