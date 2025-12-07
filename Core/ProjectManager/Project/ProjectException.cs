namespace Qlang.Core.ProjectManager.Project;

public class ProjectException(string message) : Exception(message)
{
    public override string Message { get; } = message;
}