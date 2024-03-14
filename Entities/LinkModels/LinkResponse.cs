using Entities.Models;

namespace Entities.LinkModels;

public class LinkResponse
{
    /// <summary>
    /// HasLinks is true, we use LinkedEntities, otherwise, we use ShapedEntities
    /// </summary>
    public bool HasLinks { get; set; }

    public List<Entity> ShapedEntities { get; set; }

    public LinkCollectionWrapper<Entity> LinkedEntities { get; set; }

    public LinkResponse()
    {
        ShapedEntities = [];
        LinkedEntities = new LinkCollectionWrapper<Entity>();
    }
}
