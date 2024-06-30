namespace VFoody.Application.Common.Constants;

public class NotificationMessageConstants
{
    // Default logo url 
    public static string Noti_Logo_Default_Url =
        "https://v-foody.s3.ap-southeast-1.amazonaws.com/image/1719333573556-47123349-841f-4bf3-811b-933b98bbe53f";
    // Order title
    public static string Order_Title = "Đơn hàng";
    
    // Customer Order content
    public static string Order_Fail = "{0} vừa đặt hàng thất bại";
    public static string Order_Pending_Content = "Đặt hàng cửa hàng {0} thành công";
    public static string Order_Confirmed_Content = "Shop đã xác nhận đơn hàng VFD{0}";
    public static string Order_Delivering_Content = "Đơn hàng VFD{0} đang được giao";
    public static string Order_Reject_Content = "Đơn hàng VFD{0} đã bị từ chối";
    public static string Order_Shop_Cancel_Content = "Đơn hàng VFD{0} đã bị hủy";
    public static string Order_Fail_Content = "Đơn hàng VFD{0} giao hàng không thành công";


    // Shop Order content
    public static string Order_Shop_Pending = "{0} vừa đặt một đơn mới";
    public static string Order_Customer_Cancel = "{0} vừa hủy đơn hàng VFD{1}";
    public static string Order_Successfull_Content = "Đơn hàng VFD{0} đã giao thành công";
    
    // Payment
    public static string Payment_Order_Success = "Bạn vừa thanh toán hóa đơn hàng thành công cho đơn hàng VFD{0}";
    public static string Payment_Order_Fail = "Thanh toán đơn hàng VFD{0} thất bại";
    
    // Shop Balance
    public static string Shop_Balance_Title = "Tài khoản";
    public static string Shop_Balance_Plus_From_Order = "Bạn vừa được +{0} VND vào tài khoản từ đơn hàng VFD{1}";
    
}