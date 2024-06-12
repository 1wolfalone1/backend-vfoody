
using System.Text.Json;
using Newtonsoft.Json;

namespace VFoody.Application.UseCases.Dashboard.Models;

public class RevenueChartResponse
{
    public double ThisYear
    {
        get
        {
            if (this.ThisYearStr != null)
            {
                return double.Parse(this.ThisYearStr);
            }
            
            return 0;
        }
    }

    public double LastYear {
        get
        {
            if (this.LastYearStr != null)
            {
                return double.Parse(this.LastYearStr);
            }

            return 0;
        }
    }
    
    public List<RevenueChartResponse> TwelveMonthRevenue
    {
        get
        {
            if (this.TwelveMonthRevenueStr != null)
            {
                return JsonConvert.DeserializeObject<List<RevenueChartResponse>>(this.TwelveMonthRevenueStr);
            }

            return new List<RevenueChartResponse>();
        }
    }
    [System.Text.Json.Serialization.JsonIgnore]
    public string TwelveMonthRevenueStr { get; set; }    
    [System.Text.Json.Serialization.JsonIgnore]
    public string ThisYearStr { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public string LastYearStr { get; set; }
}

public class RevenueChartResponseItem
{
    public double ThisMonth
    {
        get
        {
            if (this.ThisYearStr != null)
            {
                return double.Parse(this.ThisYearStr);
            }
            
            return 0;
        }
    }

    public double LastYear {
        get
        {
            if (this.LastYearStr != null)
            {
                return double.Parse(this.LastYearStr);
            }

            return 0;
        }
    }
    public string ThisYearStr { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public string LastYearStr { get; set; }
}