namespace VFoody.Application.UseCases.Notifications.Models;

public class NotificationResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public bool Readed { get; set; }
    public int AccountId { get; set; }
    public string ImageUrl { get; set; }
    public DateTime CreatedDate { get; set; }
}