using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using FortniteDownloader;
using Newtonsoft.Json;

namespace FnDownloader
{
    public class Downloader
    {
        private IDownloader FileDownloader;
        public string GetOAuthToken()
        {
            var oauthClient = new RestClient("https://account-public-service-prod03.ol.epicgames.com/account/api/oauth/token");
            var oauthRes = oauthClient.Execute(
                new RestRequest(Method.POST)
               .AddHeader("Content-Type", "application/x-www-form-urlencoded")
               .AddHeader("Authorization", "basic MzQ0NmNkNzI2OTRjNGE0NDg1ZDgxYjc3YWRiYjIxNDE6OTIwOWQ0YTVlMjVhNDU3ZmI5YjA3NDg5ZDMxM2I0MWE=")
               .AddParameter("grant_type", "client_credentials")
               .AddParameter("token_type", "eg1"));

            return JsonConvert.DeserializeObject<dynamic>(oauthRes.Content)["access_token"];
        }

        public string GetLatestManifest()
        {
            var manifestClient = new RestClient("https://launcher-public-service-prod06.ol.epicgames.com/launcher/api/public/assets/Windows/4fe75bbc5a674f4f9b356b5c90567da5/Fortnite?label=Live");
            var manifestRes = manifestClient.Execute(
                new RestRequest(Method.GET)
                .AddHeader("Authorization", "bearer " + GetOAuthToken()));

            var manifest = JsonConvert.DeserializeObject<dynamic>(manifestRes.Content);

            return manifest;
        }


        public void CheckManifest(string Manifest)
        {
            FileDownloader = new AuthedDownloader();
            Dictionary<string, FileChunkPart[]>.KeyCollection keys;
            Console.WriteLine("Checking Manifest");
            Console.WriteLine(Manifest);
        }


    }

    class FnDowloader : Downloader
    {
        static void Main()
        {
            Downloader downloader = new Downloader();
            Console.Write("Do you want to insert a Manifest or check for the latest? \n");
            Console.Write("M or C? \n");
            string checker = Console.ReadLine();
            if (checker == "m")
            {
                Console.WriteLine("Insert Manifest \n");
                string Manifest = Console.ReadLine();
                downloader.CheckManifest(Manifest);
               
            } else if (checker == "c")
            {
                Console.Write("Loading Manifest");
                Console.Write(downloader.GetLatestManifest());
            }
        }
    }

}
