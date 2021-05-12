﻿using RestSharp;
using System.Web.Mvc;

namespace Senparc.Weixin.MP.Sample.Controllers
{
    /// <summary>
    /// 调用别的API，返回结果。就是一个跳板。
    /// </summary>
    public partial class ApiProxyController : Controller
    {

        /// <summary>
        /// 从 http://api.fixer.io/latest?base=USD 返回汇率列表
        /// 
        /// 20191218：
        /// -->You will be required to create an account at https://fixer.io and obtain an API access key. 
        /// free plan每个月只能调用1000次，而且只能base eur。。。
        /// 所以现在先用假数据吧。。。
        /// 
        /// </summary>
        /// <returns>
        /// 返回数据示例:{	"base": "USD","date":"2017-06-06","rates": {"AUD":1.3359,"BGN":1.7373, ....	} }
        /// </returns>
        /// <remarks>
        /// 需要先 Install-Package RestSharp
        /// <package id = "RestSharp" version="105.1.0" targetFramework="net45" />
        /// </remarks>
        [HttpGet]
        public ActionResult ExchangeList()
        {
            //var client = new RestClient("http://api.fixer.io");
            //var request = new RestRequest("latest", Method.GET);
            //request.AddQueryParameter("base", "USD");

            //IRestResponse response = client.Execute(request);

            var content = @"{
  ""success"":true,
  ""timestamp"":1576656545,
  ""base"":""EUR"",
  ""date"":""2019-12-18"",
  ""rates"":{
    ""AED"":4.088564,
    ""AFN"":87.716438,
    ""ALL"":122.056814,
    ""AMD"":532.727649,
    ""ANG"":1.889131,
    ""AOA"":510.994674,
    ""ARS"":66.494363,
    ""AUD"":1.626464,
    ""AWG"":2.003674,
    ""AZN"":1.893734,
    ""BAM"":1.953035,
    ""BBD"":2.25028,
    ""BDT"":94.614608,
    ""BGN"":1.954673,
    ""BHD"":0.419705,
    ""BIF"":2093.302041,
    ""BMD"":1.113152,
    ""BND"":1.510166,
    ""BOB"":7.706671,
    ""BRL"":4.532641,
    ""BSD"":1.114525,
    ""BTC"":0.000168,
    ""BTN"":79.083921,
    ""BWP"":11.94562,
    ""BYN"":2.343625,
    ""BYR"":21817.780381,
    ""BZD"":2.246274,
    ""CAD"":1.466127,
    ""CDF"":1871.208689,
    ""CHF"":1.092514,
    ""CLF"":0.030648,
    ""CLP"":845.658984,
    ""CNY"":7.792959,
    ""COP"":3720.376816,
    ""CRC"":631.450567,
    ""CUC"":1.113152,
    ""CUP"":29.49853,
    ""CVE"":110.107436,
    ""CZK"":25.439312,
    ""DJF"":197.829005,
    ""DKK"":7.472189,
    ""DOP"":58.957954,
    ""DZD"":133.00164,
    ""EGP"":17.861187,
    ""ERN"":16.697709,
    ""ETB"":35.210004,
    ""EUR"":1,
    ""FJD"":2.416821,
    ""FKP"":0.904859,
    ""GBP"":0.850008,
    ""GEL"":3.227559,
    ""GGP"":0.850096,
    ""GHS"":6.363693,
    ""GIP"":0.904859,
    ""GMD"":57.243815,
    ""GNF"":10629.128333,
    ""GTQ"":8.576027,
    ""GYD"":232.477321,
    ""HKD"":8.670286,
    ""HNL"":27.550217,
    ""HRK"":7.446281,
    ""HTG"":107.27541,
    ""HUF"":330.48039,
    ""IDR"":15579.898866,
    ""ILS"":3.890633,
    ""IMP"":0.850096,
    ""INR"":79.120061,
    ""IQD"":1330.485984,
    ""IRR"":46869.267366,
    ""ISK"":136.995721,
    ""JEP"":0.850096,
    ""JMD"":149.010442,
    ""JOD"":0.789891,
    ""JPY"":121.819463,
    ""KES"":112.87274,
    ""KGS"":77.741646,
    ""KHR"":4545.576807,
    ""KMF"":491.317462,
    ""KPW"":1001.871501,
    ""KRW"":1300.284361,
    ""KWD"":0.337864,
    ""KYD"":0.928671,
    ""KZT"":428.776411,
    ""LAK"":9884.454155,
    ""LBP"":1684.929855,
    ""LKR"":201.917747,
    ""LRD"":208.719652,
    ""LSL"":16.027883,
    ""LTL"":3.286849,
    ""LVL"":0.673335,
    ""LYD"":1.557871,
    ""MAD"":10.703583,
    ""MDL"":19.202621,
    ""MGA"":4021.757056,
    ""MKD"":61.573352,
    ""MMK"":1677.337938,
    ""MNT"":3047.791322,
    ""MOP"":8.941504,
    ""MRO"":397.3951,
    ""MUR"":40.680805,
    ""MVR"":17.209082,
    ""MWK"":821.009862,
    ""MXN"":21.090341,
    ""MYR"":4.606269,
    ""MZN"":70.551333,
    ""NAD"":16.032026,
    ""NGN"":403.503011,
    ""NIO"":37.598041,
    ""NOK"":10.058643,
    ""NPR"":126.536759,
    ""NZD"":1.6969,
    ""OMR"":0.428548,
    ""PAB"":1.114405,
    ""PEN"":3.736852,
    ""PGK"":3.787695,
    ""PHP"":56.39231,
    ""PKR"":172.643151,
    ""PLN"":4.260979,
    ""PYG"":7193.805522,
    ""QAR"":4.052709,
    ""RON"":4.775979,
    ""RSD"":117.470568,
    ""RUB"":69.705693,
    ""RWF"":1041.877713,
    ""SAR"":4.175209,
    ""SBD"":9.22817,
    ""SCR"":15.249686,
    ""SDG"":50.220913,
    ""SEK"":10.484612,
    ""SGD"":1.509215,
    ""SHP"":1.470366,
    ""SLL"":10825.403557,
    ""SOS"":646.741135,
    ""SRD"":8.301871,
    ""STD"":24000.437809,
    ""SVC"":9.751046,
    ""SYP"":573.273704,
    ""SZL"":16.029449,
    ""THB"":33.684332,
    ""TJS"":10.78936,
    ""TMT"":3.907164,
    ""TND"":3.157461,
    ""TOP"":2.561976,
    ""TRY"":6.578428,
    ""TTD"":7.530323,
    ""TWD"":33.614413,
    ""TZS"":2561.135125,
    ""UAH"":26.151147,
    ""UGX"":4090.146899,
    ""USD"":1.113152,
    ""UYU"":42.111494,
    ""UZS"":10623.657184,
    ""VEF"":11.117604,
    ""VND"":25794.516117,
    ""VUV"":129.494338,
    ""WST"":2.96031,
    ""XAF"":655.031402,
    ""XAG"":0.065305,
    ""XAU"":0.000753,
    ""XCD"":3.00835,
    ""XDR"":0.806704,
    ""XOF"":655.031403,
    ""XPF"":119.089582,
    ""YER"":278.677815,
    ""ZAR"":16.081599,
    ""ZMK"":10019.708467,
    ""ZMW"":16.210302,
    ""ZWL"":358.434965
  }
        }";

            return Content(content, "application/json");
        }

        [HttpGet]
        public ActionResult BaiduMapRevGeo(string lat, string lng)
        {
            var client = new RestClient("http://api.map.baidu.com");
            var request = new RestRequest("reverse_geocoding/v3/", Method.GET);
            //ak=&output=json&coordtype=wgs84ll&location=31.225696563611,121.49884033194
            request.AddQueryParameter("ak", "aC5CqP6v6bjxVLcI3gyBE3gBkpxABbsd");
            request.AddQueryParameter("output", "json");
            request.AddQueryParameter("coordtype", "wgs84ll");
            request.AddQueryParameter("location", lat + "," + lng);

            IRestResponse response = client.Execute(request);

            return Content(response.Content, "application/json");
        }


    }
}