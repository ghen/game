using System;
using System.Runtime.Serialization.Json;

// NOTE:
//   This is purely cosmetic class for this particular project only.
//   In real life Newtonsoft.JSON or similar library will be used instead.

namespace Game.Utils {

  #region [JsonSerrialization class definition]

  /// <summary>
  /// A few extension methods for nicer code.
  /// </summary>
  internal static class JsonSerrialization {

    #region ToJson

    /// <summary>
    /// Converts Data Contract Object into JSON string.
    /// </summary>
    /// <param name="data">Source object.</param>
    /// <returns>JSON representation of the object.</returns>
    public static String ToJson(this Object data) {
      if (data == null) return String.Empty;

      var jsonStr = (String)null;
      using (var buf = new System.IO.MemoryStream()) {
        var ser = new DataContractJsonSerializer(data.GetType());
        ser.WriteObject(buf, data);

        buf.Seek(0, System.IO.SeekOrigin.Begin);
        using (var rdr = new System.IO.StreamReader(buf)) {
          jsonStr = rdr.ReadToEnd();
        }
      }

      return jsonStr;
    }

    #endregion ToJson

    #region ParseJson

    /// <summary>
    /// Parses Data Contract Object from source stream.
    /// </summary>
    /// <param name="stream">Source stream.</param>
    /// <param name="type">Object type to parse.</param>
    /// <returns>Parsed object instance, or <value>null</value> if stream is closed or empty.</returns>
    public static Object ParseJson(this System.IO.Stream stream, Type type) {
      if (stream == null || ! stream.CanRead) return null;
      
      var ser = new DataContractJsonSerializer(type);
      var res = ser.ReadObject(stream);

      return res;
    }

    #endregion ParseJson
  }

  #endregion [JsonSerrialization class definition]

}
