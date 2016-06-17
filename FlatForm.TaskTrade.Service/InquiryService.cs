using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Peacock.Common.Exceptions;
using Peacock.PEP.Data.Entities;
using Peacock.PEP.Model.ApiModel;
using Peacock.PEP.Repository.Repositories;
using Peacock.PEP.Service.Base;
using Peacock.PEP.Model.Condition;
using Peacock.Common.Helper;
using Peacock.PEP.Service.Extensions;
using Peacock.PMS.Service.Services;

namespace Peacock.PEP.Service
{
    public class InquiryService:SingModel<InquiryService>
    {
        private InquiryService()
        {
             
        }

        /// <summary>
        /// 获取询价信息列表
        /// </summary>
        /// <param name="condition">询价信息查询条件</param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public IList<Inquiry> GetInquiryList(InquiryCondition condition, int index, int size, out int total)
        {
            LogHelper.Ilog("GetInquiryList?condition=" + condition.ToJson() + "&index=" + index + "&size=" + size, "询价记录-" + Instance.ToString());
            var query = InquiryRepository.Instance.Source;

            //按询价数据权限查询
            query = query.FindByPower();

            //根据查询条件
            if (!string.IsNullOrEmpty(condition.ResidentialAreaName))
            {
                query = query.Where(x => x.ResidentialAreaName.Contains(condition.ResidentialAreaName));
            }
            if (!string.IsNullOrEmpty(condition.Address))
            {
                query = query.Where(x => x.Address.Contains(condition.Address));
            } 
            if (!string.IsNullOrEmpty(condition.CustomerName))
            {
                query = query.Where(x => x.Customer.CustomerName.Contains(condition.CustomerName));
            }
            if (!string.IsNullOrEmpty(condition.Bank))
            {
                query = query.Where(x => x.Customer.Bank.Contains(condition.Bank));
            }
            if (!string.IsNullOrEmpty(condition.City))
            {
                query = query.Where(x => x.City.Contains(condition.City));
            }
            if (condition.IsToProject.HasValue)
            {
                query = query.Where(x => x.IsToProject == condition.IsToProject.Value);
            }

            query = query.OrderByDescending(x => x.Id);
            return InquiryRepository.Instance.FindForPaging(size, index, query, out total).ToList();
        }

        public List<string> GetRegion(string cityName)
        {
            var city = RegionRepository.Instance.Find(x => x.Name == cityName && x.RegionType == "city").FirstOrDefault();
            if (city == null)
                return null;
            return RegionRepository.Instance.Find(x => x.ParentId == city.Id).Select(y => y.Name).ToList();
        }

        public string GetCityByRegion(string regionName)
        {
            var region = RegionRepository.Instance.Source.FirstOrDefault(x => x.Name == regionName);
            if (region == null)
                return null;
            var city = RegionRepository.Instance.Source.FirstOrDefault(x => x.Id == region.ParentId);
            if (city == null)
                return null;
            return city.Name;
        }

        public long SaveInquiry(Inquiry inquiry, Customer customer)
        {
            LogHelper.Ilog("SaveInquiry?condition=" + inquiry.ToJson() + "&customer=" + customer.ToJson(), "保存询价-" + Instance.ToString());
            if (string.IsNullOrEmpty(inquiry.ResidentialAreaName))
                throw new ServiceException("小区名称不能为空");
            var oldInquiry = new Inquiry();
            if (inquiry.Id > 0)
            {
                oldInquiry = InquiryRepository.Instance.Source.FirstOrDefault(x => x.Id == inquiry.Id);
                if (oldInquiry != null && oldInquiry.IsToProject)
                    throw new ServiceException("该询价单已立项，无法修改！");
            }
            if (!string.IsNullOrEmpty(customer.CustomerName))
            {
                if (customer.Id <= 0)
                {
                    customer = CustomerRepository.Instance.InsertReturnEntity(customer);
                }
                else
                {
                    var oldCustomer = CustomerRepository.Instance.Source.FirstOrDefault(x => x.Id == customer.Id);
                    if (oldCustomer == null)
                    {
                        customer.Id = 0;
                        customer = CustomerRepository.Instance.InsertReturnEntity(customer);
                    }
                    else
                    {
                        oldCustomer.CustomerName = customer.CustomerName;
                        oldCustomer.Tel = customer.Tel;
                        oldCustomer.Phone = customer.Phone;
                        oldCustomer.Bank = customer.Bank;
                        oldCustomer.Subbranch = customer.Subbranch;
                        oldCustomer.QQ = customer.QQ;
                        CustomerRepository.Instance.Save(oldCustomer);
                    }
                }
            }
            if (inquiry.Id <= 0)
            {
                inquiry.CreateTime = DateTime.Now;
                if (!string.IsNullOrEmpty(customer.CustomerName))
                    inquiry.CustomerId = customer.Id;
                inquiry.CreatorId = UserService.Instance.GetCrmUser().Id;
                inquiry.CreatorName = UserService.Instance.GetCrmUser().UserAccount;
                inquiry.DepartmentId = UserService.Instance.GetUserDepartment().Id;
                return InquiryRepository.Instance.InsertReturnEntity(inquiry).Id;
            }
            else
            {
                if (oldInquiry == null)
                {
                    inquiry.Id = 0;
                    inquiry.CreateTime = DateTime.Now;
                    if (!string.IsNullOrEmpty(customer.CustomerName))
                        inquiry.CustomerId = customer.Id;
                    inquiry.CreatorId = UserService.Instance.GetCrmUser().Id;
                    inquiry.CreatorName = UserService.Instance.GetCrmUser().UserAccount;
                    inquiry.DepartmentId = UserService.Instance.GetUserDepartment().Id;
                    return InquiryRepository.Instance.InsertReturnEntity(inquiry).Id;
                }
                oldInquiry.ResidentialAreaName = inquiry.ResidentialAreaName;
                oldInquiry.ResidentialAddress = inquiry.ResidentialAddress;
                oldInquiry.BuildingArea = inquiry.BuildingArea;
                oldInquiry.City = inquiry.City;
                oldInquiry.District = inquiry.District;
                oldInquiry.BuildedYear = inquiry.BuildedYear;
                oldInquiry.InquiryResult = inquiry.InquiryResult;
                oldInquiry.InquiryPrice = inquiry.InquiryPrice;
                oldInquiry.FeedbackPrice = inquiry.FeedbackPrice;
                oldInquiry.MortgagePrice = inquiry.MortgagePrice;
                oldInquiry.MortgageResult = inquiry.MortgageResult;
                oldInquiry.BuildingName = inquiry.BuildingName;
                oldInquiry.UnitName = inquiry.UnitName;
                oldInquiry.HouseNum = inquiry.HouseNum;
                oldInquiry.PropertyType = inquiry.PropertyType;
                oldInquiry.Floor = inquiry.Floor;
                oldInquiry.MaxFloor = inquiry.MaxFloor;
                oldInquiry.Toword = inquiry.Toword;
                oldInquiry.HouseType = inquiry.HouseType;
                oldInquiry.Decoration = inquiry.Decoration;
                oldInquiry.SpecialInfo = inquiry.SpecialInfo;
                oldInquiry.InquirySource = inquiry.InquirySource;
                oldInquiry.Address = inquiry.Address;
                oldInquiry.FeedbackMessage = inquiry.FeedbackMessage;
                oldInquiry.Remark = inquiry.Remark;
                oldInquiry.IsToProject = inquiry.IsToProject;
                oldInquiry.SpecialRemark = inquiry.SpecialRemark;
                oldInquiry.InquiryNo = inquiry.InquiryNo;
                oldInquiry.AppraiseUse = inquiry.AppraiseUse;
                oldInquiry.AppraiseDelegatePerson = inquiry.AppraiseDelegatePerson;
                oldInquiry.AppraiseObject = inquiry.AppraiseObject;
                oldInquiry.AppraisePurpose = inquiry.AppraisePurpose;
                oldInquiry.AppraiseDate = inquiry.AppraiseDate;
                oldInquiry.HouseStruct = inquiry.HouseStruct;
                oldInquiry.OtherPower = inquiry.OtherPower;
                oldInquiry.OtherPowerDesc = inquiry.OtherPowerDesc;
                oldInquiry.UpperAmount = inquiry.UpperAmount;
                oldInquiry.DisposeFee = inquiry.DisposeFee;
                oldInquiry.LandPremium = inquiry.LandPremium;
                oldInquiry.MinusNetWorth = inquiry.MinusNetWorth;
                oldInquiry.SpecialRemark = inquiry.SpecialRemark;
                oldInquiry.IssueDate = inquiry.IssueDate;
                oldInquiry.ValidityDate = inquiry.ValidityDate;
                oldInquiry.AppraiseOrg = inquiry.AppraiseOrg;
                oldInquiry.SetPurpose = inquiry.SetPurpose; 
                if (!string.IsNullOrEmpty(customer.CustomerName))
                    oldInquiry.CustomerId = customer.Id;
                else
                    oldInquiry.CustomerId = null;
                oldInquiry.CreatorId = UserService.Instance.GetCrmUser().Id;
                oldInquiry.CreatorName = UserService.Instance.GetCrmUser().UserAccount;
                oldInquiry.DepartmentId = UserService.Instance.GetUserDepartment().Id;
                oldInquiry.SetOther = inquiry.SetOther;
                oldInquiry.IsHouseRecord = inquiry.IsHouseRecord;
                oldInquiry.AppraisePurposeDesc = inquiry.AppraisePurposeDesc;
                oldInquiry.EffectivePeriod = inquiry.EffectivePeriod;
                oldInquiry.IsMortgage = inquiry.IsMortgage;
                oldInquiry.MortgageDesc = inquiry.MortgageDesc;
                
                InquiryRepository.Instance.Save(oldInquiry);
                return oldInquiry.Id;
            }
        }

        public Inquiry GetInquiry(long inquiryId)
        {
            return InquiryRepository.Instance.Source.FirstOrDefault(x => x.Id == inquiryId);
        }

        public bool ToProject(long inquiryId,long projectId)
        {
            var inquiry = InquiryRepository.Instance.Source.FirstOrDefault(x => x.Id == inquiryId);
            var project = ProjectRepository.Instance.Source.FirstOrDefault(x => x.Id == projectId);
            if (inquiry != null && project != null && inquiry.IsToProject == false)
            {
                inquiry.IsToProject = true;
                return InquiryRepository.Instance.Save(inquiry);
            }
            return false;
        }

        /// <summary>
        /// 线上业务转立项成功
        /// </summary>
        /// <returns></returns>
        public void SaveInquiryFromOnline(long onlineBusinessId, long inquiryId)
        {
            LogHelper.Ilog("SaveInquiryFromOnline?onlineBusinessId=" + onlineBusinessId + "&inquiryId="+ inquiryId, "线上业务转立项成功-" + Instance.ToString());
            var inquiry = InquiryRepository.Instance.Source.FirstOrDefault(x => x.Id == inquiryId);
            var onlineBusiness = OnLineBusinessRepository.Instance.Source.FirstOrDefault(x => x.Id == onlineBusinessId);
            if (inquiry != null && onlineBusiness != null && !onlineBusiness.IsInquiry)
            {
                onlineBusiness.IsInquiry = true;
                OnLineBusinessRepository.Instance.Save(onlineBusiness);
                var entity = new InquiryResultApiModel();
                entity.residentialAreaName = inquiry.ResidentialAreaName;
                entity.residentialAddress = inquiry.ResidentialAddress;
                entity.ruildingArea = inquiry.BuildingArea.HasValue ? inquiry.BuildingArea.ToString() : null;
                entity.ruildedYear = inquiry.BuildedYear.HasValue ? inquiry.BuildedYear.ToString() : null;
                entity.inquiryResult = inquiry.InquiryResult.HasValue ? inquiry.InquiryResult.ToString() : null;
                entity.inquiryPrice = inquiry.InquiryPrice.HasValue ? inquiry.InquiryPrice.ToString() : null;
                entity.mortgagePrice = inquiry.MortgagePrice.HasValue ? inquiry.MortgagePrice.ToString() : null;
                entity.mortgageResult = inquiry.MortgageResult.HasValue ? inquiry.MortgageResult.ToString() : null;
                entity.specialInfo = inquiry.SpecialInfo;
                KafkaMQService.Instance.SendMessage(new CallBackMessageEntity
                {
                    onLineBusinessID = onlineBusiness.Id.ToString(),
                    businessId = onlineBusiness.TransactionNo,
                    businessType = BussinessType.人工询价.ToString(),
                    bussinessForm = new BussinessForm
                    {
                        data = entity,
                        success = true
                    }
                });
                return;
            }
            if (onlineBusiness != null)
            {
                KafkaMQService.Instance.SendMessage(new CallBackMessageEntity
                {

                    onLineBusinessID = onlineBusiness.Id.ToString(),
                    businessId = onlineBusiness.TransactionNo,
                    businessType = BussinessType.人工询价.ToString(),
                    bussinessForm = new BussinessForm
                    {
                        message="询价成功",
                        success = true
                    }
                });
                return;
            }
        }
    }
}
