namespace CourseHub.Core.Interfaces.Logging;

public interface IAppLogger
{
    public const string INFORMATION_TEMPLATE
        = "[{time}] [Information]: [{message}]";
    public const string WARNING_TEMPLATE
        = "[{time}] [Warning]    : [{message}]";

    public void Inform(string message);

    public void Warn(string message);
}
