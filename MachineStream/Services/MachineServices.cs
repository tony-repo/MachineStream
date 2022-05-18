using MachineStream.Model;
using MachineStream.Repository;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MachineStream.Services
{
    public interface IMachineServices
    {
        public Task<MachineInfo> GetMachineInfo();
    }

    public class MachineServices : IMachineServices
    {
        private readonly IConfiguration _configuration;
        private readonly IMachineRepository _machineRepository;
        private string _url = string.Empty;
        public MachineServices(IConfiguration configuration, IMachineRepository machineRepository)
        {
            _configuration = configuration;
            _machineRepository = machineRepository;
            _url = _configuration.GetValue<string>("WebSocketUrl");
        }

        public async Task<MachineInfo> GetMachineInfo()
        {
            using ClientWebSocket cln = new ClientWebSocket();
            try
            {
                WebSocketReceiveResult clientResult = null;
                do
                {
                    await cln.ConnectAsync(new Uri(_url), CancellationToken.None);
                    byte[] byteArr = new byte[1024];
                    var data = new ArraySegment<byte>(byteArr);
                    clientResult = await cln.ReceiveAsync(data, CancellationToken.None);
                    if (clientResult.MessageType == WebSocketMessageType.Text && !clientResult.CloseStatus.HasValue)
                    {
                        var recStr = Encoding.ASCII.GetString(data);
                        var obj = JsonConvert.DeserializeObject<JObject>(recStr);
                        var machineInfo = System.Text.Json.JsonSerializer.Deserialize<MachineInfo>(obj["payload"].ToString());
                        _machineRepository.SaveMachineInfo(machineInfo);
                        return machineInfo;
                    }

                } while (!clientResult.CloseStatus.HasValue);

                return new MachineInfo();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cln.State != WebSocketState.Closed)
                {
                    cln.Dispose();
                }
            }
        }
    }
}
