using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Eba5Dal
{
    public class Operation
    {
        public void Approve(string user, int processId, int requestId, int eventId, string reasonText)
        {
            EbaClientWcf.eBAWSAPISoapClient client = new EbaClientWcf.eBAWSAPISoapClient();

            string wcfUserName =  ConfigurationManager.AppSettings["EBAWCFClientUsername"];
            string wcfPass =  ConfigurationManager.AppSettings["EBAWCFClientPass"];

            client.ContinueProcessWithValidation(wcfUserName, wcfPass, user, "turkish", processId, requestId, eventId, reasonText);
            
        }
    }
}
