namespace SND.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;



    using System.IO;
    using System.Threading.Tasks;

    using Utils;
    using Models;
    using BusinessObjects;
    using Common.Utilities;
    using System.IO.Compression;
    using Newtonsoft.Json;
    using System.Web.Script.Serialization;

    [RoutePrefix("api/Sync")]
    public class SyncController : APIBase
    {
        #region Initialize
        [HttpPost]
        public HttpResponseMessage Initialize()
        {
            try
            {
                var _User = GetRawResponse<User>();
                if (!Utils.Security.IsMD5ChecksumValid(_User.device_id + _User.reg_id + "", _User.checksum))
                    return SendToApp(new SND.Models.AppResponse { message = MessageHandler.NotAuthtorizedMsg, status_code = HttpStatusCode.Unauthorized });

                return SendToApp(new SND.Models.AppResponse { message = MessageHandler.SuccessMsg, status_code = HttpStatusCode.OK, file_path = string.Format("http://10.1.10.224/Upload/Sync/{0}/Cloud/", 0) + "MasterData.zip" });

            }
            catch (Exception exception)
            {
               
                Logger.CreateLog(exception.Message);Logger.CreateLog(exception.StackTrace);throw; 
            }
        }
        
        [Route("UploadSyncFile")]
        public HttpResponseMessage UploadSyncFile()
        {
            try
            {

                var _User = GetRawResponse<DAL.User>();
    
                if (!Security.IsMD5ChecksumValid(_User.user_id.ToString() + _User.login_code.ToString() + _User.company_code.ToString() + _User.office_code.ToString() + _User.device_id.ToString() + _User.reg_id.ToString() + "88988934258f3d78ab16462fd68d6a38", _User.checksum))
                    return SendToApp(new SND.Models.AppResponse { message = MessageHandler.NotAuthtorizedMsg, status_code = HttpStatusCode.Unauthorized });
      
                using (SyncBO _SyncBO = new SyncBO())
                {
                    string workItemId = Guid.NewGuid().ToString();
                    if (!string.IsNullOrEmpty( workItemId))
                    {
         
                        //string pathPrefix = string.Format("E:/Release/SMSUAT/Upload/Sync/{0}/Device/", workItemId);
                        string pathPrefix = string.Format("D:/WEBSERVICEORACLE/Upload/", workItemId);
                       // string pathPrefix = string.Format("d:/wamiq/Upload/", workItemId);
                        if (!Directory.Exists(pathPrefix))
                            Directory.CreateDirectory(pathPrefix);
                        byte[] file = System.Convert.FromBase64String(_User.file);
                        File.WriteAllBytes(pathPrefix + workItemId + ".zip", file);
                        Stream files = new MemoryStream(File.ReadAllBytes(pathPrefix + workItemId + ".zip"));
           
                        using (ZipArchive archive = new ZipArchive(files))
                        {
                            foreach (ZipArchiveEntry entry in archive.Entries)
                            {
                            
                                if (entry.FullName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                                {

                                    using (var zipEntryStream = entry.Open())
                                    {

                                        StreamReader sd = new StreamReader(zipEntryStream);
                                        string json = sd.ReadToEnd();
                                        var result = JsonConvert.DeserializeObject<List<RootObject>>(json, new JsonItemConverter());
                                        List<ReponseFormat> ListOFTableAdded = new List<ReponseFormat>();
                                        List<ErrorStack> errStack = new List<ErrorStack>();
                                        //Insert OR Update

                                        foreach (var item in result)
                                        {


                                            foreach (var row in item.rows)
                                            {
                                                if (!row.NeedFK)
                                                    ListOFTableAdded.Add(row.Insert(errStack));
                                                else
                                                    ListOFTableAdded.Add(row.Insert(ListOFTableAdded, errStack));
                                            }
                                        }
                                        ListOFTableAdded.RemoveAll(item => item == null);
                                        //Delete child of a parernt if exceeding after inseritng


                                        ////////Delete new logic 
                                        var rowToBeDeleted = ListOFTableAdded.Where(m => m.isDeleted == "Y" && m.IsError=="N").OrderByDescending(m => m.DeleteOrder).ToList();
                                        foreach (var item in rowToBeDeleted)
                                        {
                                            using (SyncBO _ObjBO = new SyncBO())
                                            {


                                                _ObjBO.DeleteNotNeeded(item.ID, item.ERPTableName);
                                                ListOFTableAdded.Remove(item);
                                            }
                                        }
                                        //dELETE Whole Data
                                        foreach (var item in errStack)
                                        {
                                            using (SyncBO _ObjBO = new SyncBO())
                                            { _ObjBO.DeleteNotNeeded(item.ID, item.TableName); }
                                        }

                                        List<ReponseFormatMobile> CustomData = new List<ReponseFormatMobile>();
                                        foreach (var item in ListOFTableAdded)
                                        {
                                            CustomData.Add(new ReponseFormatMobile() { ERPTableName = item.ERPTableName, ID = item.ID, IDColumn = item.IDColumn, isParent = item.isParent, MapCode = item.MapCode, TableName = item.TableName ,IsError =item.IsError,Message =item.Message });
                                        }
                                        /////Show First Error Msg
                                        var FirstError = CustomData.Where(m => m.IsError == "Y").ToList();
                                        string FirstErrorMsg = (FirstError.Count > 0) ? FirstError.FirstOrDefault().Message : "Data Sync Sucessfully";


                                        var ReposnseData = new JavaScriptSerializer().Serialize(CustomData);
                                        return SendToApp(new Models.AppResponse { message = MessageHandler.SuccessMsg, status_code = HttpStatusCode.OK, data = CustomData,error_msg = FirstErrorMsg });


                                    }
                                }
                            }
                        }



                        return SendToApp(new SND.Models.AppResponse {  message = MessageHandler.SuccessMsg, status_code = HttpStatusCode.OK });
                    }
                    else
                        return SendToApp(new SND.Models.AppResponse { message = MessageHandler.FailedMsg, status_code = HttpStatusCode.NotFound });
                }
            }
            catch (Exception exception)
            {
                
                Logger.CreateLog(exception.Message);Logger.CreateLog(exception.StackTrace);throw; 
            }
        }
        [Route("UnZipAndMerge")]
        public HttpResponseMessage UnZipAndMerge()
        {
            try
            {



                Stream file = new MemoryStream(File.ReadAllBytes("D:/wamiq/1.zip"));
                using (ZipArchive archive = new ZipArchive(file))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (entry.FullName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                        {
                            using (var zipEntryStream = entry.Open())
                            {

                                StreamReader sd = new StreamReader(zipEntryStream);
                                string json = sd.ReadToEnd();
                                var result = JsonConvert.DeserializeObject<List<RootObject>>(json, new JsonItemConverter());
                                List<ReponseFormat> ListOFTableAdded = new List<ReponseFormat>();
                                List<ErrorStack> errStack = new List<ErrorStack>();
                                //Insert OR Update

                                foreach (var item in result)
                                {


                                    foreach (var row in item.rows)
                                    {
                                        if (!row.NeedFK)
                                            ListOFTableAdded.Add(row.Insert(errStack));
                                        else
                                            ListOFTableAdded.Add(row.Insert(ListOFTableAdded, errStack));
                                    }
                                }
                                ListOFTableAdded.RemoveAll(item => item == null);
                                //Delete child of a parernt if exceeding after inseritng


                                ////////Delete new logic 
                                var rowToBeDeleted = ListOFTableAdded.Where(m => m.isDeleted == "Y" && m.IsError == "N").OrderByDescending(m => m.DeleteOrder).ToList();
                                foreach (var item in rowToBeDeleted)
                                {
                                    using (SyncBO _ObjBO = new SyncBO())
                                    {


                                        _ObjBO.DeleteNotNeeded(item.ID, item.ERPTableName);
                                        ListOFTableAdded.Remove(item);
                                    }
                                }
                                //dELETE Whole Data
                                foreach (var item in errStack)
                                {
                                    using (SyncBO _ObjBO = new SyncBO())
                                    { _ObjBO.DeleteNotNeeded(item.ID, item.TableName); }
                                }

                                List<ReponseFormatMobile> CustomData = new List<ReponseFormatMobile>();
                                foreach (var item in ListOFTableAdded)
                                {
                                    CustomData.Add(new ReponseFormatMobile() { ERPTableName = item.ERPTableName, ID = item.ID, IDColumn = item.IDColumn, isParent = item.isParent, MapCode = item.MapCode, TableName = item.TableName, IsError = item.IsError, Message = item.Message });
                                }
                                /////Show First Error Msg
                                var FirstError = CustomData.Where(m => m.IsError == "Y").ToList();
                                string FirstErrorMsg = (FirstError.Count > 0) ? FirstError.FirstOrDefault().Message : "Data Sync Sucessfully";


                                var ReposnseData = new JavaScriptSerializer().Serialize(CustomData);
                                return SendToApp(new Models.AppResponse { message = MessageHandler.SuccessMsg, status_code = HttpStatusCode.OK, data = CustomData, error_msg = FirstErrorMsg });


                            }
                        }
                    }
                }
                return SendToApp(new SND.Models.AppResponse { message = MessageHandler.SuccessMsg, status_code = HttpStatusCode.OK });


            }
            catch (Exception exception)
            {

                Logger.CreateLog(exception.Message); Logger.CreateLog(exception.StackTrace); throw;
            }
        }
        [Route("UploadInquiryFile")]
        public HttpResponseMessage UploadInquiryFile()
        {
            try
            {
                var _User = GetRawResponse<User>();
                if (!Security.IsMD5ChecksumValid(_User.user_id.ToString() + _User.login_code.ToString() + _User.company_code.ToString() + _User.office_code.ToString() + _User.device_id.ToString() + _User.reg_id.ToString() + "88988934258f3d78ab16462fd68d6a38", _User.checksum))
                    return SendToApp(new Models.AppResponse { message = MessageHandler.NotAuthtorizedMsg, status_code = HttpStatusCode.Unauthorized });

                using (SyncBO _SyncBO = new SyncBO())
                {
                    string pathPrefix = string.Format("E:/Release/SMSUAT/Upload/Sync/{0}/Device/", _User.device_id);
                    if (!Directory.Exists(pathPrefix))
                        Directory.CreateDirectory(pathPrefix);
                    byte[] file = System.Convert.FromBase64String(_User.file);
                    File.WriteAllBytes(pathPrefix + DateTime.Now.ToString("ddMMMyyyyHHmmss") + ".zip", file);
                    return SendToApp(new Models.AppResponse { message = MessageHandler.SuccessMsg, status_code = HttpStatusCode.OK });

                }
            }
            catch (Exception exception)
            {
                //Logger.PrintError(exception);
                Logger.CreateLog(exception.Message);Logger.CreateLog(exception.StackTrace);throw; 
            }
        }
        #endregion
    }
}
