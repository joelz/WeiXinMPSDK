//DPBMARK_FILE MP
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.MP.MvcExtension;
using Senparc.Weixin.MP.Sample.CommonService.CustomMessageHandler;

namespace Senparc.Weixin.MP.Sample.Controllers
{
    public class AsyncMethodsController : AsyncController
    {
        private string appId = Config.SenparcWeixinSetting.WeixinAppId;
        private string appSecret = Config.SenparcWeixinSetting.WeixinAppSecret;

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 使用异步Action测试异步临时二维码接口
        /// </summary>
        /// <returns></returns>
        public async Task<RedirectResult> QrCodeTest()
        {
            var ticks = SystemTime.Now.Ticks.ToString();
            var sceneId = int.Parse(ticks.Substring(ticks.Length - 7, 7));

            var qrResult = await QrCodeApi.CreateAsync(appId, 100, sceneId, QrCode_ActionName.QR_SCENE, "QrTest");
            var qrCodeUrl = QrCodeApi.GetShowQrCodeUrl(qrResult.ticket);

            return Redirect(qrCodeUrl);
        }

        /// <summary>
        /// 使用异步Action测试异步模板消息接口
        /// </summary>
        /// <param name="checkcode"></param>
        /// <returns></returns>
        public async Task<ActionResult> TemplateMessageTest(
            string openIds,
            string templateId,
            string url,
            string first,
            string keyword1,
            string keyword2,
            string keyword3,
            string remark)
        {
            var testData = new
            {
                first = new TemplateDataItem(first),
                keyword1 = new TemplateDataItem(keyword1),
                keyword2 = new TemplateDataItem(keyword2),
                keyword3 = new TemplateDataItem(keyword3),
                remark = new TemplateDataItem(remark)
            };
            var arr = openIds.Split(",".ToCharArray());
            foreach (var openId in arr)
            {
                await TemplateApi.SendTemplateMessageAsync(appId, openId, templateId, url, testData);
            }

            return Content("异步模板消息已经发送。");
        }

        #region 异步死锁测试

        /// <summary>
        /// 此方法会引发死锁，需要重启服务
        /// </summary>
        /// <returns></returns>
        public ActionResult DeadLockTest()
        {
            var result =
                Senparc.CO2NET.HttpUtility.RequestUtility.HttpGetAsync(null, "https://sdk.weixin.senparc.com",
                    cookieContainer: null).Result;
            return Content(result);
        }

        /// <summary>
        /// 此方法可以避免死锁
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> NoDeadLockTest()
        {
            var result = await Senparc.CO2NET.HttpUtility.RequestUtility.HttpGetAsync(null,"https://sdk.weixin.senparc.com",
                cookieContainer: null);
            return Content(result);
        }


        private async Task<string> GetRemoteData()
        {
            string result = null;
            await Task.Run(() =>
               {
                   Task.Delay(1000);
                   result = "hi " + SystemTime.Now.ToString();
               });
            return result;
        }

        /// <summary>
        /// 此方法会引发死锁，需要重启服务
        /// </summary>
        /// <returns></returns>
        public ActionResult DeadLockTest2()
        {
            var result = GetRemoteData().Result;
            return Content(result);
        }


        /// <summary>
        /// 此方法可以避免死锁
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> NoDeadLockTest2()
        {
            var result = await GetRemoteData();
            return Content(result);
        }



        /// <summary>
        /// 此方法加上.ConfigureAwait(false)可以避免死锁
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetRemoteData2()
        {
            string result = null;
            await Task.Run(() =>
            {
                Task.Delay(1000);
                result = "hi " + SystemTime.Now.ToString();
            }).ConfigureAwait(false);
            return result;
        }


        /// <summary>
        /// 此方法可以避免死锁
        /// </summary>
        /// <returns></returns>
        public ActionResult NoDeadLockTest3()
        {
            var result = GetRemoteData2().Result;
            return Content(result);
        }


        #endregion
    }
}