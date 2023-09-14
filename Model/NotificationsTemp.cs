using Utils.injection;
using Utils.signal;

namespace Model
{
    public enum NotificationType
    {
        CardUsed,
        Resolve,
        PerformAction,
        ReceiveDamage,
        Reset,
        
        VFXSlash,
        VFXSlashHorisontal,
        VFXRanged,
    }

    [Singleton]
    public class NotificationsTemp : Signal<NotificationType, bool, string>
    {
    }
}