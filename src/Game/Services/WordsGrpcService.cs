using System;
using Microsoft.Extensions.Configuration;
using Grpc.Net.Client;
using ProtoContracts;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Game.Services
{
    public class WordsGrpcService
    {
        private readonly IConfiguration _config;
        private WordsService.WordsServiceClient _client;

        public WordsGrpcService(IConfiguration config)
            => _config = config;
        public void Init()
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            var channel = GrpcChannel.ForAddress(_config["WordsSvc_Endpoint"]);
            _client = new WordsService.WordsServiceClient(channel);
        }

        public async Task<List<string>> GetNewWordsList()
        {
            var res = await _client.GetWordsPackageAsync(new GetWordsPackageReq());
            return res.Words.ToList();
        }
    }
}