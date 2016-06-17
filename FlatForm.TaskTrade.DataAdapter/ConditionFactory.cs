using Peacock.PEP.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Peacock.PEP.DataAdapter.Interface;
using Peacock.PEP.DataAdapter.Implement;

namespace Peacock.PEP.DataAdapter
{
    /// <summary>
    /// 指定接口的实现类型
    /// </summary>
    public class ConditionFactory
    {
        private static readonly ConditionFactory _Instance = new ConditionFactory();
        private Dictionary<Type, object> _items;
        private ConditionFactory()
        {
            _items = new Dictionary<Type, object>();
            Registered<IInquiryAdapter, InquiryAdapter>(new InquiryAdapter());
            Registered<IUserAdapter, UserAdapter>(new UserAdapter());
            Registered<IProjectAdapter, ProjectAdapter>(new ProjectAdapter());
            Registered<IReportSendAdapter, ReportSendAdapter>(new ReportSendAdapter());
            Registered<IProjectResourceAdapter, ProjectResourceAdapter>(new ProjectResourceAdapter());
            Registered<IOnLineBusinessAdapter, OnLineBusinessAdapter>(new OnLineBusinessAdapter());
            Registered<IOnLineFeedBackAdapter, OnLineFeedBackAdapter>(new OnLineFeedBackAdapter());
            //Registered<IGetDataFromAPIAdapter, GetDataFromAPIAdapter>(new GetDataFromAPIAdapter());
            Registered<IParameterAdapter, ParameterAdapter>(new ParameterAdapter());
            Registered<IDtgjAPIAdapter,DtgjAPIAdapter>(new DtgjAPIAdapter());
            Registered<ICompanyAdapter, CompanyAdapter>(new CompanyAdapter());
            Registered<IProjectStateInfoAdapter, ProjectStateInfoAdapter>(new ProjectStateInfoAdapter());
            Registered<IIntegratedQueryAdapter, IntegratedQueryAdapter>(new IntegratedQueryAdapter());
            Registered<IProjectDocumentAdapter, ProjectDocumentAdapter>(new ProjectDocumentAdapter());
            Registered<IExplorationContactsAdapter, ExplorationContactsAdapter>(new ExplorationContactsAdapter());
            Registered<IBaseAPIAdapter, BaseAPIAdapter>(new BaseAPIAdapter());
            Registered<ISummaryDataAdapter, SummaryDataAdapter>(new SummaryDataAdapter());
            Registered<IRegionAdapter, RegionAdapter>(new RegionAdapter());        
            Registered<IConfigSolutionAdapter, ConfigSolutionAdapter>(new ConfigSolutionAdapter());
            Registered<IReportHistoryAdapter, ReportHistoryAdapter>(new ReportHistoryAdapter());
        }

        public static ConditionFactory Conditions
        {
            get
            {
                return _Instance;
            }
        }

        private void Registered<TInterface, TImplement>(TImplement obj) where TImplement : TInterface
        {
            if (_items.ContainsKey(typeof(TInterface)))
                throw new Exception(string.Format("{0}类型已注册"));
            _items.Add(typeof(TInterface), obj);
        }

        public TResult Resolve<TResult>()
        {
            if (!typeof(TResult).IsInterface)
                throw new Exception(string.Format("{0}类型TResult必须是接口类型", typeof(TResult).Name));
            if (!_items.ContainsKey(typeof(TResult)))
                throw new Exception(string.Format("{0}类型尚未注册", typeof(TResult).Name));
            return (TResult)_items[typeof(TResult)];
        }
    }
}
