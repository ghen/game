using System;
using System.Web;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Game.Utils;
using Game.Services.Data;

namespace Game.Services.Test {

  #region [GameServiceClientTests class definition]

  /// <summary>
  /// Test scenarios for <seealso cref="GameServiceClient"/> class.
  /// </summary>
  [TestClass]
  public class GameServiceClientTests {

    #region Constants

    /// <summary>
    /// Just to make sure that our tests would never hit any remote servers.
    /// </summary>
    private static readonly String ServiceApiUrl = "http://localhost";

    #endregion Constants

    #region Constructor

    /// <summary>
    /// Invalid argument should throw an exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GameServiceClient_Ctor_ArgumentException_On_ApiUrl_Is_Zero() {
      var url = (String)null;
      using (var svc = new GameServiceClient(url)) { }
    }

    /// <summary>
    /// Invalid argument should throw an exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GameServiceClient_Ctor_ArgumentException_On_ApiUrl_Is_Empty() {
      var url = String.Empty;
      using (var svc = new GameServiceClient(url)) { }
    }

    #endregion Constructor

    #region GetData

    /// <summary>
    /// Should throw a proper exception on HTPP 5xx remote server error.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(HttpException))]
    public async Task GameServiceClient_GetData_HttpException_On_Server_Error() {
      
      var handler = new GameServiceHttpHandlerMock(HttpStatusCode.ServiceUnavailable);

      var url = GameServiceClientTests.ServiceApiUrl;
      using (var svc = new GameServiceClient(url, handler, true)) {
        await svc.GetData();
      }
    }

    /// <summary>
    /// Should throw a proper exception on invalid input data.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(SerializationException))]
    public async Task GameServiceClient_GetData_SerializationException_On_Invalid_Data_Stream() {

      var handler = new GameServiceHttpHandlerMock(HttpStatusCode.OK, "{ \"id\":1, \"last_child\": ");

      var url = GameServiceClientTests.ServiceApiUrl;
      using (var svc = new GameServiceClient(url, handler, true)) {
        await svc.GetData();
      }
    }

    /// <summary>
    /// Verifies that server response is parsed properly.
    /// </summary>
    [TestMethod]
    public async Task GameServiceClient_GetData_Parses_Response_Properly() {

      var data = new GameData() {
        Id = 1,
        ChildrenCount = 3,
        EliminateEach = 1
      };
      var handler = new GameServiceHttpHandlerMock(HttpStatusCode.OK, data.ToJson());

      var res = (GameData)null;
      var url = GameServiceClientTests.ServiceApiUrl;
      using (var svc = new GameServiceClient(url, handler, true)) {
        res = await svc.GetData();
      }

      Assert.IsNotNull(res);
      Assert.AreEqual(data.Id, res.Id);
      Assert.AreEqual(data.ChildrenCount, res.ChildrenCount);
      Assert.AreEqual(data.EliminateEach, res.EliminateEach);
    }

    #endregion GetData

    #region PostResults

    /// <summary>
    /// Should throw a proper exception on HTPP 5xx remote server error.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(HttpException))]
    public async Task GameServiceClient_PostResults_HttpException_On_Server_Error() {

      var data = new GameResult() {
        Id = 1,
        LastChild = 1,
        OrderOfElimination = new Int32[0]
      };

      var handler = new GameServiceHttpHandlerMock(HttpStatusCode.ServiceUnavailable);

      var url = GameServiceClientTests.ServiceApiUrl;
      using (var svc = new GameServiceClient(url, handler, true)) {
        await svc.PostResults(data);
      }
    }

    /// <summary>
    /// Should throw a proper exception on invalid input data.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public async Task GameServiceClient_PostResults_ArgumentNullException_On_Invalid_Data() {

      var handler = new GameServiceHttpHandlerMock(HttpStatusCode.OK);

      var url = GameServiceClientTests.ServiceApiUrl;
      using (var svc = new GameServiceClient(url, handler, true)) {
        await svc.PostResults(null);
      }
    }

    /// <summary>
    /// Verifies that server request is submitted successfully.
    /// </summary>
    [TestMethod]
    public async Task GameServiceClient_PostResults_Parses_Response_Properly() {
      
      var data = new GameResult() {
        Id = 1,
        LastChild = 1,
        OrderOfElimination = new Int32[0]
      };

      var handler = new GameServiceHttpHandlerMock(HttpStatusCode.OK, null);
      handler.Callback = (req, resp) => {
        var stream = Task.Run(() => req.Content.ReadAsStreamAsync()).Result;
        var res = (GameResult)stream.ParseJson(typeof(GameResult));
        
        Assert.IsNotNull(res);
        Assert.AreEqual(data.Id, res.Id);
        Assert.AreEqual(data.LastChild, res.LastChild);
        Assert.IsNotNull(res.OrderOfElimination);
        Assert.IsTrue(data.OrderOfElimination.SequenceEqual(res.OrderOfElimination), "OrderOfElimination value does not match.");
      };

      var url = GameServiceClientTests.ServiceApiUrl;
      using (var svc = new GameServiceClient(url, handler, true)) {        
        await svc.PostResults(data);
      }
    }

    #endregion PostResults
  }

  #endregion [GameServiceClientTests class definition]
}
