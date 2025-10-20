using System;
using System.Collections.Generic;
using System.Text;

namespace ChatCommon
{
    public class MessageModel
    {
        public string From { get; set; } = null!;
        public string To { get; set; } = null!;
        public string? Body { get; set; }
    }
}
