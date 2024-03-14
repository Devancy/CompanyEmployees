namespace Entities.Models;

/// <summary>
/// ShapedEntity carries the Id to constructs the links for the responses
/// </summary>
public class ShapedEntity
{
    public ShapedEntity()
    {
        Entity = new();
    }
    public Guid Id { get; set; }
    public Entity Entity { get; set; }
}
