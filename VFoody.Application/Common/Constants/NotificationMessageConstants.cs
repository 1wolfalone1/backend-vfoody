namespace VFoody.Application.Common.Constants;

public class NotificationMessageConstants
{
    // Order title
    public static string Order_Title = "Đơn hàng";
    
    // Customer Order content
    public static string Order_Fail = "Đặt hàng thất bại";
    public static string Order_Pending_Content = "Đặt hàng thành công";
    public static string Order_Confirmed_Content = "Shop đã xác nhận đơn hàng VFD{0}";
    public static string Order_Delivering_Content = "Đơn hàng VFD{0} đang được giao";
    public static string Order_Reject_Content = "Đơn hàng VFD{0} đã bị từ chối";
    public static string Order_Cancel_Content = "Đơn hàng VFD{0} đã bị hủy";
    public static string Order_Fail_Content = "Đơn hàng VFD{0} giao hàng không thành công";


    // Shop Order content
    public static string Order_Shop_Pending = "Bạn có một đơn hàng mới!";
    public static string Order_Successfull_Content = "Đơn hàng VFD{0} đã giao thành công";
    
    // Payment
    public static string Payment_Order_Success = "Bạn vừa thanh toán hóa đơn hàng thành công cho đơn hàng VFD{0}";
    public static string Payment_Order_Fail = "Thanh toán đơn hàng VFD{0} thất bại";
    
    // Shop Balance
    public static string Shop_Balance_Title = "Tài khoản";
    public static string Shop_Balance_Plus_From_Order = "Bạn vừa được +{0} VND vào tài khoản từ đơn hàng VFD{1}";
    
}