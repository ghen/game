using System;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;

using Game.Utils;
using Game.Services.Data;

namespace Game.Services {

  public class GameServiceClient : Disposable, IDisposable {

    #region Private members

    private readonly HttpClient _client;

    #endregion Private members

    #region Constructor and Initialization

    /// <summary>
    /// Constructs new <see cref="GameServiceClient"/> instance.
    /// </summary>
    /// <exception cref="ArgumentException">
    /// Throws if <paramref name="apiUrl"/> is <value>null</value> or empty.
    /// </exception>
    /// <param name="apiUrl">API endpoint base URL.</param>
    public GameServiceClient(String apiUrl) 
      : this (apiUrl, null, true) { }

    /// <summary>
    /// Constructs new <see cref="GameServiceClient"/> instance.
    /// </summary>
    /// <exception cref="ArgumentException">
    /// Throws if <paramref name="apiUrl"/> is <value>null</value> or empty.
    /// </exception>
    /// <param name="apiUrl">API endpoint base URL.</param>
    /// <param name="httpHandler">The HTTP handler stack to use for sending requests.</param>
    /// <param name="disposeHandler"><value>true</value> if the inner handler should be disposed of by the Dispose method,
    /// <value>false</value> if you intend to reuse the inner handler.</param>
    internal GameServiceClient(String apiUrl, HttpMessageHandler httpHandler, Boolean disposeHandler) {
      if (String.IsNullOrEmpty(apiUrl)) throw new ArgumentException("apiUrl");

      this._client = ((httpHandler == null) ? new HttpClient() : new HttpClient(httpHandler, disposeHandler));
      this._client.BaseAddress = new Uri(apiUrl);

      // NOTE:
      //   Some servers do not handle properly the Expect header which might cause timed out errors,
      //   so disabling this will avoid those issues. In fact this can increase the performance 
      //   by reducing a round trip.
      this._client.DefaultRequestHeaders.ExpectContinue = false;

      this._client.DefaultRequestHeaders.Accept.Clear();
      this._client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    #endregion Constructor and Initialization

    #region IDisposable support

    /// <summary>
    /// Releases object resources.
    /// Only unmanaged objects should be released if <paramref name="disposable"/> is 'false'.
    /// </summary>
    /// <remarks>
    /// This method should be overridden in nested classes.
    /// Use the implementation bellow as a template.
    /// </remarks>
    /// <param name="disposing">Indicates if this method is called by user ('true') or by GC during finalization ('false').</param>
    override protected void Dispose(Boolean disposing) {

      // Dispose all managed resources only if disposing
      // Managed resources are unavailable during finalization stage!

      if (disposing) {

        // Dispose managed resources

        this._client.Dispose();
      }

      // Clean up unmanaged resources                
      // CloseHandle(handle);
      // handle = IntPtr.Zero;
    }

    #endregion IDisposable support

    #region GetData

    /// <summary>
    /// Loads game input parameters.
    /// </summary>
    /// <exception cref="HttpException">Throws if HTTP request failed.</exception>
    /// <exception cref="SerializationException">Throws if input dta can't be parsed.</exception>
    /// <returns>Initialized <seealso cref="GameData"/> class instance.</returns>
    public async Task<GameData> GetData() {

      var uri = new UriBuilder(String.Format("{0}/game", this._client.BaseAddress));
      
      var response = await this._client.GetAsync(uri.Uri, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false);
      if (!response.IsSuccessStatusCode)
        throw new HttpException((Int32)response.StatusCode, response.ReasonPhrase);

      var data = await response.Content.ReadAsStreamAsync();
      var res = (GameData)data.ParseJson(typeof(GameData));

      return res;
    }

    #endregion GetData

    #region PostResults

    /// <summary>
    /// Reports game results.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// Throws if <paramref name="data"/> is <value>null</value>.
    /// </exception>
    /// <exception cref="HttpException">Throws if HTTP request failed.</exception>
    /// <param name="data">Game results data.</param>
    public async Task PostResults(GameResult data) {
      if (data == null) throw new ArgumentNullException("data");

      var uri = new UriBuilder(String.Format("{0}/game/{1}", this._client.BaseAddress, data.Id));
      var content = new StringContent(data.ToJson(), System.Text.Encoding.UTF8, "application/json");
      var response = await this._client.PostAsync(uri.Uri, content).ConfigureAwait(false);
      if (!response.IsSuccessStatusCode)
        throw new HttpException((Int32)response.StatusCode, response.ReasonPhrase);
    }

    #endregion PostResults
  }

}
