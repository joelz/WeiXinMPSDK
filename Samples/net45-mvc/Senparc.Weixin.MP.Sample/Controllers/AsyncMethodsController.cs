//DPBMARK_FILE MP
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Senparc.NeuChar.Entities;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.GroupMessage;
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


        /// <summary>
        /// 使用异步Action测试异步模板消息接口
        /// </summary>
        /// <param name="checkcode"></param>
        /// <returns></returns>
        public async Task<ActionResult> SendNews(
            string openIds,
            string title,
            string author,
            string url,
            string desc)
        {
            //string thrumbMediaId = "8_cg03XSzRM4AwScm91SrMOd4tzNnPvtu8JLqOJuR0d_fyu_KnkKKrByVQKJm2lZ";
            //NewsModel news = new NewsModel
            //{
            //    thumb_media_id = thrumbMediaId,
            //    author = author,
            //    title = title,
            //    content_source_url = url,
            //    digest = desc,
            //};
            //var result = await MediaApi.UploadTemporaryNewsAsync(appId, 10000, news);
            ////  测试号不能群发消息了！！！
            //var result2 = GroupMessageApi.SendGroupMessageByOpenId(appId, GroupMessageType.mpnews,
            //    result.media_id, null, 10000, openIds.Split(",".ToCharArray()));


            Article article = new Article()
            {
                Title = title,
                Description = desc,
                Url = url,
                PicUrl = "https://share.dc1979.com/run/123.jpg"
            };
            var arr = openIds.Split(",".ToCharArray());
            foreach (var openId in arr)
            {
                //只能用客服消息来发送图文消息
                CustomApi.SendMpNews(appId, openId, "tXa-SMXKKx1djW3CgvVu3lUW57w2hqxIFzOgmj0jUBSBq4V-oGltLmgd-ui6Gg7k");
                //只能发一篇article！！
                //CustomApi.SendNews(appId, openId, new List<Article> { article });
            }

            return Content("图文消息已经发送。");
        }

        /// <summary>
        /// 使用异步Action测试异步模板消息接口
        /// </summary>
        /// <param name="checkcode"></param>
        /// <returns></returns>
        public ContentResult UploadPic()
        {
            string file = @"D:\GitRepo\WeiXinMPSDK\Samples\net45-mvc\Senparc.Weixin.MP.Sample\Images\123.jpg";
            var result = MediaApi.UploadTemporaryMedia(appId, UploadMediaFileType.image, file);

            return Content(result.media_id + "（00）" + result.thumb_media_id);
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