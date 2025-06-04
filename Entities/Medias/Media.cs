using Entities.Common;

namespace Entities.Medias;
public class Media : BaseEntity<Guid>
{
    public Media(string uRL, Guid objectId, MediaTypes type)
    {
        URL = uRL;
        ObjectId = objectId;
        Type = type;
    }

    public string URL { get; private set; }
    public Guid ObjectId { get; private set; }
    public MediaTypes Type { get; private set; }


    internal void SetURL(string url) => URL = url;

    internal void SetObjectId(Guid id) => ObjectId = id;

    internal void SetType(MediaTypes type) => Type = type;
}