using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;


namespace TestingUtilities
{
    public class TestCase
    {
        public void writeResults(string TestScript, string TestCase, string Results)
        {
            Models.QuickResult item = new Models.QuickResult();
            item.status = 1;
            item.testscript = TestScript;
            item.testcase = TestCase;
            item.results = Results;
            item.clientIdNumber = new Guid();


            var clieNt = new RestClient("http://localhost:65480/api/QuickResults");

            var request = new RestRequest(Method.POST);
            request.AddQueryParameter("loggedInas", "kconners");
            request.AddJsonBody(item);

            var response = clieNt.Execute(request);
            string L = response.Content;
        }
        public void writeResults(Guid Client_idnumber, string TestScript, string TestCase, string Results)
        {
            Models.QuickResult item = new Models.QuickResult();
            item.status = 1;
            item.testscript = TestScript;
            item.testcase = TestCase;
            item.results = Results;
            item.clientIdNumber = Client_idnumber;


            var clieNt = new RestClient("http://localhost:65480/api/QuickResults");

            var request = new RestRequest(Method.POST);
            request.AddQueryParameter("loggedInas", "kconners");
            request.AddJsonBody(item);

            var response = clieNt.Execute(request);
            string L = response.Content;
        }
    }
}
