using System;

namespace Game.Utils {

  #region [Disposable class definition]

  /// <summary>
  /// Implements standard routines for disposable pattern support.
  /// </summary>
  /// <remarks>
  /// See <seealso cref="IDisposable"/> interface description in MSDN for more information.
  /// </remarks>
  public abstract class Disposable : IDisposable {

    #region IDisposable support

    private Boolean _disposed = false;
    private Object _lock = new Object();

    /// <summary>
    /// We override destructor to control object finalization process.
    /// </summary>
    ~Disposable() {
      this.InternalDispose(false);
    }

    /// <summary>
    /// This method normally should be called to release the object.
    /// </summary>
    /// <remarks>
    /// This method is thread-safe.
    /// </remarks>
    void IDisposable.Dispose() {

      if (!this._disposed) {
        this.InternalDispose(true);
        this._disposed = true;
      }

      // Tell GC to skip finallization as we released the object already
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Internal disposal procedure that makes Dispose methos thread-safe.
    /// </summary>
    /// <param name="disposing">Indicates if this method is called by user ('true') or by GC during finalization ('false').</param>
    private void InternalDispose(Boolean disposing) {
      if (!this._disposed) {

        // We should not lock the thread during finalization process
        // GC will never call destructor from multiple threads
        if (disposing) {
          System.Threading.Monitor.Enter(this._lock);
          try {
            if (!this._disposed)
              this.Dispose(disposing);
          } finally {
            System.Threading.Monitor.Exit(this._lock);
          }
        } else {
          this.Dispose(disposing);
        }

        this._disposed = true;
      }

    }

    /// <summary>
    /// Releases object resources.
    /// Only unmanaged objects should be released if <paramref name="disposable"/> is 'false'.
    /// </summary>
    /// <remarks>
    /// This method should be overridden in nested classes.
    /// Use the implementation bellow as a template.
    /// </remarks>
    /// <param name="disposing">Indicates if this method is called by user ('true') or by GC during finalization ('false').</param>
    protected virtual void Dispose(Boolean disposing) {

      // Dispose all managed resources only if disposing
      // Managed resources are unavailable during finalization stage!

      if (disposing) {

        // Dispose managed resources

        // this.connection.Dispose();
        // this.otherClass.Dispose();

      }

      // Clean up unmanaged resources                
      // CloseHandle(handle);
      // handle = IntPtr.Zero;
    }

    #endregion IDisposable support

    #region IsDisposed

    /// <summary>
    /// Indicates if object is already disposed or not.
    /// For nested classes internal use only.
    /// </summary>
    protected Boolean IsDisposed {
      get {
        return this._disposed;
      }
    }

    #endregion IsDisposed
  }

  #endregion [Disposable class definition]

}