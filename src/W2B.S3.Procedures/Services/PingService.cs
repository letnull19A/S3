using W2B.S3.Procedures.Protos;

namespace W2B.S3.Procedures.Services
{
    public class PingService : Pinger.PingerBase
    {
        public override Task<PingReply> Ping(PingRequest request, Grpc.Core.ServerCallContext context)
        {
            return Task.FromResult(new PingReply
            {
                Message = "Pong"
            });
        }
    }
}
