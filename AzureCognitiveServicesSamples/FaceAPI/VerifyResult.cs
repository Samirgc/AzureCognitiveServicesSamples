﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureCognitiveServicesSamples.FaceAPI
{
    public class VerifyResult
    {
        public bool isIdentical { get; set; }
        public double confidence { get; set; }

    }
}
