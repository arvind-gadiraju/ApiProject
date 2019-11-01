using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiProject.Models;

namespace ApiProject.Models
{
    public class Code : INames
    {
         static public Inputs _inputs;
       


            public Inputs Add(Inputs inputs)
        {
            _inputs = inputs;
            return _inputs;
           
        }

        public Inputs GetInputs()
        {
            return _inputs;
        }
    }
}
