using Peacock.PEP.Data.Entities;
using Peacock.PEP.Service.Base;
using Peacock.PEP.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Peacock.PEP.Repository.Repositories;
using Peacock.Common.Helper;
using Peacock.PEP.Model.ApiModel;
using ResourceLibraryExtension.Helper;
using System.IO;
using Newtonsoft.Json;

namespace Peacock.PEP.Service
{
    public class ProjectDocumentService : SingModel<ProjectDocumentService>
    {
        private ProjectDocumentService()
        {
        }
        //测试方法
        public void SendFileList()
        {
            List<OnlineBusinessDocmentModel> list = new List<OnlineBusinessDocmentModel>() { 
            new OnlineBusinessDocmentModel{
             fileType="图片",
              fileName="Chrysanthemum",
               fileFormat="jpg",
                fileCategory="身份证",
                 resourceID=559308
            },
            new OnlineBusinessDocmentModel{
             fileType="图片",
              fileName="Lighthouse",
               fileFormat="jpg",
                fileCategory="身份证",
                resourceID=559308
            },
            new OnlineBusinessDocmentModel{
             fileType="图片",
              fileName="Penguins",
               fileFormat="jpg",
                fileCategory="身份证",
                resourceID=559308
            },
            new OnlineBusinessDocmentModel{
             fileType="图片",
              fileName="Penguins",
               fileFormat="jpg",
                fileCategory="身份证",
                resourceID=559308
            }
            };
            //var aa=JsonConvert.SerializeObject(st1);
            ReceiveDataEntity data = new ReceiveDataEntity
            {
                bussinessId = "20160429121605",
                bussinessType = "附件上传",
                bussinessForm = JsonConvert.SerializeObject(list)
            };
            var aa = JsonConvert.SerializeObject(data);
            KafkaMQService.Instance.ReceiveMessage(aa);
            //KafkaMQService.Instance.SendMessageTest<ReceiveDataEntity>(data);
        }
        private byte[] FileContent(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            try
            {
                byte[] buffur = new byte[fs.Length];
                fs.Read(buffur, 0, (int)fs.Length);
                return buffur;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (fs != null)
                {//关闭资源  
                    fs.Close();
                }
            }
        }
        /// <summary>
        /// 获取线上资源
        /// </summary>
        /// <param name="onLineBusinessId"></param>
        /// <returns></returns>
        public IList<ProjectDocument> GetDocumentList(long onLineBusinessId)
        {
            LogHelper.Ilog("GetDocumentList?onLineBusinessId=" + onLineBusinessId, "获取线上资源-" + Instance.ToString());
            return ProjectDocumentRepository.Instance.Find(x => x.OnLineBussinessId == onLineBusinessId).ToList();
        }

        /// <summary>
        /// 在线业务附件上传、更新
        /// </summary>
        /// <param name="Model"></param>
        /// <param name="businessType"></param>
        public void UploadOnlineDocment(List<OnlineBusinessDocmentModel> FileList, string TransactionNo, BussinessType businessType)
        {
            var business = OnLineBusinessRepository.Instance.Find(x => x.TransactionNo == TransactionNo).FirstOrDefault();
            if (business == null)
            {
                KafkaMQService.Instance.SendMessage(new CallBackMessageEntity
                {
                    onLineBusinessID = "",
                    businessId = TransactionNo,
                    businessType = businessType.ToString(),
                    bussinessForm = new BussinessForm {  data=null,message = "委托不存在，不能上传附件",success=false },
                });
                return;
            }
            if (business.Project != null)
            {
                var isAduit = ProjectStateInfoService.Instance.GetProjectStateInfoList(business.Project.Id).Select(x => x.Content).Contains("提交审核");
                if (isAduit)
                {
                    KafkaMQService.Instance.SendMessage(new CallBackMessageEntity
                    {
                        onLineBusinessID = business.Id.ToString(),
                        businessId = TransactionNo,
                        businessType = businessType.ToString(),
                        bussinessForm = new BussinessForm { data = null,message = "项目已提交审核，不提供上传、更新附件", success = false }
                    });
                    return;
                }
            }
            bool IsValidate = true;
            //LogHelper.Ilog("在线业务附件上传、更新-" + Instance.ToString() + ".UploadOnlineDocment");
            //验证模型中数据的合法性
            foreach (var Model in FileList)
            {
                string ErrorMsg = string.Empty;
                //验证模型中的值是否合法
                if (!ValidateModel.ValidateEntityData<OnlineBusinessDocmentModel>(Model, out ErrorMsg))
                {
                    KafkaMQService.Instance.SendMessage(new CallBackMessageEntity
                    {
                        onLineBusinessID = "",
                        businessId = TransactionNo,
                        businessType = businessType.ToString(),
                        bussinessForm = new BussinessForm {data=null, message = ErrorMsg, success = false }
                    });
                    IsValidate = false;
                    break;
                }
            }
            if (!IsValidate) return;

            try
            {
                //首先删除该委托单的
                ProjectDocumentRepository.Instance.Delete(x => x.OnLineBussinessId == business.Id);
                foreach (var Model in FileList)
                {
                    ProjectDocumentRepository.Instance.Insert(new ProjectDocument
                    {
                        OnLineBussinessId = business.Id,
                        CreateTime = DateTime.Now,
                        FileName = Model.fileName,
                        Project = business.Project,
                        ResourceId = Model.resourceID,
                        FileClass = Model.fileCategory,
                        FileFormat = Model.fileFormat,
                        FileType = Model.fileType
                    });
                }
            }
            catch (Exception ex)
            {
                //LogHelper.Ilog("在线业务附件上传、更新,异常:" + ex.ToString() + ";" + Instance.ToString() + ".UploadOnlineDocment");
                KafkaMQService.Instance.SendMessage(new CallBackMessageEntity
                {
                    onLineBusinessID = business.Id.ToString(),
                    businessId = TransactionNo,
                    businessType = businessType.ToString(),
                    bussinessForm = new BussinessForm { message = "附件上传异常", success = false }
                });
            }
        }
    }
}
