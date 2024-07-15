namespace VFoody.Application.Common.Constants;

public class NotificationMessageConstants
{
    // Default logo url 
    public static string Noti_Logo_Default_Url =
        "https://v-foody.s3.ap-southeast-1.amazonaws.com/image/1719333573556-47123349-841f-4bf3-811b-933b98bbe53f";
    // Order title
    public static string Order_Title = "Đơn hàng";
    
    // Customer Order content
    public static string Order_Fail = "{0} đã đặt hàng thất bại";
    public static string Order_Pending_Content = "Đặt hàng cửa hàng {0} thành công";
    public static string Order_Confirmed_Content = "Shop đã xác nhận đơn hàng VFD{0}";
    public static string Order_Delivering_Content = "Đơn hàng VFD{0} đang được giao";
    public static string Order_Reject_Content = "Đơn hàng VFD{0} đã bị từ chối";
    public static string Order_Shop_Cancel_Content = "Đơn hàng VFD{0} đã bị hủy";
    public static string Order_Fail_Content = "Đơn hàng VFD{0} giao hàng không thành công";


    // Shop Order content
    public static string Order_Shop_Pending = "{0} đã đặt một đơn mới";
    public static string Order_Customer_Cancel = "{0} đã hủy đơn hàng VFD{1}";
    public static string Order_Successfull_Content = "Đơn hàng VFD{0} đã giao thành công";
    
    // Payment
    public static string Payment_Order_Success = "Bạn đã thanh toán hóa đơn hàng thành công cho đơn hàng VFD{0}";
    public static string Payment_Order_Fail = "Thanh toán đơn hàng VFD{0} thất bại";
    
    // Shop Balance
    public static string Shop_Balance_Title = "Tài khoản";
    public static string Shop_Balance_Plus_From_Order = "Tài khoản cửa hàng đã được +{0} từ đơn hàng VFD{1}";
    
    // Verify
    public static string Approve_Shop_Title = "Cửa hàng";
    public static string Approve_Shop_Cus_Content =
        "Cửa hàng {0} của bạn đã được xác nhận bởi hệ thống. Bạn đã có thể bắt đầu buôn bán";
    public static string Apprve_Shop_Welcome = "Chúc mừng bạn đã mở cửa hàng thành công. Bạn đã có thể đầu buôn bán";
    
    // Shop ban action
    public static string Ban_Shop_Title = "Cửa hàng";
    public static string Ban_Shop_Content =
        "Cửa hàng của bạn đã bị hệ thống cấm. Bạn vui lòng mở email để biết thêm chi tiết. Mọi chi tiết xin liên hệ với chúng tôi qua email";
    
    // Shop un ban action
    public static string Un_Ban_Shop_Title = "Cửa hàng";
    public static string Un_Ban_Shop_Content = "Chúc mừng cửa hàng của bạn đã được gỡ lệnh cấm";
    public static string Un_Ban_Shop_Cus_Content = "Cửa hàng của bạn đã có thể bán hàng trở lại bình thường";
    
    // Shop receive feedback
    public static string Feedback_Shop_Title = "Phản hồi";
    public static string Feedback_Shop_Content = "Bạn vừa nhận phản hồi từ khách hàng {0} với đơn hàng VFD{1}";
    
    // Shop withdrawal 
    public static string Shop_Withdrawal_Title = "Tài khoản";
    public static string Shop_Withdrawal_Pending_Content = "Tài khoản cửa hàng bạn vừa bị trừ -{0} với lý do rút tiền";
    public static string Shop_Withdrawal_Approve_Content = "Yêu cầu rút tiền của bạn đã được chấp nhận lúc {0}";
    public static string Shop_Withdrawal_Reject_Content = "Yêu cầu rút tiền của bạn đã bị từ chối lúc {0}";
    public static string Shop_Withdrawal_Reject_Refund_Content =
        "Tài khoản cửa hàng đã được +{0} từ yêu cầu rút từ yêu cầu rút tiền bị từ chối";

}