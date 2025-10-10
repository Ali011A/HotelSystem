﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Common.Exceptions
{
    public class DomainValidationException:Exception
    {
        public DomainValidationException(string message):base(message) { }
    }
}
