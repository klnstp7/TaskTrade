using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Xml;
using GHSoft.DTO;
using GHSoft.Helper;
using Peacock.PEP.Data.Entities;
using Peacock.PEP.Service.Base;
using System.Linq;
using Peacock.PEP.Repository.Repositories;
using Peacock.Common.Helper;
using Peacock.PEP.Service.PeacockReportWS;
using System.ComponentModel;

namespace Peacock.PEP.Service
{
    public class SummaryDataService : SingModel<SummaryDataService>
    {
        private SummaryDataService()
        {
        }

        /// <summary>
        /// 通过ID获取汇总数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SummaryData GetById(long id)
        {
            var entity = SummaryDataRepository.Instance.Find(x => x.Id == id).FirstOrDefault();
            return entity;
        }

        /// <summary>
        /// 通过项目ID获取汇总数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SummaryData GetByProjectId(long projectId)
        {
            var pj = ProjectRepository.Instance.Source.FirstOrDefault(x => x.Id == projectId);
            var entity = SummaryDataRepository.Instance.Find(x => x.Project == pj).FirstOrDefault();
            return entity;
        }

        public void Save(SummaryData entity)
        {
            if (entity.Id == 0)
            {
                SummaryDataRepository.Instance.Insert(entity);
            }
            else
            {
                SummaryDataRepository.Instance.Save(entity);
            }
        }

        /// <summary>
        /// 提取报告汇总数据
        /// </summary>
        /// <param name="projectNo"></param>
        public void ExtractionSummaryData(string projectNo)
        {
            try
            {
                string reportDataFormat = ConfigurationManager.AppSettings["ReportDateFormat"];
                if (string.IsNullOrEmpty(reportDataFormat))
                {
                    LogHelper.Error("缺少报告生成系统日期格式配置", null);
                }
                var reportDataFormatList = reportDataFormat.Split(',');

                var ws = new PeacockReportWS.PeacockReportWS();
                ws.Url = System.Configuration.ConfigurationManager.AppSettings["ReportServiceUrl"];
                var inputdto = new WSInputDTO<GHSoft.Collections.SerializableDictionary<string, object>>();
                var gh = new GHSoft.Collections.SerializableDictionary<string, object>();
                gh.Add("trustno", projectNo);
                inputdto.Parameter.Params = gh;
                inputdto.Command.CommandName = "GetReportCollectSourceXml";
                inputdto.Security = null;
                string outputXml = ws.WSMethod(XmlHelper.ObjectToASXml(inputdto));
                LogHelper.Error(outputXml, null);
                WSOutputDTO<string> outputDTO = XmlHelper.XmlToOutputDTO<string>(outputXml);
                if (outputDTO.Status.Success)
                {
                    XmlDocument resultXml = new XmlDocument();
                    resultXml.LoadXml(outputDTO.ResultData);
                    XmlNodeList noteSate = resultXml.GetElementsByTagName("datas");
                    // int Count = noteSate[0].ChildNodes.Count;
                    XmlNodeList list = resultXml.GetElementsByTagName("data");//要查询的的节点名  

                    SummaryData Entity = SummaryDataRepository.Instance.Find(x => x.Project.ProjectNo == projectNo).FirstOrDefault();

                    Entity.IsComplete = true;
                    Type type = typeof(SummaryData);
                    ConstructorInfo constructure_new = type.GetConstructor(new Type[] { });
                    PropertyInfo[] propertys = type.GetProperties();

                    Dictionary<string, string> DicMaping = GetSummaryDataMaping();

                    foreach (XmlNode node in list)
                    {
                        string desc = node.Attributes["desc"].Value;
                        string value = node.Attributes["value"].Value;

                        foreach (PropertyInfo pi in propertys)
                        {
                            if (DicMaping.Keys.Contains(desc) && !string.IsNullOrWhiteSpace(DicMaping[desc]))
                            {
                                if (DicMaping[desc] == pi.Name)
                                {
                                    try
                                    {
                                        Type tp = pi.PropertyType;
                                        object g = value;
                                        object Convalue = new object();

                                        if (tp == typeof(DateTime?))
                                        {
                                            string timeString = g.ToString();
                                            DateTime time;
                                            foreach (var dateFormart in reportDataFormatList)
                                            {
                                                if (DateTime.TryParseExact(timeString, dateFormart, null, System.Globalization.DateTimeStyles.None, out time))
                                                {
                                                    Convalue = time;
                                                    break;
                                                }
                                                else
                                                {
                                                    Convalue = null;
                                                }
                                            }
                                        }
                                        else if (tp == typeof(Int32))
                                        {
                                            Convalue = (int)Convert.ToDouble(g);
                                        }
                                        else if(tp.IsGenericType && tp.GetGenericTypeDefinition().Equals(typeof(Nullable< >)))
                                        {
                                            NullableConverter nullConverter = new NullableConverter(tp);
                                            tp = nullConverter.UnderlyingType;
                                            Convalue = Convert.ChangeType(g, tp);
                                        }
                                        else
                                        {
                                            Convalue = Convert.ChangeType(g, tp);
                                        }

                                        pi.SetValue(Entity, Convalue, new object[] { });
                                    }
                                    catch (Exception ex)
                                    {
                                        if (ex is ApplicationException)
                                            throw;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    SummaryDataRepository.Instance.Save(Entity);
                    LogHelper.Error(JsonHelper.JsonSerializer<SummaryData>(Entity), null);
                }
                string FileUrl = outputDTO.ResultData;
            }
            catch (Exception ex)
            {
                if (ex is ApplicationException)
                    throw ex;
                LogHelper.Error("获取线上汇总数据出错", ex);
            }
        }
        /// <summary>
        /// 获取数据汇总映射关系
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetSummaryDataMaping()
        {
            string FilePath = AppDomain.CurrentDomain.BaseDirectory + "汇总数据字段对应.xls";
            //FilePath = @"D:\Source Code\VS2010\Peacock\Peacock.InWork3\Code\Peacock.InWork2.MvcWebSite\汇总数据字段对应.xls";
            return ExcelAsposeHelper.GetDataMaping(FilePath);
        }
    }
}
