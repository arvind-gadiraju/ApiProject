﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProject.Models
{
    public interface INames
    {
       Inputs GetInputs();
        Inputs Add(Inputs inputs);


    }
}
