using OA.IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OA.WebApp.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        IUserInfoService UserInfoService { get; set; }
        public ActionResult Index()
        {

            #region 校验cookie是否有值及有效性
            if (CheckCookieInfo())
            {
                return Redirect("/Home/Index");
            }
            #endregion
            return View();
        }
        #region 显示验证码
        public ActionResult ShowValidateCode()
        {
            Common.ValidateCode validateCode = new Common.ValidateCode();
            string code = validateCode.CreateValidateCode(4);//产生验证码
            Session["validateCode"] = code;
            byte[] buffer=validateCode.CreateValidateGraphic(code);//将验证码画到画布上
            return File(buffer, "image/jpeg");
        }
        #endregion

        /// <summary>
        /// 校验cookie的值
        /// </summary>
        private bool CheckCookieInfo()
        {
            if (Request.Cookies["cp1"] != null && Request.Cookies["cp2"] != null)
            {
                string userName = HttpUtility.UrlDecode(Request.Cookies["cp1"].Value);
                string userPwd = Request.Cookies["cp2"].Value;
               var userInfo= UserInfoService.LoadEntities(u => u.UName == userName && u.UPwd == userPwd).FirstOrDefault();
                //判断用户是否存在
                if (userInfo != null)
                {

                    //验证通过，自动登录
                    string sessionId = Guid.NewGuid().ToString(); //产生一个GUID值作为Memcached的的键
                                                                  //将登录用户信息存储到Memcache中
                    Common.MemcachedHelper.SetMemcached(sessionId, Common.SerializeHelper.SerializeToString(userInfo), DateTime.Now.AddMinutes(20));
                    Response.Cookies["sessionId"].Value = sessionId;//将Memcached中的key以cookie的形式返回给浏览器
                    //跳转到用户首页
                    return true;



                }
                else//cookie信息有误，立即删除
                {
                    Response.Cookies["cp1"].Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies["cp2"].Expires = DateTime.Now.AddDays(-1);
                    return false;
                }
            }
            return false;
        }
        
        #region 验证用户登录
        public ActionResult UserLogin()
        {

            #region 验证码检验完成
            //接收验证码
            string userValidateCode = Request["vCode"];
            string validateCode = Session["validateCode"] == null ? string.Empty : Session["validateCode"].ToString();
            //清空Session["validateCode"]
            Session["validateCode"] = null;
            if (string.IsNullOrEmpty(validateCode))
            {
                return Content("no:validateCode");
            }
            if (!string.Equals(userValidateCode, validateCode, StringComparison.CurrentCultureIgnoreCase))
            {
                return Content("no:validateCode");
            }
            #endregion

         

            #region 校对用户名和密码
            string userName = Request["LoginCode"];
            string userPwd = Request["LoginPwd"];
            var userInfo = UserInfoService.LoadEntities(u => u.UName == userName && u.UPwd == userPwd).FirstOrDefault();
            if (userInfo == null)
            {
                return Content("no:user");
            }
            else//登录成功
            {
                //Session["userInfo"] = userInfo;
               
                string sessionId = Guid.NewGuid().ToString(); //产生一个GUID值作为Memcached的的键
                //将登录用户信息存储到Memcache中
                Common.MemcachedHelper.SetMemcached(sessionId, Common.SerializeHelper.SerializeToString(userInfo), DateTime.Now.AddMinutes(20));
                Response.Cookies["sessionId"].Value = sessionId;//将Memcached中的key以cookie的形式返回给浏览器
                //判断用户是否选择了 自动登录
                #region 判断用户是否选择了自动登录
                if (!string.IsNullOrEmpty(Request["cbAutoLogin"]))
                {
                    // 存储cookie的值，方便下次自动登录
                    HttpCookie cookie1 = new HttpCookie("cp1")
                    {
                        Value = HttpUtility.UrlEncode(userName)
                    };
                    HttpCookie cookie2 = new HttpCookie("cp2", userPwd);
                    cookie1.Expires = DateTime.Now.AddDays(7);//7天后自动登录
                    cookie2.Expires = DateTime.Now.AddDays(7);
                    //绑定cookie
                    Response.Cookies.Add(cookie1);
                    Response.Cookies.Add(cookie2);



                } 
                #endregion
                return Content("ok:success");
            } 
            #endregion

        }
        #endregion

        #region 清除cookie及session
        public ActionResult ClearCookie()
        {
            Response.Cookies["cp1"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["cp2"].Expires = DateTime.Now.AddDays(-1);//清理用户名和密码的cookie
            string cache = Request.Cookies["sessionId"].Value; //获取Memcached的的键
            Common.MemcachedHelper.DeleteMemcached(cache);//清除缓存
            Response.Cookies["sessionId"].Expires = DateTime.Now.AddDays(-1);//清除session
            return Content("ok");
        }
        #endregion

    }
}