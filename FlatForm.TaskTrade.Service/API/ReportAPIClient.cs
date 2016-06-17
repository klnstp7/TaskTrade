using GHSoft.DTO;
using Peacock.Common.Helper;
using Peacock.PEP.Model.DTO;
using ResourceLibraryExtension.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GHValue = GHSoft.Collections.SerializableDictionary<string, object>;

namespace Peacock.PEP.Service.API
{
    /// <summary>
    /// 报告生成系统服务
    /// </summary>
    public class ReportAPIClient
    {
        private string ReportServiceUrl;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ReportAPIClient()
        {
            ReportServiceUrl = System.Configuration.ConfigurationManager.AppSettings["ReportServiceUrl"];
        }

        /// <summary>
        /// 获取线上报告文件的地址
        /// </summary>
        /// <param name="ProjectNo">流水号</param>
        /// <returns></returns>
        public ReportFilePathInfoDTO GetReportAuditFilePath(string ProjectNo)
        {
            ReportFilePathInfoDTO result = null;
            try
            {
                Peacock.PEP.Service.PeacockReportWS.PeacockReportWS ws = new Peacock.PEP.Service.PeacockReportWS.PeacockReportWS();
                ws.Url = ReportServiceUrl;
                WSInputDTO<GHValue> inputdto = new WSInputDTO<GHValue>();
                GHValue gh = new GHValue();
                gh.Add("trustno", ProjectNo);
                inputdto.Parameter.Params = gh;
                inputdto.Command.CommandName = "GetReportWordFilePath";
                inputdto.Security = null;
                string outputXml = ws.WSMethod(XmlHelper.ObjectToASXml(inputdto));
                WSOutputDTO<ReportFilePathInfoDTO> outputDTO = XmlHelper.XmlToOutputDTO<ReportFilePathInfoDTO>(outputXml);
                ReportFilePathInfoDTO dto = outputDTO.ResultData;
                if (dto != null)
                {
                    result = new ReportFilePathInfoDTO();
                    if (dto.IsResourceSys)
                    {//资源库
                        result.WordPath = dto.WordPath;
                        result.ExcelPath = dto.ExcelPath;
                    }
                    else
                    {//非资源库，先把资源保存到资源库
                        try
                        {
                            result.WordPath = SingleFileManager.SaveFileResourceByNetPath(dto.WordPath, "报告文件", ProjectNo, "内业提取报告word保存").ToString();
                            result.ExcelPath = SingleFileManager.SaveFileResourceByNetPath(dto.ExcelPath, "计算表文件", ProjectNo, "内业提取报告excel保存").ToString();
                        }
                        catch {//保证能提交，临时办法
                            result.WordPath = "566811";
                            result.ExcelPath = "566811";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("获取线上报告文件的地址出错", ex);
                throw;
            }
            return result;
        }
    }
}
