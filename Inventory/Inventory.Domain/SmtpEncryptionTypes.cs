﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain
{
    public enum SmtpEncryptionTypes
    {
        Normal,
        TLS,
        SSL
    }
}