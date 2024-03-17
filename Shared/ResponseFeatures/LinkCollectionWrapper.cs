namespace Shared.ResponseFeatures;

/// <summary>
/// LinkCollectionWrapper describes the root of the controller for the responses
/// </summary>
/// <typeparam name="T"></typeparam>
public class LinkCollectionWrapper<T> : LinkResourceBase
{
    public List<T> Value { get; set; } = [];

    public LinkCollectionWrapper()
    {
    
    }

    public LinkCollectionWrapper(List<T> value)
    {
        Value = value;
    }
}
