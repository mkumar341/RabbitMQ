﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ
{
    public interface IReceiveMessage
    {
        string ConsumeMessage();

    }
}
