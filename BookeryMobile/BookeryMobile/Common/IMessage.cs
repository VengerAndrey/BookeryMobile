using System;
using System.Collections.Generic;
using System.Text;

namespace BookeryMobile.Common
{
    public interface IMessage
    {
        void Long(string message);
        void Short(string message);
    }
}
