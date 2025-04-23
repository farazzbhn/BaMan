using System;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace BaMan.Shared.Types.Messages
{
    public class RPCMessage
    {
        public Guid Id { get; set; }
        public string MethodName { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
    }
}