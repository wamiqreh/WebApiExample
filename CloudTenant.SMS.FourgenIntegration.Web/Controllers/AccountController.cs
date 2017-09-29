using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using WebApplication1.Models;
using WebApplication1.Providers;
using WebApplication1.Results;
using DAL;
using SND;
using SND.BusinessObjects;
using Common.Enumeration;
using Common.Models;
using System.Linq;
using System.Net;
using DAL.DataAccess;
using Common.Utilities;
using System.ComponentModel;
using System.Data;
using DAL.Provider;
using System.Data.SqlClient;

namespace WebApplication1.Controllers
{
   

    [RoutePrefix("api/Account")]
    public class AccountController  : APIBase
    {
        #region login
        [HttpPost]
        [Route("Login")]
        public HttpResponseMessage Login()
        {
            #region committed
            try
            {
               // Logger.CreateLog("in");
                var _User = GetRawResponse<User>();

                //if (!Security.IsMD5ChecksumValid(_User.username + _User.password + _User.device_id + _User.reg_id + "88988934258f3d78ab16462fd68d6a38", _User.checksum))
                //    return SendToApp(new SND.Models.AppResponse { message = "Not Authorized", status_code = HttpStatusCode.Unauthorized });
              //  Logger.CreateLog("0");
                using (LoginBO _objUserBO = new LoginBO())
                {
                  //  Logger.CreateLog("111");
                    Dictionary<enLoginResponse, UserModel> dictionaryOfUserInfo = _objUserBO.GetLogininfoByCredentials(_User.username, _User.password);
                    enLoginResponse response = dictionaryOfUserInfo.ElementAt(0).Key;
                    switch (response)
                    {
                        case enLoginResponse.AccountExpired:
                            return SendToApp(new SND.Models.AppResponse { message = "Account Expired", status_code = HttpStatusCode.NotFound });
                        case enLoginResponse.AccountLock:
                            return SendToApp(new SND.Models.AppResponse { message = "Account Locked", status_code = HttpStatusCode.NotFound });
                        case enLoginResponse.ActivationRequired:
                            return SendToApp(new SND.Models.AppResponse { message = "Account activation required", status_code = HttpStatusCode.NotFound });
                        case enLoginResponse.Failed:
                            return SendToApp(new SND.Models.AppResponse { message = "Failed", status_code = HttpStatusCode.NotFound });
                        case enLoginResponse.InValidUsernameOrPassword:
                            return SendToApp(new SND.Models.AppResponse { message = "Invalid Username or Password", status_code = HttpStatusCode.NotFound });
                        case enLoginResponse.PasswordExpired:
                            return SendToApp(new SND.Models.AppResponse { message = "Password has expired", status_code = HttpStatusCode.NotFound });
                        case enLoginResponse.NoCompany:
                            return SendToApp(new SND.Models.AppResponse { message = "No Company", status_code = HttpStatusCode.NotFound });
                        case enLoginResponse.NoOffice:
                            return SendToApp(new SND.Models.AppResponse { message = "No Office", status_code = HttpStatusCode.NotFound });
                        case enLoginResponse.NoCompanyNoOffice:
                            return SendToApp(new SND.Models.AppResponse { message = "No Company and Office", status_code = HttpStatusCode.NotFound });
                        case enLoginResponse.Success:
                            {
                                UserModel row = dictionaryOfUserInfo.ElementAt(0).Value;
                                _User.user_id = row.Id.ToString();
                                _User.login_code = 1;// Convert.ToDecimal(row.LoginCode.ToString());
                                _User.company_code = row.CompanyInfo.Id.ToString();
                                _User.office_code = row.OfficeInfo.ToString();
                                _User.priority_code = 1;

                                //   Device Verifications
                                //    Service Call
                               // Logger.CreateLog("1"); 
                                DAL.DataAccess.LoginDAO LoginContext = new LoginDAO();
                                decimal deviceId = LoginContext.IsDeviceRegistered(_User.user_id, _User.login_code, _User.device_id, _User.reg_id, _User.brand_name, _User.model_name, _User.os_version, _User.resolution,_User.company_code);
                                if (deviceId <= 0)
                                    return SendToApp(new SND.Models.AppResponse { message = "Not Authorized", status_code = HttpStatusCode.Unauthorized });
                                //Logger.CreateLog("2");
                                // User Location(s)
                                // TODO: NEED TO SEND ONLY USER SPECIFIC OFFICES
                                //row.ListOfOffices = new OfficesComBO().GetOfficesComByUserId(_User.user_id);
                                row.DeviceInfo = LoginContext.GetDeviceInfoById(deviceId);
                                row.ListOfOffices = _objUserBO.GetUsersOffices(row.Id.ToString());
                                //row.DeviceInfo = null;
                                // Logger.CreateLog("2");
                                //row.supplierList = new ItemDAO().GetSupplierDetail();
                                row.supplierList = null; 
                                //Logger.CreateLog("3");
                                row.StockStatus = new ItemDAO().GetStockTypeDetail();
                                //Logger.CreateLog("4");
                                row.warehouseList = new ItemDAO().GetWarehouseDetail();
                                //row.IndentOfflineList = new ItemDAO().GetStockInHandForIndent();
                                //GetStockInHandForIndent

                                // Logger.CreateLog("5");
                                row.aisleList = new ItemDAO().GetAISLESDetail();
                                row.bayList = new ItemDAO().GetBayDetail();
                                row.ThirdParty = "Y";
                                ///Optional Feilds
                                row.TransferInReasonList = new TransferDAO().GetReasonsType();
                                row.IntendReasonList = new ItemDAO().GetIntendReason();
                                row.TransferTypeList = new ItemDAO().GetTransferType();
                                row.DepartmentList = new ItemDAO().GetAllDepartment();
                               
                                return SendToApp(new SND.Models.AppResponse { message = "Done", status_code = HttpStatusCode.OK, data = row });



                            }

                        default:
                            return SendToApp(new SND.Models.AppResponse { message = "Failed", status_code = HttpStatusCode.NotFound });
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.CreateLog(exception.Message); Logger.CreateLog(exception.StackTrace); throw;

            }
            #endregion

        }



      

        #endregion
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
            {
                Email = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
            {
                return null;
            }

            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

            foreach (IdentityUserLogin linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = user.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);
            
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/AddExternalLogin
        [Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("External login failure.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return BadRequest("The external login is already associated with an account.");
            }

            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }
        
        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null)
            {
                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                
                 ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            else
            {
                IEnumerable<Claim> claims = externalLogin.GetClaims();
                ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                Authentication.SignIn(identity);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RegisterExternal
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var info = await Authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return InternalServerError();
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            result = await UserManager.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result); 
            }
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}
