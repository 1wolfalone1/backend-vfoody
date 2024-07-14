using System.Globalization;

namespace VFoody.Application.Common.Utils;

public class StringUtils
{
    public static string DateToStringFormat(DateTime dateTime)
    {
        string formattedDate = dateTime.ToString("dd/MM/yyyy hh:mm tt", CultureInfo.InvariantCulture);
        return formattedDate;
    }

    public static string ToVnCurrencyFormat(float amount)
    {
        CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
        string amountFormat = double.Parse(amount.ToString()).ToString("#,###", cul.NumberFormat);
        return amountFormat + " đ";
    }
    public static string ToVnCurrencyFormat(double amount)
    {
        CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
        string amountFormat = double.Parse(amount.ToString()).ToString("#,###", cul.NumberFormat);
        return amountFormat + " đ";
    }
}