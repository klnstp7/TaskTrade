
using Peacock.PEP.Data.Entities;
using Peacock.PEP.Repository.Repositories;
using Peacock.PEP.Service.Base;
using Peacock.Common.Helper;

namespace Peacock.PEP.Service
{
    public class OnlineFeedBackService : SingModel<OnlineFeedBackService>
    {
        private OnlineFeedBackService()
        {
        }

        /// <summary>
        /// 保存反馈意见
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public long Save(OnLineFeedBack entity)
        {
            LogHelper.Ilog("Save?OnLineFeedBack=" + entity.ToJson(), "保存反馈意见-" + Instance.ToString());
            OnLineFeedBackRepository.Instance.Insert(entity);
            return entity.Id;
        }

        /// <summary>
        /// 发送消息队列
        /// 作者：BOBO
        /// 时间：2016-4-26
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public short SendMessageQueue(OnLineFeedBack entity, string transactionNo)
        {
            CallBackMessageEntity param = new CallBackMessageEntity();
            param.businessType = BussinessType.信息反馈.ToString();
            param.onLineBusinessID = entity.OnLineBussinessId.ToString();
            param.businessId = transactionNo;
            param.bussinessForm = new BussinessForm
            {
                success = true,
                message = "反馈成功",
                data = new FeeBackData
                {
                    comment = entity.Content + ",反馈内容:" + entity.Remark,
                    companyName = UserService.Instance.GetUserCompany().CompanyName,
                    createTime = entity.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    feebacker = UserService.Instance.GetCrmUser().UserAccount
                }
            };


            return KafkaMQService.Instance.SendMessage(param);
        }
    }

    public class FeeBackData
    {
        public string comment { get; set; }

        public string feebacker { get; set; }

        public string createTime { get; set; }

        public string companyName { get; set; }
    }
}
