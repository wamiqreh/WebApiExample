using Common.Enumeration;
using Common.Models;
using Common.Utilities;
using DAL;
using DAL.DataAccess;
using SND.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SND.Controllers
{

    [RoutePrefix("api/Accountss")]
    public class AcntController :APIBase
    {
        #region login
        [HttpPost]
        [Route("Login")]
        public HttpResponseMessage Login()
        {
            #region committed
            try
            {
                var _User = GetRawResponse<User>();

                //if (!Security.IsMD5ChecksumValid(_User.username + _User.password + _User.device_id + _User.reg_id + "88988934258f3d78ab16462fd68d6a38", _User.checksum))
                //    return SendToApp(new SND.Models.AppResponse { message = "Not Authorized", status_code = HttpStatusCode.Unauthorized });

                using (LoginBO _objUserBO = new LoginBO())
                {
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

                                // Device Verifications
                                // Service Call
                                //decimal deviceId = _objUserBO.IsDeviceRegistered(_User.user_id, _User.login_code, _User.device_id, _User.reg_id, _User.brand_name, _User.model_name, _User.os_version, _User.resolution);
                                //if (deviceId <= 0)
                                //    return SendToApp(new SND.Models.AppResponse { message = Resources.Message.LOGIN_DEVICE_UNATHORIZED_DEVICE, status_code = HttpStatusCode.Unauthorized });

                                // User Location(s)
                                // TODO: NEED TO SEND ONLY USER SPECIFIC OFFICES
                                //row.ListOfOffices = new OfficesComBO().GetOfficesComByUserId(_User.user_id);
                                //row.DeviceInfo = _objUserBO.GetDeviceInfoById(deviceId);
                                row.ListOfOffices = null;
                                row.DeviceInfo = null;
                                row.Supplier = new ItemDAO().GetSupplierDetail();
                                row.StockStatus = new ItemDAO().GetStockTypeDetail();
                                row.Warehouse = new ItemDAO().GetWarehouseDetail();
                                row.Aisle = new ItemDAO().GetBayDetail();
                                row.Bay = new ItemDAO().GetBayDetail();
                                row.ThirdParty = "Y";
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
    }
}
