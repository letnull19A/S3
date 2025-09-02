using System.Text;

namespace W2B.S3.Core.Utils;

public sealed class Token {
  public string Generate(string payload)
  {
    if (string.IsNullOrEmpty(payload))
      throw new ArgumentException(nameof(payload));

    var originPayload = payload;
    var dateTimeNow = DateTime.Now;

    var formatedDate = BuildFormatedDate(dateTimeNow);

    string formatedData = string.Join("~", formatedDate);
    byte[] bytesOfData = GetBytes(formatedData); 
    string base64Data = ToBase64(bytesOfData);

    string formatedPayload = payload.ToLower();
    byte[] bytesOfPayload = GetBytes(formatedPayload);
    string base64Payload = ToBase64(bytesOfPayload);

    string resultToken = string.Join(".", [ base64Data, base64Payload ]);

    return resultToken;
  }

  private string[] BuildFormatedDate(DateTime date)
    => new string[] {
      date.Year.ToString(),
      date.Month.ToString(), 
      date.Day.ToString(), 
      date.Hour.ToString(), 
      date.Minute.ToString(), 
      date.Second.ToString()
    };

  private byte[] GetBytes(string payload) =>
    Encoding.ASCII.GetBytes(payload);

  private string ToBase64(byte[] payloadBytes) =>
    Convert.ToBase64String(payloadBytes);
}
