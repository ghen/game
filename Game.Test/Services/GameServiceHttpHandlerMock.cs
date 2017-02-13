using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Game.Services.Test {

  #region [GameServiceHttpHandlerMock class definition]

  /// <summary>
  /// Mock class to be used with <seealso cref="HttpClient"/>.
  /// </summary>
  internal sealed class GameServiceHttpHandlerMock : HttpMessageHandler {

    #region Constructor and Initialization

    /// <summary>
    /// Constructs new <see cref="GameServiceHttpHandlerMock"/> instance.
    /// </summary>
    /// <param name="status">Desired response status code.</param>
    /// <param name="content">Desired response content.</param>
    public GameServiceHttpHandlerMock(HttpStatusCode status, String content = null) {
      this.Response = new HttpResponseMessage(status);
      if (!String.IsNullOrEmpty(content))
        this.Response.Content = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
    }
    
    #endregion Constructor and Initialization

    #region Properties

    /// <summary>
    /// Response message to be reported to the caller.
    /// </summary>
    public HttpResponseMessage Response { get; private set; }

    /// <summary>
    /// Optional callback for custom request checks.
    /// </summary>
    public Action<HttpRequestMessage, HttpResponseMessage> Callback { get; set; }

    #endregion Properties

    #region SendAsync

    /// <summary>
    /// Executes request asynchronously.
    /// </summary>
    /// <param name="request">Request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Response.</returns>
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken) {
      return Task<HttpResponseMessage>.Factory.StartNew(() => { 
        var response = this.Response;
        if (this.Callback != null) this.Callback(request, response);
        return response; 
      }, cancellationToken);
    }

    #endregion SendAsync

  }

  #endregion [GameServiceHttpHandlerMock class definition]
}
