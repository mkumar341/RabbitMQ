using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ
{
    public interface ISendMessage
    {
        void PublishMessage(object message);

    }
}
