namespace EmployeeManagement.Domain.Interfaces;

public interface IFileLogger
{
    void Log(string message);
    void LogError(string message, Exception? ex);
}
