namespace ToDo.Application.Abstractions.Behaviors;

public class ValidationError
{
    private string propertyName;
    private string errorMessage;

    public ValidationError(string propertyName, string errorMessage)
    {
        this.propertyName = propertyName;
        this.errorMessage = errorMessage;
    }
}