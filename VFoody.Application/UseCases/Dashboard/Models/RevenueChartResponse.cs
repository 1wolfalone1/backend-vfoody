
using System.Text.Json;
using Newtonsoft.Json;

namespace VFoody.Application.UseCases.Dashboard.Models;

public class RevenueChartResponse
{
    public int ThisYear { get; set; }
    public int LastYear { get; set; }
    public double TotalOfThisYear
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

    public double TotalOflastYear {
        get
        {
            if (this.LastYearStr != null)
            {
                return double.Parse(this.LastYearStr);
            }

            return 0;
        }
    }
    
    public List<RevenueChartResponseItem> TwelveMonthRevenue
    {
        get
        {
            if (this.TwelveMonthRevenueStr != null)
            {
                return JsonConvert.DeserializeObject<List<RevenueChartResponseItem>>(this.TwelveMonthRevenueStr);
            }

            return new List<RevenueChartResponseItem>();
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

    [System.Text.Json.Serialization.JsonIgnore]
    public string ThisYearStr { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public string LastYearStr { get; set; }
}