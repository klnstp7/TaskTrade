using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Peacock.PEP.Data.Entities;
using Peacock.PEP.Repository.Repositories;
using Peacock.PEP.Service.Base;
using Peacock.PEP.Model.Condition;
using Peacock.Common.Exceptions;
using Peacock.Common.Helper;
using Peacock.PEP.Service.Extensions;
using System.Reflection;
using Peacock.PEP.Model.ApiModel;
using ResourceLibraryExtension.Helper;
using log4net;

namespace Peacock.PEP.Service
{
    public class OnlineBusinessService : SingModel<OnlineBusinessService>
    {
        private OnlineBusinessService()
        {
        }

        public void TestSendData()
        {
            OnlineBusinessApiModel Model = new OnlineBusinessApiModel
            {
                //transactionNo = "20160429121604",//DateTime.Now.ToString("yyyyMMddHHmmss"),
                area = "100",
                expectPrice = "230",
                //assessPrice = "20000",
                evaluationCompanyId = 1,
                //borrowCardType = "身份证",
                roomType = "三",
                //borrowName = "借款人1",
                //borrowID = "4030303023874333",
                //buildYear = "1998",
                cityName = "北京市",
                residentialAreaName = "小区",
                dataSource = "链家",
                bussinessContactTelephone = "12938899333",
                bussinessContactName = "经纪人",
                //decorateCase = "毛坯",
                //delegateName = "代理名称",
                //delegatePhone = "1235434343",
                floor = "3",
                totalFloor = "6",
                //floorBuilding = "测试楼栋号",
                //highLowAssess = "高评",
                address = "测试小区地址",
                //houseNo = "dffd",
                planUse = "住宅",
                region = "西城区",
                toward = "南",
                //recipientName = "报告接收人",
                //reportReceiveCity = "广州市",
                //recipientTelephone = "2334343434343",
                //reportReceiveProvince = "广东省",
                //reportReceiveRegion = "越秀区",
                //reportReceiveStreet = "水荫路52号中环大厦9楼",
                //seeHouseAddr = "水荫路52号中环大厦9楼",
                loanType = "商贷",
                houseContact = new List<houseContact>() { 
                    new houseContact {houseContactName = "中国农业银行", houseContactTelephone = "1234567"},
                    new houseContact {houseContactName = "招商银行", houseContactTelephone = "1234567"}
                },
                //sellerCardType = "身份证",
                //unitName = "单元2",
                bank = new List<BankInfo>() { new BankInfo { bankbranchName = "招商银行-广州分行", bankName = "招商银行" },
                    new BankInfo {bankbranchName = "招商银行-广州分行", bankName = "招商银行"},
                    new BankInfo {bankbranchName = "广发银行-广州分行", bankName = "广发银行"}
                }
            };

            OnlineBusinessApiModel Modelzhengshi = new OnlineBusinessApiModel
            {
                //transactionNo = "20160429121606",//DateTime.Now.ToString("yyyyMMddHHmmss"),
                area = "100",
                expectPrice = "230",
                evaluationCompanyId = 1,
                roomType = "三",
                doorModelHall = "一",
                doorModelWash = "二",
                cityName = "北京市",
                residentialAreaName = "小区",
                dataSource = "链家",
                bussinessContactTelephone = "12938899333",
                bussinessContactName = "经纪人",
                decorateCase = "毛坯",
                delegateName = "委托人（买房人）",
                delegatePhone = "1235434343",
                delegateCardType = "身份证",
                delegateCardID = "身份证号：8989898989-9-0",
                floor = "3",
                totalFloor = "6",
                floorBuilding = "测试楼栋号",
                highLowAssess = "高评",
                address = "正式委托测试测试小区地址",
                houseNo = "dffd",
                planUse = "住宅",
                region = "西城区",
                toward = "南",
                buildYear = "1997",
                recipientName = "报告接收人",
                loanRatio = "3",
                reportNum = "1",
                comment = "正式委托测试",
                houseType = "住宅",
                houseNature = "房屋性质",
                realMoney = "120",
                contractMoney = "150",
                recipientTelephone = "2334343434343",
                recipientAddress = "广东省广州市越秀区水荫路52号中环大厦9楼云房数据",
                loanType = "商贷",
                houseContact = new List<houseContact>() { 
                   new houseContact { houseContactName = "中国农业银行", houseContactTelephone = "18684869820" },
                   new houseContact { houseContactName = "招商银行", houseContactTelephone = "123456709888" } },
                sellerCardType = "身份证",
                sellerName = "卖房人",
                sellerPhone = "135990003333",
                sellerID = "4033231967332323388",
                unitName = "单元2",
                bank = new List<BankInfo>() { new BankInfo { bankbranchName = "招商银行-广州分行", bankName = "招商银行" },
                new BankInfo { bankbranchName = "农业银行北京", bankName = "农业银行北京分行" }}
            };

            var formData = JsonConvert.SerializeObject(Modelzhengshi);
            var businessId = DateTime.Now.ToString("yyyyMMddHHmmss");
            ReceiveDataEntity data = new ReceiveDataEntity
            {
                bussinessId = businessId,
                bussinessType = "信息补全",
                bussinessForm = formData
            };
            var yupinggu = JsonConvert.SerializeObject(Model);
            ReceiveDataEntity data1 = new ReceiveDataEntity
            {
                bussinessId = businessId,
                bussinessType = "预评估",
                bussinessForm = yupinggu
            };
            KafkaMQService.Instance.ReceiveMessage(JsonConvert.SerializeObject(data1));
            KafkaMQService.Instance.ReceiveMessage(JsonConvert.SerializeObject(data));
            //KafkaMQService.Instance.SendMessage<ReceiveDataEntity>(data);
        }

        /// <summary>
        /// 根据在线业务ID获取银行列表
        /// </summary>
        /// <param name="ProjectId">在线业务ID</param>
        /// <returns></returns>
        public string GetBankListByOnlineBusinessId(long onlineBusinessId)
        {
            LogHelper.Ilog("GetBankListByOnlineBusinessId?onlineBusinessId=" + onlineBusinessId, "根据在线业务ID获取银行列表-" + Instance.ToString());
            var query = OnLineBusinessBankRepository.Instance.Source;
            query = query.Where(x => x.OnBusinessId == onlineBusinessId);
            var result = query.ToList().Select(x => x.BankName + "(" + x.BankbranchName + ")").ToList();
            return string.Join(",", result);
        }

        /// <summary>
        /// 保存在线业务
        /// </summary>
        /// <param name="pf"></param>
        /// <returns></returns>
        public long Save(OnLineBusiness entity)
        {
            LogHelper.Ilog("Save?entity=" + entity.ToJson(), "保存线上业务-" + Instance.ToString());
            OnLineBusinessRepository.Instance.Insert(entity);
            return entity.Id;
        }

        /// <summary>
        /// 保存在线业务（线上业务）
        /// </summary>
        /// <returns></returns>
        public void SaveOnLineDataByKafkaMq(OnlineBusinessApiModel Model, string DataSource, string BusinessId, BussinessType businessType)
        {
            string ErrorMsg = string.Empty;
            //验证模型中的值是否合法
            if (!ValidateModel.ValidateEntityData<OnlineBusinessApiModel>(Model, out ErrorMsg))
            {
                KafkaMQService.Instance.SendMessage(new CallBackMessageEntity
                {
                    onLineBusinessID = "",
                    businessId = BusinessId,
                    businessType = businessType.ToString(),
                    bussinessForm = new BussinessForm
                    {
                        success = false,
                        message = ErrorMsg
                    }

                });
                return;
            }

            if (businessType == BussinessType.信息补全)
            {
                OnLineBusiness OnlineData = OnLineBusinessRepository.Instance.Find(x => x.TransactionNo == BusinessId).FirstOrDefault();
                if (OnlineData != null && OnlineData.Project != null)
                {
                    KafkaMQService.Instance.SendMessage(new CallBackMessageEntity
                    {
                        onLineBusinessID = OnlineData.Id.ToString(),
                        businessId = OnlineData.TransactionNo,
                        businessType = businessType.ToString(),
                        bussinessForm = new BussinessForm
                        {
                            success = false,
                            message = "操作失败！项目已立项，不能更新数据！"
                        }
                    });
                    return;
                }
                else
                {
                    OnLineBusinessBankRepository.Instance.Delete(x => x.OnBusinessId == OnlineData.Id);
                    ExplorationContactsRepository.Instance.Delete(x => x.OnlineBusinessId == OnlineData.Id);
                    ModelToEnity(ref OnlineData, Model);
                    OnlineData.DataSource = DataSource;
                    OnlineData.LastupdateTime = DateTime.Now;
                    OnLineBusinessRepository.Instance.Save(OnlineData);
                    KafkaMQService.Instance.SendMessage(new CallBackMessageEntity
                    {
                        onLineBusinessID = OnlineData.Id.ToString(),
                        businessId = OnlineData.TransactionNo,
                        businessType = businessType.ToString(),
                        bussinessForm = new BussinessForm
                        {
                            success = true,
                            message = "操作成功！"
                        }
                    });
                }
            }
            else
            {
                OnLineBusiness entity = new OnLineBusiness();
                ModelToEnity(ref entity, Model);
                entity.DataSource = DataSource;
                entity.TransactionNo = BusinessId;
                if (businessType == BussinessType.预评估)
                    entity.DelegateType = OnLineBusiness.DelegateTypeEnum.预评估;
                if (businessType == BussinessType.正式委托)
                    entity.DelegateType = OnLineBusiness.DelegateTypeEnum.正式委托;
                if (businessType == BussinessType.预评估转正式)
                    entity.DelegateType = OnLineBusiness.DelegateTypeEnum.正式委托;
                OnLineBusinessRepository.Instance.Insert(entity);
                KafkaMQService.Instance.SendMessage(new CallBackMessageEntity
                {
                    onLineBusinessID = entity.Id.ToString(),
                    businessId = entity.TransactionNo,
                    businessType = businessType.ToString(),
                    bussinessForm = new BussinessForm
                    {
                        success = true,
                        message = "操作成功！"
                    }
                });
            }
        }

        /// <summary>
        /// 模型转实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="Model"></param>
        private void ModelToEnity(ref OnLineBusiness entity, OnlineBusinessApiModel Model)
        {
            entity.Remark = Model.comment;
            entity.ExplorationContacts = Model.houseContact == null ? new List<ExplorationContacts>() : Model.houseContact.Select(x => new ExplorationContacts { Contacts = x.houseContactName, Phone = x.houseContactTelephone }).ToList();
            entity.OnLineBusinessBanks = Model.bank == null ? new List<OnLineBusinessBank>() : Model.bank.Select(x => new OnLineBusinessBank { OnBusinessId = 0, BankName = x.bankName, BankbranchName = x.bankbranchName }).ToList();
            entity.HouseAddress = Model.address;
            entity.ReportReceiveStreet = Model.reportReceiveStreet;
            entity.Area = Model.area;
            entity.Assessment = Model.expectPrice;
            entity.AssessPrice = Model.assessPrice;
            entity.BorrowCardType = Model.borrowCardType;
            entity.BorrowID = Model.borrowID;
            entity.BorrowName = Model.borrowName;
            entity.BuildYear = Model.buildYear;
            entity.City = Model.cityName;
            entity.Orientation = Model.toward;
            entity.Community = Model.residentialAreaName;
            entity.DealuserMobile = Model.bussinessContactTelephone;
            entity.DealuserName = Model.bussinessContactName;
            entity.DecorateCase = Model.decorateCase;
            entity.DelegateName = Model.delegateName;
            entity.DelegatePhone = Model.delegatePhone;
            entity.DelegateCardType = Model.delegateCardType;
            entity.DelegateCardId = Model.delegateCardID;
            entity.DepartmentId = Model.evaluationCompanyId;
            entity.DoorModelHall = Model.doorModelHall;
            entity.DoorModelRoom = Model.roomType;
            entity.DoorModelWash = Model.doorModelWash;
            entity.Floor = Model.floor;
            entity.TotalFloor = Model.totalFloor;
            entity.FloorBuilding = Model.floorBuilding;
            entity.HighLowAssess = Model.highLowAssess;
            entity.HouseNo = Model.houseNo;
            entity.PlanUse = Model.planUse;
            entity.ReportReceive = Model.recipientName;
            entity.ReportReceiveCity = Model.reportReceiveCity;
            entity.ReportReceivePhone = Model.recipientTelephone;
            entity.ReportReceiveProvince = Model.reportReceiveProvince;
            entity.ReportReceiveRegion = Model.reportReceiveRegion;
            entity.SeeHouseAddr = Model.seeHouseAddr;
            entity.SellerCardType = Model.sellerCardType;
            entity.PostAddress = Model.recipientAddress;
            entity.SellerID = Model.sellerID;
            entity.SellerName = Model.sellerName;
            entity.SellerPhone = Model.sellerPhone;
            entity.Region = Model.region;
            entity.UnitName = Model.unitName;
            entity.LoanType = Model.loanType;
            entity.LoanRatio = Model.loanRatio;
            entity.RealMoney = Model.realMoney;
            entity.ContractMoney = Model.contractMoney;
            entity.DataSource = Model.dataSource;
            entity.HouserOperties = Model.houseNature;
            entity.PropertysType = Model.houseType;
            entity.ReportQuantity = Model.reportNum;
        }

        /// <summary>
        /// 获取在线业务列表
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<OnLineBusiness> GetOnlineBusinessList(OnLineBusinessCondition condition, int index, int size,
            out int total)
        {
            LogHelper.Ilog("GetOnlineBusinessList?entity=" + condition.ToJson() + "&index=" + index + "&size=" + size, "查询线上报告-" + Instance.ToString());
            var personalCompany = UserService.Instance.GetDepartmentByName(ConfigurationManager.AppSettings["PersonalUserCompany"]);
            if (UserService.Instance.GetUser().CompanyId == personalCompany.Id)
            {
                total = 0;
                return null;
            }
            var query = OnLineBusinessRepository.Instance.Source;
            //按数据权限查询列表
            query = query.FindByPower();

            if (!string.IsNullOrWhiteSpace(condition.ProjectNo))
            {
                IList<long> projectids =
                    ProjectRepository.Instance.Find(x => x.ProjectNo.Contains(condition.ProjectNo))
                        .Select(x => x.Id)
                        .ToList();
                query = query.Where(x => projectids.Contains(x.ProjectId ?? 0));
            }
            if (!string.IsNullOrWhiteSpace(condition.TransactionNo))
                query = query.Where(x => x.TransactionNo.Contains(condition.TransactionNo));
            if (!string.IsNullOrWhiteSpace(condition.HouseAddress))
                query = query.Where(x => x.HouseAddress.Contains(condition.HouseAddress));
            if (!string.IsNullOrWhiteSpace(condition.Community))
                query = query.Where(x => x.Community.Contains(condition.Community));

            query = query.OrderByDescending(x => x.Id);
            var result = OnLineBusinessRepository.Instance.FindForPaging(size, index, query, out total).ToList();
            return result;
        }

        /// <summary>
        /// 获取在线业务
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public OnLineBusiness GetById(long id)
        {
            var entity = OnLineBusinessRepository.Instance.Find(x => x.Id == id).FirstOrDefault();
            return entity;
        }

        /// <summary>
        /// 受理在线业务
        /// </summary>
        /// <param name="id"></param>
        /// <param name="projectno"></param>
        /// <param name="reportno"></param>
        /// <returns></returns>
        public void Accept(long id, long projectId)
        {
            LogHelper.Ilog("Accept?id=" + id + "&projectId=" + projectId, "受理在线业务-" + Instance.ToString());
            OnLineBusiness business = OnLineBusinessRepository.Instance.Find(x => x.Id == id).FirstOrDefault();
            var project = ProjectRepository.Instance.Find(x => x.Id == projectId).FirstOrDefault();
            if (business == null)
                throw new ServiceException("该在线业务不存在");
            if (project == null)
                throw new ServiceException("该立项不存在");
            OnLineBusinessRepository.Instance.Transaction(() =>
            {
                business.ProjectId = project.Id;
                business.IsAccept = true;
                OnLineBusinessRepository.Instance.Save(business);

                //将线相关附件关联当前项目
                var projectDoc = ProjectDocumentRepository.Instance.Find(x => x.OnLineBussinessId == id).ToList();
                if (projectDoc != null && projectDoc.Count > 0)
                {
                    foreach (var doc in projectDoc)
                    {
                        doc.ProjectId = projectId;
                        ProjectDocumentRepository.Instance.Save(doc);
                    }
                }
                //将相关看房联系人关联当前项目
                //var contact = ExplorationContactsRepository.Instance.Find(x => x.OnlineBusinessId == id).ToList();
                //if (contact != null && contact.Count > 0)
                //{
                //    foreach (var ct in contact)
                //    {
                //        ct.ProjectId = projectId;
                //        ExplorationContactsRepository.Instance.Save(ct);
                //    }
                //}

                //将相关银行关联当前项目
                var banks = OnLineBusinessBankRepository.Instance.Find(x => x.OnBusinessId == id).ToList();
                if (banks != null && banks.Count > 0)
                {
                    foreach (var bk in banks)
                    {
                        bk.ProjectId = projectId;
                        OnLineBusinessBankRepository.Instance.Save(bk);
                    }
                }
                //委托进度查询(受理评估)
                WriteFeedBackKafkaMQ(projectId, ProgressEnum.受理评估);
            });

        }

        /// <summary>
        /// 不受理在线业务
        /// </summary>
        /// <param name="tid"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public void Refuse(long id, string reason)
        {
            LogHelper.Ilog("Refuse?id=" + id + "&reason=" + reason, "不受理在线业务-" + Instance.ToString());
            var business = OnLineBusinessRepository.Instance.Find(x => x.Id == id).FirstOrDefault();
            if (business == null)
                throw new ServiceException("该在线业务不存在");
            business.IsAccept = false;
            business.RefusedReason = reason;
            OnLineBusinessRepository.Instance.Save(business);
            KafkaMQService.Instance.SendMessage(new CallBackMessageEntity
            {
                onLineBusinessID = business.Id.ToString(),
                businessId = business.TransactionNo,
                businessType = ProgressEnum.受理评估.ToString(),
                bussinessForm = new BussinessForm
                {
                    message = "拒绝受理,原因：" + reason,
                    success = false
                }
            });
        }

        /// <summary>
        /// 保存在线人工询价（线上业务）
        /// </summary>
        /// <returns></returns>
        public void SaveOnlineInquiryByKafkaMq(OnlineInquiryApiModel Model, string BusinessId, string DataSource,
            BussinessType businessType)
        {
            string ErrorMsg = string.Empty;
            if (!ValidateModel.ValidateEntityData<OnlineInquiryApiModel>(Model, out ErrorMsg))
            {
                KafkaMQService.Instance.SendMessage(new CallBackMessageEntity
                {
                    onLineBusinessID = "",
                    businessId = BusinessId,
                    businessType = businessType.ToString(),
                    bussinessForm = new BussinessForm
                    {
                        message = ErrorMsg,
                        success = false
                    }
                });
                return;
            }
            OnLineBusiness entity = new OnLineBusiness();
            entity.TransactionNo = BusinessId;
            entity.DataSource = DataSource;
            entity.City = Model.cityName;
            entity.DepartmentId = Model.evaluationCompanyId;
            entity.Region = Model.region;
            entity.Community = Model.residentialAreaName;
            entity.FloorBuilding = Model.floorBuilding;
            entity.Orientation = Model.toward;
            entity.Area = Model.area;
            entity.PropertysType = Model.roomType;
            entity.PlanUse = Model.planUse;
            entity.Floor = Model.floor;
            entity.TotalFloor = Model.totalFloor;
            entity.BuildYear = Model.buildYear;
            entity.DecorateCase = Model.decorateCase;
            entity.Remark = Model.comment;
            entity.Assessment = Model.expectPrice;
            entity.HighLowAssess = Model.highLowAssess;
            entity.DelegateType = OnLineBusiness.DelegateTypeEnum.人工询价;
            LogHelper.Ilog("SaveOnlineInquiryByKafkaMq?OnLineBusiness=" + entity.ToJson(), "线上业务保存在线人工询价-" + Instance.ToString());
            if (Model.bank.Count > 0)
            {
                entity.OnLineBusinessBanks =
                    Model.bank.Select(
                        x => new OnLineBusinessBank { BankName = x.bankName, BankbranchName = x.bankbranchName })
                        .ToList();
            }
            if (!OnLineBusinessRepository.Instance.Insert(entity))
                KafkaMQService.Instance.SendMessage(new CallBackMessageEntity
                {
                    onLineBusinessID = entity.Id.ToString(),
                    businessId = BusinessId,
                    businessType = businessType.ToString(),
                    bussinessForm = new BussinessForm
                    {
                        message = "委托失败",
                        success = false
                    }
                });
        }


        ///根据businessId获取报告下载的URL
        ///作者：BOBO
        ///时间：2016-4-27
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        public IDictionary<string, object> GetOnlineReportDownloadUrl(string businessId)
        {
            LogHelper.logerror.Error("接口调用:获取报告下载Url,传递参数为:" + businessId);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (string.IsNullOrEmpty(businessId))
            {
                dic.Add("code", "0");
                dic.Add("msg", "交易编号不能为空!");
                dic.Add("data", "");
                return dic;
            }
            //long lbusinessId = long.Parse(businessId);
            var curResource = from pr in ProjectRepository.Instance.Source
                              join online in OnLineBusinessRepository.Instance.Source on pr.Id equals online.ProjectId
                              join prs in ProjectResourceRepository.Instance.Source on pr.Id equals prs.ProjectId
                              where online.TransactionNo == businessId && prs.ResourceType == 1
                              select new { prs = prs, online = online };
            //var OnlineBusiness = OnLineBusinessRepository.Instance.Find(x => x.TransactionNo == businessId).FirstOrDefault();
            var result = curResource.FirstOrDefault();
            if (result == null || result.prs == null)
            {
                dic.Add("code", "0");
                dic.Add("msg", "报告资源不存在!");
                dic.Add("data", "");
                return dic; ;
            }
            //获取资源库路径
            string reportFilePath = SingleFileManager.GetFileUrl(result.prs.ResourceId) + "";
            if (string.IsNullOrEmpty(reportFilePath))
            {
                dic.Add("code", "0");
                dic.Add("msg", "报告资源不存在!");
                dic.Add("data", "");
                return dic;
            }
            var data = new
            {
                evaluationCompanyId = result.online.DepartmentId,
                bank = result.online.OnLineBusinessBanks.Select(x => new BankInfo { bankName = x.BankName, bankbranchName = x.BankbranchName }).ToList(),
                cityName = result.online.City,
                region = result.online.Region,
                residentialAreaName = result.online.Community,
                area = result.online.Area,
                roomType = result.online.HouserOperties,
                toward = result.online.Orientation,
                floor = result.online.Floor,
                totalFloor = result.online.Floor,
                floorBuilding = result.online.FloorBuilding,
                unitName = result.online.UnitName,
                houseNo = result.online.HouseNo,
                doorModelHall = result.online.DoorModelHall,
                doorModelWash = result.online.DoorModelWash,
                decorateCase = result.online.DecorateCase,
                highLowAssess = result.online.HouseAddress,
                sellerName = result.online.SellerName,
                sellerPhone = result.online.SellerPhone,
                sellerCardType = result.online.SellerCardType,
                sellerID = result.online.SellerID,
                buildYear = result.online.BuildYear,
                expectPrice = result.online.Assessment,
                recipientName = result.online.ReportReceive,
                recipientTelephone = result.online.ReportReceivePhone,
                recipientAddress = result.online.PostAddress,
                loanType = result.online.LoanType,
                loanRatio = result.online.LoanRatio,
                reportNum = result.online.ReportQuantity,
                comment = result.online.Remark,
                planUse = result.online.PlanUse,
                houseType = result.online.PropertysType,
                houseNature = result.online.HouserOperties,
                address = result.online.HouseAddress,
                dealuserName = result.online.DealuserName,
                dealuserMobile = result.online.DealuserMobile,
                delegateName = result.online.DelegateName,
                delegateCardType = result.online.DelegateCardType,
                delegateCardID = result.online.DelegateCardId,
                houseContact = result.online.ExplorationContacts.Select(x => new houseContact { houseContactName = x.Contacts, houseContactTelephone = x.Phone }).ToList(),
                realMoney = result.online.RealMoney,
                contractMoney = result.online.ContractMoney,
                reportDownloadUrl = reportFilePath
            };
            dic.Add("code", "1");
            dic.Add("msg", "请求成功!");
            dic.Add("data", data);
            return dic;
        }

        /// <summary>
        /// 根据报告编号和委托人获取报告下载的URL
        /// 作者：BOBO
        /// 时间：2016-4-27
        /// </summary>
        /// <param name="reportNo">报告编号</param>
        /// <param name="delegateName">委托人姓名</param>
        /// <returns></returns>
        public IDictionary<string, object> GetReportQueryDownloadUrl(string reportNo, string delegateName)
        {
            IDictionary<string, object> dic = new Dictionary<string, object>();
            //var summaryData = SummaryDataRepository.Instance.Find(x => x.ReportNo == reportNo && x.EvalEntrust == delegateName).FirstOrDefault();
            var summaryData = (from project in ProjectRepository.Instance.Source
                               join summary in SummaryDataRepository.Instance.Source
                               on project.Id equals summary.Project.Id
                               where project.ReportNo == reportNo && summary.EvalEntrust == delegateName
                               select summary).FirstOrDefault();
            if (summaryData == null)
            {
                dic.Add("code", "0");
                dic.Add("msg", "记录不存在!");
                dic.Add("data", "");
                return dic;
            }
            var projectResource = ProjectResourceRepository.Instance.Find(x => x.ProjectId == summaryData.Project.Id).FirstOrDefault();
            if (projectResource == null)
            {
                dic.Add("code", "0");
                dic.Add("msg", "报告不存在!");
                dic.Add("data", "");
                return dic;
            }
            //获取资源库路径
            string reportFilePath = SingleFileManager.GetFileUrl(projectResource.ResourceId);
            if (string.IsNullOrEmpty(reportFilePath))
            {
                dic.Add("code", "0");
                dic.Add("msg", "报告资源不存在!");
                dic.Add("data", "");
                return dic;
            }
            var data = new
            {
                evaluationCompanyId = summaryData.Project.DepartmentId,
                bank = new string[] { summaryData.Project.CreditBank, summaryData.Project.CreditSubbranch },
                cityName = summaryData.Project.City,
                region = summaryData.Project.District,
                residentialAreaName = summaryData.Project.ResidentialAreaName,
                area = summaryData.Project.BuildingArea,
                roomType = summaryData.HouseType,
                toward = "",
                floor = summaryData.Floor,
                totalFloor = summaryData.MaxFloor,
                floorBuilding = "",
                unitName = "",
                houseNo = "",
                doorModelHall = "",
                doorModelWash = "",
                decorateCase = summaryData.Decoration,
                highLowAssess = "",
                sellerName = "",
                sellerPhone = "",
                sellerCardType = "",
                sellerID = "",
                buildYear = summaryData.BuiltYear,
                expectPrice = "",
                recipientName = "",
                recipientTelephone = "",
                recipientAddress = "",
                loanType = "",
                loanRatio = "",
                reportNum = "",
                comment = summaryData.Project.Remark,
                planUse = "",
                houseType = summaryData.Project.PropertyType,
                houseNature = "",
                address = "",
                dealuserName = "",
                dealuserMobile = "",
                delegateName = "",
                delegateCardType = "",
                delegateCardID = "",
                houseContactName = "",
                houseContactTelephone = "",
                realMoney = "",
                contractMoney = "",
                reportDownloadUrl = reportFilePath
            };

            dic.Add("code", "1");
            dic.Add("msg", "请求成功!");
            dic.Add("data", data);
            return dic;
        }


        /// <summary>
        /// 委托进度查询
        /// 作者：BOBO
        /// 时间：2016-4-28
        /// </summary>
        /// <returns></returns>
        public void WriteFeedBackKafkaMQ(long projectId, ProgressEnum progressEnum)
        {
            LogHelper.Ilog("WriteFeedBackKafkaMQ?projectId=" + projectId + "&progressEnum=" + progressEnum, "委托进度查询-" + Instance.ToString());
            if (projectId <= 0)
                return;
            var query = from pr in ProjectRepository.Instance.Source
                        join ob in OnLineBusinessRepository.Instance.Source on pr.Id equals ob.ProjectId
                        where pr.Id == projectId
                        select ob;
            var result = query.ToList().FirstOrDefault();
            if (result == null)
                return;

            object data = null;
            var summaryData = SummaryDataRepository.Instance.Find(x => x.Project.Id == projectId).FirstOrDefault();

            switch (progressEnum)
            {
                case ProgressEnum.报告完成:
                    FinishReport fReport = new FinishReport();
                    fReport.completeTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    fReport.evaluatePrice = summaryData.EvaluatePrice.Equals(0) ? "0.1" : summaryData.EvaluatePrice.ToString();
                    fReport.evaluateValue = summaryData.EvaluateTotal.Equals(0) ? "0.1" : summaryData.EvaluateTotal.ToString();//防止未填评估总价过不了搜房接口
                    fReport.quantityCompany = summaryData.Company;
                    fReport.surplusTerm = summaryData.LandSpareYear.ToString();
                    var curProjectResource = ProjectResourceRepository.Instance.Find(x => x.ProjectId == projectId).FirstOrDefault();
                    //获取资源库路径
                    string reportFilePath = curProjectResource == null ? "" : SingleFileManager.GetFileUrl(curProjectResource.ResourceId);
                    fReport.reportUrl = reportFilePath;
                    fReport.processName = progressEnum.ToString();
                    data = fReport;
                    break;
                case ProgressEnum.递送纸质报告:
                    var reportSend = ReportSendRepository.Instance.Find(x => x.ProjectId == summaryData.Project.Id).FirstOrDefault();
                    DeliveryReport dReport = new DeliveryReport();
                    dReport.reportNo = summaryData.ReportNo;
                    dReport.sendTime = (reportSend == null ? DateTime.Now : reportSend.CreateTime).ToString("yyyy-MM-dd HH:mm:ss");
                    dReport.sender = CookieHelper.GetUserAccount();
                    dReport.sendType = reportSend == null ? "" : reportSend.SendType;
                    dReport.receiver = reportSend == null ? "" : reportSend.Receiver;
                    dReport.sendExpress = reportSend == null ? "" : reportSend.SendExpress;
                    dReport.receiverPhone = reportSend == null ? "" : reportSend.ReciverMobile;
                    dReport.expressNo = reportSend == null ? "" : reportSend.ExpressNo;
                    dReport.processName = progressEnum.ToString();
                    data = dReport;
                    break;
                //case ProgressEnum.实勘完成:
                //    RealProspecting rProspec = new RealProspecting();
                //    rProspec.reconnaissanceTime = summaryData.SurveyTime.HasValue ? summaryData.SurveyTime.Value : DateTime.Now;
                //    rProspec.reconnaissanceStatus = true;
                //    rProspec.reconnaissancePerson = summaryData.SurveyPeople;
                //    rProspec.comment = "";
                //    data = rProspec;
                //    break;
                case ProgressEnum.受理评估:
                    Acceptance acceptance = new Acceptance();
                    acceptance.acceptTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    acceptance.comment = summaryData.Project.Remark;
                    acceptance.status = true;
                    acceptance.preeValue = summaryData.Project.InquiryResult.ToString();
                    acceptance.processName = progressEnum.ToString();
                    data = acceptance;
                    break;
                //case ProgressEnum.预约看房:
                //    BookingRoom bookingRoom = new BookingRoom();
                //    bookingRoom.comment = "";
                //    bookingRoom.invitewatchPerson = "";
                //    bookingRoom.invitewatchPhone = "";
                //    bookingRoom.invitewatchStatus = true;
                //    bookingRoom.invitewatchTime = DateTime.Now;
                //    data = bookingRoom;
                //    break;
                case ProgressEnum.制作报告:
                    ProduReport produReport = new ProduReport();
                    produReport.begingDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    produReport.creator = CookieHelper.GetUserAccount();
                    produReport.processName = progressEnum.ToString();
                    break;
                default:
                    data = new object();
                    break;
            }
            KafkaMQService.Instance.SendMessage(new CallBackMessageEntity
            {
                businessType = BussinessType.进度反馈.ToString(),
                businessId = result.TransactionNo,
                onLineBusinessID = result.Id.ToString(),
                bussinessForm = new BussinessForm
                {
                    success = true,
                    data = data
                }
            });
        }

        /// <summary>
        /// 外采消息推送时特殊处理
        /// </summary>
        /// <param name="ProjectNo">项目流水号</param>
        /// <param name="progressEnum">业务类型</param>
        /// <param name="ObjectJson"></param>
        public void WriteFeedBackKafkaMQ(string ProjectNo, BussinessType bussinessType, string ObjectJson)
        {
            var project = ProjectRepository.Instance.Find(x => x.ProjectNo == ProjectNo).FirstOrDefault();
            if (project == null)
            {
                LogManager.GetLogger("LogExceptionRabbitMQReceive").Error(string.Format("外采消息推送时特殊处理失败，不项目存在！流水号:{0},业务类型:{1},消息内容:{2}", ProjectNo, bussinessType.ToString(), ObjectJson));
                return;
            }
            var OnliBusiness = OnLineBusinessRepository.Instance.Find(x => x.ProjectId == project.Id).FirstOrDefault();
            if (OnliBusiness == null)
            {
                LogManager.GetLogger("LogExceptionRabbitMQReceive").Error(string.Format("外采消息推送时特殊处理失败，线上业务存在！流水号:{0},业务类型:{1},消息内容:{2}", ProjectNo, bussinessType.ToString(), ObjectJson));
                return;
            }
            object data = null;
            if (bussinessType == BussinessType.实勘完成)
            {
                RealProspecting realProspecting = JsonConvert.DeserializeObject<RealProspecting>(ObjectJson);
                realProspecting.processName = ProgressEnum.实勘完成.ToString();
                data = realProspecting;
            }
            else
            {
                BookingRoom bookingRoom = JsonConvert.DeserializeObject<BookingRoom>(ObjectJson);
                bookingRoom.processName = ProgressEnum.预约看房.ToString();
                data = bookingRoom;
            }
            KafkaMQService.Instance.SendMessage(new CallBackMessageEntity
            {
                businessType = BussinessType.进度反馈.ToString(),
                businessId = OnliBusiness.TransactionNo,
                onLineBusinessID = OnliBusiness.Id.ToString(),
                bussinessForm = new BussinessForm
                {
                    success = true,
                    data = data
                }
            });
        }

        /// <summary>
        /// 在线业务终止
        /// </summary>
        /// <returns></returns>
        public void StopOnlineBusinessByKafkaMq(OnlineBusinessStopApiModel Model, string businessId)
        {
            LogHelper.Ilog("StopOnlineBusinessByKafkaMq?OnlineBusinessStopApiModel=" + Model.ToJson() + "&businessId=" + businessId, "在线业务终止-" + Instance.ToString());
            string ErrorMsg = string.Empty;
            if (!ValidateModel.ValidateEntityData<OnlineBusinessStopApiModel>(Model, out ErrorMsg))
            {
                KafkaMQService.Instance.SendMessage(new CallBackMessageEntity
                {
                    onLineBusinessID = "",
                    businessId = businessId,
                    businessType = BussinessType.业务终止.ToString(),
                    bussinessForm = new BussinessForm
                    {
                        message = ErrorMsg,
                        success = false
                    }
                });
                return;
            }
            var onlineBusiness = OnLineBusinessRepository.Instance.Source.FirstOrDefault(x => x.TransactionNo == businessId);
            if (onlineBusiness == null)
            {
                KafkaMQService.Instance.SendMessage(new CallBackMessageEntity
                {
                    onLineBusinessID = "",
                    businessId = businessId,
                    businessType = BussinessType.业务终止.ToString(),
                    bussinessForm = new BussinessForm
                    {
                        message = "不存在该在线业务，取消失败！",
                        success = false
                    }
                });
                return;
            }
            var entity = new OnlineBusinessStop();
            entity.OnlineBusinessId = onlineBusiness.Id;
            entity.Reason = Model.reason;
            entity.StopTime = DateTime.Now;
            if (OnlineBusinessStopRepository.Instance.Insert(entity))
                KafkaMQService.Instance.SendMessage(new CallBackMessageEntity
                {
                    onLineBusinessID = onlineBusiness.Id.ToString(),
                    businessId = businessId,
                    businessType = BussinessType.业务终止.ToString(),
                    bussinessForm = new BussinessForm
                    {
                        message = "终止成功!",
                        success = true
                    }
                });
        }

    }

    /// <summary>
    /// 委托进度查询
    /// </summary>
    public enum ProgressEnum
    {
        受理评估 = 1,
        预约看房 = 2,
        实勘完成 = 3,
        制作报告 = 4,
        报告完成 = 5,
        递送纸质报告 = 6
    }

    public class OnlineApiReturnData<T> where T : class
    {
        public string code { get; set; }

        public string msg { get; set; }

        public T data { get; set; }
    }

    /// <summary>
    /// 受理评估
    /// </summary>
    public class Acceptance
    {
        /// <summary>
        /// 进度名称
        /// </summary>
        public string processName { get; set; }
        /// <summary>
        ///受理时间
        /// </summary>
        public string acceptTime { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string comment { get; set; }

        /// <summary>
        /// 预期评估价
        /// </summary>
        public string preeValue { get; set; }

        /// <summary>
        /// 受理是否成功
        /// </summary>
        public bool status { get; set; }
    }

    /// <summary>
    /// 预约看房
    /// </summary>
    public class BookingRoom
    {
        /// <summary>
        /// 进度名称
        /// </summary>
        public string processName { get; set; }
        /// <summary>
        /// 约看时间
        /// </summary>
        public string invitewatchTime { get; set; }

        /// <summary>
        /// 约看是否成功
        /// </summary>
        public bool invitewatchStatus { get; set; }

        /// <summary>
        /// 约看人
        /// </summary>
        public string invitewatchPerson { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string comment { get; set; }

        /// <summary>
        /// 约看人电话
        /// </summary>
        public string invitewatchPhone { get; set; }
    }

    /// <summary>
    /// 实勘完成
    /// </summary>
    public class RealProspecting
    {
        /// <summary>
        /// 进度名称
        /// </summary>
        public string processName { get; set; }
        /// <summary>
        /// 实勘时间
        /// </summary>
        public string reconnaissanceTime { get; set; }

        /// <summary>
        /// 实勘人
        /// </summary>
        public string reconnaissancePerson { get; set; }

        /// <summary>
        /// 是否勘察成功
        /// </summary>
        public bool reconnaissanceStatus { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string comment { get; set; }
    }

    /// <summary>
    /// 制作报告
    /// </summary>
    public class ProduReport
    {
        /// <summary>
        /// 进度名称
        /// </summary>
        public string processName { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string begingDate { get; set; }

        /// <summary>
        /// 制作人
        /// </summary>
        public string creator { get; set; }
    }

    /// <summary>
    /// 报告完成
    /// </summary>
    public class FinishReport
    {
        /// <summary>
        /// 进度名称
        /// </summary>
        public string processName { get; set; }
        /// <summary>
        /// 完成时间
        /// </summary>
        public string completeTime { get; set; }

        /// <summary>
        /// 估价总价
        /// </summary>
        public string evaluateValue { get; set; }

        /// <summary>
        /// 估价单价
        /// </summary>
        public string evaluatePrice { get; set; }

        /// <summary>
        /// 报告地址
        /// </summary>
        public string reportUrl { get; set; }

        /// <summary>
        /// 剩余使用年限
        /// </summary>
        public string surplusTerm { get; set; }

        /// <summary>
        /// 评估公司
        /// </summary>
        public string quantityCompany { get; set; }
    }

    /// <summary>
    /// 递送纸质报告
    /// </summary>
    public class DeliveryReport
    {
        /// <summary>
        /// 进度名称
        /// </summary>
        public string processName { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public string sendTime { get; set; }

        /// <summary>
        /// 报告编号
        /// </summary>
        public string reportNo { get; set; }

        /// <summary>
        /// 发送方式
        /// </summary>
        public string sendType { get; set; }

        /// <summary>
        /// 发送人
        /// </summary>
        public string sender { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>
        public string expressNo { get; set; }

        /// <summary>
        /// 接收人
        /// </summary>
        public string receiver { get; set; }

        /// <summary>
        /// 接收人电话
        /// </summary>
        public string receiverPhone { get; set; }
        /// <summary>
        /// 快递公司
        /// </summary>
        public string sendExpress { get; set; }

    }
}
