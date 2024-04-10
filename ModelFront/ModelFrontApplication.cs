using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Metadata;

namespace Apps.ModelFront;

public class ModelFrontApplication : IApplication, ICategoryProvider
{
    public IEnumerable<ApplicationCategory> Categories
    {
        get => [ApplicationCategory.MachineTranslationAndMtqe];
        set { }
    }
    
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