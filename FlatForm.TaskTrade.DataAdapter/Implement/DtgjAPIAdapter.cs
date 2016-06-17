using System;
using System.Collections.Generic;
using Peacock.PEP.DataAdapter.Interface;
using Peacock.PEP.Service;

namespace Peacock.PEP.DataAdapter.Implement
{
    public class DtgjAPIAdapter:IDtgjAPIAdapter
    {
        public List<string> GetHouseTypeByArea(string address, int area)
        {
            return DtgjAPIService.Instance.GetHouseTypeByArea(address, area);
        }

        public List<string> GetSpecialInfo(string address)
        {
            return DtgjAPIService.Instance.GetSpecialInfo(address);
        }

        public string GetPriceDiff(string address)
        {
            return DtgjAPIService.Instance.GetPriceDiff(address);
        }

        public Tuple<double, int, string> GetFindPriceByMoHu(string propertytype, int housetype, double area,
            string filter, string cityname,
            string floor_building = null, int? buildedyear = null, int? floor = null, int? maxfloor = null,
            string toward = null, string special_factors = null, int? hall = null, int? toilet = null,
            string house_number = null, string renovation = null)
        {
            return DtgjAPIService.Instance.GetFindPriceByMoHu(propertytype, housetype, area,
                filter, cityname,
                floor_building, buildedyear, floor, maxfloor,
                toward, special_factors, hall, toilet,
                house_number, renovation);
        }

        public Tuple<double, double, double, double> GetPriceBetween(string cityname, string address, int area,
            string propertytype)
        {
            return DtgjAPIService.Instance.GetPriceBetween(cityname, address, area, propertytype);
        }

        public List<Tuple<string, string, double, double, string>> GetResidential(string cityname, string address,
            int count)
        {
            return DtgjAPIService.Instance.GetResidential(cityname, address, count);
        }

        public bool FeedbackMessage(string message)
        {
            return DtgjAPIService.Instance.FeedbackMessage(message);
        }
    }
}
