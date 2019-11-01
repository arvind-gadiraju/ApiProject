using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ApiProject.Models
{
    public class Inputs

    {
        
        public  string Api_Key { get; set; }
        public string Domain_Name { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int No_Of_Chats { get; set; }
        public int NO_Of_Messages { get; set; }
        public string Input_Message { get; set; }
        public  Channel channel { get; set; }
        public  long? PSID { get; set; }
        public long? Page_Id { get; set; }
        public string Agent { get; set; }
        public string End_user { get; set; }
        public long? Asset_ID { get; set; }
        public long? Destination_Id { get; set; }
        public long? Asset_Live { get; set; }
        public long? Destination_Live{ get; set; }



    }
}