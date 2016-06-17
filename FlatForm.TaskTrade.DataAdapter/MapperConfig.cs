using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Peacock.PEP.Data;
using System.Linq.Expressions;
using Peacock.PEP.Data.Entities;
using Peacock.PEP.Model;
using Peacock.PEP.Model.DTO;
using Peacock.PEP.Service;

namespace Peacock.PEP.DataAdapter
{
    /// <summary>
    /// Model和Entity之间的转换
    /// ConditionQuery和Condition之间的转换
    /// </summary>
    public static class MapperConfig
    {
        static MapperConfig()
        {
            Init();
        }

        public static void Init()
        {
            AutoMapper.Mapper.CreateMap<OnLineBusiness, OnLineBusinessModel>()
                .ForMember(x => x.BankListStr, opt => opt.MapFrom(src=>OnlineBusinessService.Instance.GetBankListByOnlineBusinessId(src.Id)))
                .ForMember(x => x.SeeHouseLinker, opt => opt.MapFrom(src => ExplorationContactsService.Instance.GetListStrByBusinessId(src.Id)));
            AutoMapper.Mapper.CreateMap<OnLineFeedBackModel, OnLineFeedBack>();
            AutoMapper.Mapper.CreateMap<ProjectModel, Project>().ForMember(x => x.IsSubmitted, opt =>
                opt.MapFrom(
                    src => src.IsSubmitted == "true" ? true : false));
            AutoMapper.Mapper.CreateMap<OnLineFeedBack, OnLineFeedBackModel>();
            AutoMapper.Mapper.CreateMap<Project, ProjectModel>()
                .ForMember(x => x.CustomerId, x => x.MapFrom(scr => scr.Customer.Id))
                .ForMember(x => x.CustomerName, x => x.MapFrom(scr => scr.Customer.CustomerName))
                .ForMember(x => x.CustomerTel, x => x.MapFrom(scr => scr.Customer.Tel))
                .ForMember(x => x.CustomerPhone, x => x.MapFrom(scr => scr.Customer.Phone))
                .ForMember(x => x.CustomerBank, x => x.MapFrom(scr => scr.Customer.Bank))
                .ForMember(x => x.CustomerSubbranch, x => x.MapFrom(scr => scr.Customer.Subbranch))
                .ForMember(x => x.CustomerQQ, x => x.MapFrom(scr => scr.Customer.QQ))
                .ForMember(x => x.CreateTime, opt =>
                        opt.MapFrom(
                            src => src.CreateTime == null ? null : src.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(x => x.IsSubmitted, opt =>
                        opt.MapFrom(
                            src => src.IsSubmitted ? "是" : "否"));
            Mapper.CreateMap<ReportSend, ReportSendDto>();
            Mapper.CreateMap<ReportSendDto, ReportSend>();
            Mapper.CreateMap<Customer, CustomerModel>()
                .ForMember(x => x.CustomerId, opt => opt.MapFrom(src => src.Id));
            Mapper.CreateMap<CustomerModel, Customer>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.CustomerId));
            Mapper.CreateMap<Company, CompanyModel>();
            Mapper.CreateMap<CompanyModel, Company>();
            Mapper.CreateMap<ExplorationContactsModel, ExplorationContacts>();
            Mapper.CreateMap<ProjectResource, ProjectResourceModel>();
            Mapper.CreateMap<ProjectResourceModel, ProjectResource>();
            Mapper.CreateMap<ProjectDocument, ProjectDocumentModel>();
            Mapper.CreateMap<ProjectStateInfo, ProjectStateInfoModel>();
            Mapper.CreateMap<ExplorationContacts, ExplorationContactsModel>();
            Mapper.CreateMap<Inquiry, InquiryModel>()
                .ForMember(x => x.CreateTime, opt => opt.MapFrom(src => src.CreateTime == null ? null : src.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")))
                .ForMember(x => x.AppraiseDate, opt => opt.MapFrom(src => src.AppraiseDate == null ? null : src.AppraiseDate.Value.ToString("yyyy-MM-dd")))
                .ForMember(x => x.IssueDate, opt => opt.MapFrom(src => src.IssueDate == null ? null : src.IssueDate.Value.ToString("yyyy-MM-dd")))
                .ForMember(x => x.ValidityDate, opt => opt.MapFrom(src => src.ValidityDate == null ? null : src.ValidityDate.Value.ToString("yyyy-MM-dd")));
                //.ForMember(x => x.OtherPower, opt => opt.MapFrom(src => src.OtherPower ? "true" : "false"));
            Mapper.CreateMap<InquiryModel, Inquiry>()
                .ForMember(x => x.CreateTime, opt => opt.MapFrom(src => src.CreateTime == null ? DateTime.Now : DateTime.Parse(src.CreateTime)))
                .ForMember(x => x.AppraiseDate, opt => opt.MapFrom(src => src.AppraiseDate == null ? DateTime.Now : DateTime.Parse(src.AppraiseDate)))
                .ForMember(x => x.IssueDate, opt => opt.MapFrom(src => src.IssueDate == null ? DateTime.Now : DateTime.Parse(src.IssueDate)))
                .ForMember(x => x.ValidityDate, opt => opt.MapFrom(src => src.ValidityDate == null ? DateTime.Now : DateTime.Parse(src.ValidityDate)));
                //.ForMember(x => x.OtherPower, opt => opt.MapFrom(src => bool.Parse(src.OtherPower)));
            Mapper.CreateMap<OnLineBusiness, OnLineBusinessModel>();
            Mapper.CreateMap<OnLineFeedBackModel, OnLineFeedBack>();
            Mapper.CreateMap<ExplorationContactsModel, ExplorationContacts>();
            Mapper.CreateMap<ProjectModel, Project>();
            Mapper.CreateMap<OnLineFeedBack, OnLineFeedBackModel>();
            Mapper.CreateMap<Project, ProjectModel>();
            Mapper.CreateMap<ProjectResource, ProjectResourceModel>();
            Mapper.CreateMap<ParameterModel, Parameter>();
            Mapper.CreateMap<Parameter, ParameterModel>();
            Mapper.CreateMap<ConfigSolution, ConfigSolutionModel>();
            Mapper.CreateMap<ConfigSolutionModel, ConfigSolution>();
            Mapper.CreateMap<SummaryData, SummaryDataModel>();
            Mapper.CreateMap<SummaryDataModel, SummaryData>();
            Mapper.CreateMap<Region, RegionModel>();
            Mapper.CreateMap<ConfigFunctioncol, ConfigFunctioncolModel>();
            Mapper.CreateMap<ConfigListFunction, ConfigListFunctionModel>();
            Mapper.CreateMap<ReportHistory, ReportHistoryModel>();
            Mapper.CreateMap<ReportHistoryModel, ReportHistory>();
            Mapper.CreateMap<ConfigUserFuncCol, ConfigUserFuncColModel>();
            Mapper.CreateMap<ConfigFunctioncolModel, ConfigFunctioncol>();
            Mapper.CreateMap<ConfigListFunctionModel, ConfigListFunction>();
            Mapper.CreateMap<ConfigUserFuncColModel, ConfigUserFuncCol>();
            Mapper.CreateMap<ProjectCancle, ProjectCancleModel>();
            Mapper.CreateMap<OnLineBusinessBank, OnLineBusinessBankModel>();

            Mapper.CreateMap<Project, ProjectListModel>()
                .ForMember(x => x.SummaryDataId, opt => opt.MapFrom(src => src.SummaryData.Id))
                .ForMember(x => x.IsSubmitted, opt =>  opt.MapFrom( src => src.IsSubmitted ? "是" : "否"))
                .ForMember(x=>x.CustomerName, opt=> opt.MapFrom(src=>src.Customer==null?"":src.Customer.CustomerName))
                .ForMember(x => x.CustomerPhone, opt => opt.MapFrom(src => src.Customer == null ? "" : src.Customer.Phone))
                .ForMember(x=>x.CustomerQQ, opt=> opt.MapFrom(src=>src.Customer==null?"":src.Customer.QQ))
                .ForMember(x=>x.CustomerBank, opt=> opt.MapFrom(src=>src.Customer==null?"":src.Customer.Bank))
                .ForMember(x => x.CustomerSubbranch, opt => opt.MapFrom(src => src.Customer == null ? "" : src.Customer.Subbranch))
                .ForMember(x => x.CustomerTel, opt => opt.MapFrom(src => src.Customer == null ? "" : src.Customer.Tel))
                ;
            Mapper.CreateMap<OnLineBusiness, OnLineBusinessListModel>();
        }

        public static TResult ToModel<TResult>(this object entity)
        {
            return Mapper.Map<TResult>(entity);
        }

        public static List<TResult> ToListModel<TResult, TInput>(this IEnumerable<TInput> list)
        {
            return list.Select(x => x.ToModel<TResult>()).ToList();
        }
    }
}
