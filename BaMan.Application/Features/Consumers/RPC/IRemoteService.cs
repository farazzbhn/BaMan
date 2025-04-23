namespace BaMan.Application.Features.Consumers.RPC
{

    public interface IRemoteService
    {
        public string F1(string p1);
    }

    public class RemoteService : IRemoteService
    {
        public string F1(string p1)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Invoked F1 with parameter {p1}");
            Console.ResetColor();

            return Guid.NewGuid().ToString();
        }
    }

}
