using System;
using System.Collections.Generic;

namespace Peacock.PEP.DataAdapter.Interface
{
    public interface IDtgjAPIAdapter
    {
        List<string> GetHouseTypeByArea(string address, int area);

        List<string> GetSpecialInfo(string address);

        string GetPriceDiff(string address);

        Tuple<double, int, string> GetFindPriceByMoHu(string propertytype, int housetype, double area,
            string filter, string cityname,
            string floor_building = null, int? buildedyear = null, int? floor = null, int? maxfloor = null,
            string toward = null, string special_factors = null, int? hall = null, int? toilet = null,
            string house_number = null, string renovation = null);

        Tuple<double, double, double, double> GetPriceBetween(string cityname, string address, int area,
            string propertytype);

        List<Tuple<string, string, double, double, string>> GetResidential(string cityname, string address,
            int count);

        bool FeedbackMessage(string message);
    }
}
