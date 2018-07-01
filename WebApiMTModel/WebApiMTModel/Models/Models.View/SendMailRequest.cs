using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiMTModel.Models.Models.View
{
    public class SendMailRequest
    {
            public string from { get; set; }
            public string recipient { get; set; }
            public string cc { get; set; }
            public string replyto { get; set; }
            public string subject { get; set; }
            public string body { get; set; }
            public string filecontent { get; set; }
            public string filename { get; set; }
           


    }
}