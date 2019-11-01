using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.IO;
using System.Globalization;
using ApiProject.Models;
using ApiProject.ViewModels;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;


namespace ApiProject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValuesController : Controller
    {
        
        private readonly INames _names;
        private readonly IHttpClientFactory _clientFactory;


        public bool GetBranchesError { get; private set; }

        public ValuesController(IHttpClientFactory clientFactory, INames names)
        {
            _clientFactory = clientFactory;
            _names = names;
        }
        public  int TimeDuration()
        {
            Inputs ne = _names.GetInputs();
            TimeSpan k = ne.To - ne.From;
            double NrOfDays = k.TotalMilliseconds;
            int g = (int)NrOfDays;
            return g;
        }
        public string CurrentTime()
        {
            DateTime dateTime = DateTime.Now;
            string r = dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            return r;
        }



        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public string InOrOut(int j)
        {
            if(j%2==0)
            { return "INBOUND"; }
            else
            {
                return "OUTBOUND";
            }
        }

        public string Json()
        {
            Inputs Json_inputs = _names.GetInputs();
            if (Json_inputs.channel == Channel.SMS)
            {
                return "{'channel': 'SMS','asset_id':'" + Json_inputs.Asset_ID + "','destination_id':'" + Json_inputs.Destination_Id + "','messages':[{'type':'INBOUND','text':'" + Json_inputs.Input_Message + "','timestamp':'" + CurrentTime() + "'}]}";
            }
            else if(Json_inputs.channel == Channel.facebook)
            {
                return "{'channel': 'facebook','asset_id':'" + Json_inputs.Page_Id+ "','destination_id':'" + Json_inputs.PSID + "','messages':[{'type':'INBOUND','text':'" + Json_inputs.Input_Message + "','timestamp':'" + CurrentTime() + "'}]}";
            }
            else if (Json_inputs.channel == Channel.email)
            {
                return "{'channel': 'email','asset_id':'" + Json_inputs.Agent + "','destination_id':'" + Json_inputs.End_user + "','subject':'Hi','messages':[{'type':'INBOUND','text':'" + Json_inputs.Input_Message + "','timestamp':'" + CurrentTime() + "'}]}";
            }
            else 
            {
                return "{'channel': 'Livechat','asset_id':'" + Json_inputs.Agent + "','destination_id':'" + Json_inputs.End_user + "','subject':'Hi','messages':[{'type':'INBOUND','text':'" + Json_inputs.Input_Message + "','timestamp':'" + CurrentTime() + "'}]}";
            }
        }
        public async Task<string> GEtAuthTokenAsync()
        {
            Inputs AuthCode = _names.GetInputs();
            

            string Auth = Base64Encode(AuthCode.Api_Key+"@"+AuthCode.Domain_Name);

            var request = new HttpRequestMessage(HttpMethod.Get,
                             "https://api-sandbox.imi.chat/apiqa/authorize");
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("auth_code", Auth);


            var client = _clientFactory.CreateClient();




            var response = await client.SendAsync(request);
            string s = response.Headers.GetValues("access_token").FirstOrDefault();
            return s;
        }



       


        [HttpGet]
        public ViewResult CreateChat()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateChat([FromForm]Inputs inputs)
        {
            Inputs n = _names.Add(inputs);
            
           double o=1;
            while ( o> 0)
            {
                TimeSpan l = Convert.ToDateTime(n.From) - DateTime.Now;
                double pp = l.TotalSeconds;

                if (pp < 0)
                {
                    o = -1;
                }

            }



            string myJson;
           
                string auth_token = await GEtAuthTokenAsync();
                var client = _clientFactory.CreateClient();
                myJson = Json();
                client.DefaultRequestHeaders.Add("access_token", auth_token);
                client.DefaultRequestHeaders.Add("teamid", "868");

                var response = await client.PostAsync(
            "https://api-sandbox.imi.chat/apiqa/v3.0/chats",
             new StringContent(myJson, Encoding.UTF8, "application/json"));

            string t = await response.Content.ReadAsStringAsync();
               

                    int i = t.IndexOf("chat_id");
                    string x = t.Substring(i + 10, 16);



                    for (int w = 1; w <= n.NO_Of_Messages; w++)
                    {


                        string auth_token_ = await GEtAuthTokenAsync();
                        var client_ = _clientFactory.CreateClient();

                        string Json = "	[ {'type': '" + InOrOut(w) + "','text': '" + n.Input_Message + " 0" + w + "','timestamp':'" + CurrentTime() + "'}]";


                        client_.DefaultRequestHeaders.Add("access_token", auth_token_);
                        var response_ = await client_.PostAsync(
                   "https://api-sandbox.imi.chat/APIQA/v3.0/chats/" + x + "/messages",
                    new StringContent(Json, Encoding.UTF8, "application/json"));


                        Thread.Sleep(TimeDuration() / n.NO_Of_Messages);


                    }
                    
                

               
               
                return View() ;
            }
                


           

      

        



    }
}