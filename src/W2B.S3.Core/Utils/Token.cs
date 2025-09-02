namespace W2B.S3.Core.Utils;

public sealed class Token {
  public string Generate(string payload)
  {
    var originPayload = payload;
    var dateTimeNow = DateTime.Now;

    string[] octets = {
      dateTimeNow.Year.ToString(),
      dateTimeNow.Month.ToString(), 
      dateTimeNow.Day.ToString(), 
      dateTimeNow.Hour.ToString(), 
      dateTimeNow.Minute.ToString(), 
      dateTimeNow.Second.ToString()
    };

    string formatedData = string.Join(".", octets);

    return formatedData;
  }
}
