using System.Runtime.Serialization;

namespace OutlandsTool.ServiceModel.Entities.Base
{
    [DataContract]
    public abstract class Entity<TId> : EntityBase<TId>
        where TId : struct
    {
    }
}
