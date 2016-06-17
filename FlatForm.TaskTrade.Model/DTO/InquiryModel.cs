using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Peacock.PMS.Service.Services;

namespace Peacock.PEP.Model
{
    public class InquiryModel
    {

        /// <summary>
        /// auto_increment
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// ResidentialAreaName
        /// </summary>
        public string ResidentialAreaName { get; set; }

        /// <summary>
        /// ResidentialAddress
        /// </summary>
        public string ResidentialAddress { get; set; }

        /// <summary>
        /// BuildingArea
        /// </summary>
        public decimal? BuildingArea { get; set; }

        /// <summary>
        /// City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// District
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// BuildedYear
        /// </summary>
        public int? BuildedYear { get; set; }

        /// <summary>
        /// InquiryResult
        /// </summary>
        public decimal? InquiryResult { get; set; }

        /// <summary>
        /// InquiryPrice
        /// </summary>
        public decimal? InquiryPrice { get; set; }

        /// <summary>
        /// FeedbackPrice
        /// </summary>
        public decimal? FeedbackPrice { get; set; }

        /// <summary>
        /// MortgagePrice
        /// </summary>
        public decimal? MortgagePrice { get; set; }

        /// <summary>
        /// MortgageResult
        /// </summary>
        public decimal? MortgageResult { get; set; }

        /// <summary>
        /// CustomerId
        /// </summary>
        public long? CustomerId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CustomerModel Customer { get; set; }

        /// <summary>
        /// BuildingName
        /// </summary>
        public string BuildingName { get; set; }

        /// <summary>
        /// UnitName
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// HouseNum
        /// </summary>
        public string HouseNum { get; set; }

        /// <summary>
        /// PropertyType
        /// </summary>
        public string PropertyType { get; set; }

        /// <summary>
        /// Floor
        /// </summary>
        public string Floor { get; set; }

        /// <summary>
        /// MaxFloor
        /// </summary>
        public string MaxFloor { get; set; }

        /// <summary>
        /// Toword
        /// </summary>
        public string Toword { get; set; }

        /// <summary>
        /// HouseType
        /// </summary>
        public string HouseType { get; set; }

        /// <summary>
        /// Decoration
        /// </summary>
        public string Decoration { get; set; }

        /// <summary>
        /// SpecialInfo
        /// </summary>
        public string SpecialInfo { get; set; }

        /// <summary>
        /// InquirySource
        /// </summary>
        public string InquirySource { get; set; }

        /// <summary>
        /// Address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        public string FeedbackMessage { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// CreatorId
        /// </summary>
        public long CreatorId { get; set; }

        /// <summary>
        /// CreatorName
        /// </summary>
        public string CreatorName { get; set; }

        /// <summary>
        /// IsToProject
        /// </summary>
        public bool IsToProject { get; set; }

        /// <summary>
        /// DepartmentId
        /// </summary>
        public long DepartmentId { get; set; }

        #region ѯ�۵������ֶ�

        /// <summary>
        /// ѯ�۵����
        /// </summary>
        public string InquiryNo { get; set; }

        /// <summary>
        /// ����ʹ�÷�
        /// </summary>
        public string AppraiseUse { get; set; }

        /// <summary>
        /// ����ί����
        /// </summary>
        public string AppraiseDelegatePerson { get; set; }

        /// <summary>
        /// ���۶���
        /// </summary>
        public string AppraiseObject { get; set; }

        /// <summary>
        /// ����Ŀ��
        /// </summary>
        public string AppraisePurpose { get; set; }

        /// <summary>
        /// ��ֵʱ��
        /// </summary>
        public string AppraiseDate { get; set; }
        public string AppraiseDatestr { get; set; }
        /// <summary>
        /// ���ݽṹ
        /// </summary>
        public string HouseStruct { get; set; }

        /// <summary>
        /// ����Ȩ��״��
        /// </summary>
        //public string OtherPower { get; set; }
        public bool OtherPower { get; set; }


        /// <summary>
        /// ����Ȩ��״��
        /// </summary>
        public string OtherPowerDesc { get; set; }

        /// <summary>
        /// ��д���
        /// </summary>
        public string UpperAmount { get; set; }

        /// <summary>
        /// Ԥ�ƴ���˰��
        /// </summary>
        public decimal? DisposeFee { get; set; }

        /// <summary>
        /// Ӧ���ؼ�
        /// </summary>
        public decimal? LandPremium { get; set; }

        /// <summary>
        /// �ۺ�ֵ
        /// </summary>
        public decimal? MinusNetWorth { get; set; }

        /// <summary>
        /// ��������˵��
        /// </summary>
        public string SpecialRemark { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public string IssueDate { get; set; }

        /// <summary>
        /// ��Ч��
        /// </summary>
        public string ValidityDate { get; set; }

        public string ValidityDatestr { get; set; }

        /// <summary>
        /// ���ۻ�������
        /// </summary>
        public string AppraiseOrg { get; set; }

        /// <summary>
        /// �趨��;
        /// </summary>
        public string SetPurpose { get; set; }


        /// <summary>
        /// �趨����
        /// </summary>
        public string SetOther { get; set; }

        /// <summary>
        /// ����Ŀ������
        /// </summary>
        public string AppraisePurposeDesc { get; set; }

        /// <summary>
        /// ѯ�۵���Ч��
        /// </summary>
        public string EffectivePeriod { get; set; }

        /// <summary>
        /// �Ƿ��п�����¼
        /// </summary>
        public bool IsHouseRecord { get; set; }

        /// <summary>
        /// �Ƿ���ڵ�Ѻ
        /// </summary>
        public bool IsMortgage { get; set; }

        /// <summary>
        /// ��Ѻ����
        /// </summary>
        public string MortgageDesc { get; set; }

        #endregion

    }
}
