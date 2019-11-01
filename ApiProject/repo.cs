using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Globalization;

namespace ApiProject
{
    [DataContract(Name = "repo")]
    public class Repository
    {
        //[DataMember(Name = "description")]
       // public string Description { get; set; }

        [DataMember(Name = "access_token")]
        public static string Access_token { get; set; }

        [DataMember(Name = "token_expiry")]
        public string Token_expiry { get; set; }

        
    }
}
