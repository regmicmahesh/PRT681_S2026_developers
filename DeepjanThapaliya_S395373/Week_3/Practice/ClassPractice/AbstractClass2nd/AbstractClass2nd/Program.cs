using System;

List<NotificationSender> notifications = new List<NotificationSender>();

notifications.Add(new EmailSender());
notifications.Add(new SmsSender());

foreach (NotificationSender notification in notifications)
{
    notification.Send("Your order is ready.");
}

Console.ReadLine();

public abstract class NotificationSender
{
    public NotificationSender()
    {

    }

    public abstract void Send(string message);
}

public class EmailSender : NotificationSender
{

    public EmailSender()
    {

    }

    public override void Send(string message)
    {
        Console.WriteLine($"This is Email Notification: {message}");
    }
}

public class SmsSender : NotificationSender
{

    public SmsSender()
    {

    }

    public override void Send(string message)
    {
        Console.WriteLine($"This is SMS Notification: {message}");
    }
}