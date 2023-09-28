using Blackbird.Applications.Sdk.Common;

namespace Apps.ModelFront;

public class ModelFrontApplication : IApplication
{
    public string Name
    {
        get => "ModelFront";
        set { }
    }

    public T GetInstance<T>()
    {
        throw new NotImplementedException();
    }
}