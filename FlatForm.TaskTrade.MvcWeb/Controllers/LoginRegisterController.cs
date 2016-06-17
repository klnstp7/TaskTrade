using Peacock.Common.Helper;
using Peacock.PEP.DataAdapter;
using Peacock.PEP.DataAdapter.Interface;
using Peacock.PEP.Model;
using PermissionsMiddle.Dto;
using System;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Peacock.InWork4.Voucher.MvcWebSite.Controllers
{
    public class LoginRegisterController : Controller
    {
        protected static readonly IUserAdapter UserService = ConditionFactory.Conditions.Resolve<IUserAdapter>();
        protected static readonly ICompanyAdapter CompanyService = ConditionFactory.Conditions.Resolve<ICompanyAdapter>();

        //
        // GET: /LoginRegister/

        public ActionResult Register()
        {
            string url = ConfigurationManager.AppSettings["ReLogInUrl"];
            ViewBag.ReLogInUrl = url;
            return View();
        }

        //
        // GET: /LoginRegister/浏览 *:14000 (http)

        //public ActionResult Login()
        //{
        //    return View();
        //}


        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public JsonResult Register(UserApiDto dto)
        {
            ResultInfo result = new ResultInfo();
            if (ModelState.IsValid)
            {
                string securityCode = CookieHelper.GetCookie(dto.Phone);
                if (securityCode == dto.Desc)
                {
                    dto.UserName = dto.UserAccount;
                    //dto.Password = dto.Password.ToMD5();
                    dto.CreateDateTime = DateTime.Now;
                    //if (string.IsNullOrWhiteSpace(dto.CompanyName))
                    //{
                    //    dto.CompanyName = ConfigurationManager.AppSettings["PersonalUserCompany"];
                    //}
                    result = UserService.SaveUserInfo(dto);
                    result.Others = ConfigurationManager.AppSettings["ReLogInUrl"];
                }
                else
                {
                    result.Message = "手机验证码不正确或已失效";
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult RegisterCompany()
        {
            ResultInfo result = new ResultInfo();
            if (ModelState.IsValid)
            {
                CompanyModel dto = new CompanyModel();
                dto.CompanyName = Request["CompanyName"] ?? "";
                dto.Address = Request["CompanyAddress"] ?? "";
                dto.UserName = Request["CompanyUserName"] ?? "";
                dto.Password = Request["CompanyPassword"] ?? "";
                dto.City = Request["City"] ?? "";
                dto.Contact = Request["Contact"] ?? "";
                dto.Tel = Request["Tel"] ?? "";
                dto.Remark = Request["Remark"] ?? "";

                if (Request.Files.Count > 0)
                {
                    string[] ext = Request.Files[0].FileName.Split('.');//获取文件后缀.
                    int extIndex = ext.Length - 1;
                    if (ext.Length > 1 && (ext[extIndex].ToUpper() == "JPG" || ext[extIndex].ToUpper() == "BMP" || ext[extIndex].ToUpper() == "PNG" || ext[extIndex].ToUpper() == "GIF" || ext[extIndex].ToUpper() == "JPEG"))
                    {
                        CompanyResourseModel resourseDto = new CompanyResourseModel();
                        byte[] fileConcent = FileStreamHelper.StreamToBytes(Request.Files[0].InputStream);
                        resourseDto.FileConcent = fileConcent;
                        resourseDto.FileName = Request.Files[0].FileName;
                        resourseDto.Ext = ext[extIndex];
                        result = CompanyService.SaveCompany(dto, resourseDto);
                        result.Others = ConfigurationManager.AppSettings["ReLogInUrl"];
                    }
                    else
                    {
                        result.Message = "上传资源限于png,gif,jpeg,jpg格式";
                    }
                }
                else
                {
                    result.Message = "请上传企业资料";
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 发送手机验证码
        /// </summary>
        /// <param name="phoneNumber">需要发送的号码</param>        
        /// <returns></returns>
        [AllowAnonymous]
        public JsonResult SendSecurityCode(string phoneNumber)
        {
            ResultInfo result = new ResultInfo();
            if (Regex.IsMatch(phoneNumber, @"^1\d{10}$"))
            {
                string securityCode = StringHelper.GetSecurityCode();
                CookieHelper.WriteCookie(phoneNumber, securityCode);
                //设置验证码5分钟失效
                CookieHelper.SetCookieExpires(phoneNumber, DateTime.Now.AddMinutes(5));
                result = CookieHelper.SendSecurityCode(phoneNumber, securityCode);
            }
            else
            {
                result.Message = "请填写正确的手机号码";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取CRM公司
        /// </summary>         
        /// <returns></returns>
        [AllowAnonymous]
        public JsonResult GetCompanys()
        {
            ResultInfo result = UserService.GetCRMCompanys();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
