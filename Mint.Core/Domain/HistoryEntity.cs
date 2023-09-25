using System.Text.Json;

namespace Mint.Core.Domain;

public abstract class HistoryEntity<TEntity, TKey> : Entity<long>
    where TEntity : Entity<TKey>
{
    public TKey EntityId { get; set; }
    public string AuditorId { get; set; }
    public string Snapshot { get; set; }

    protected HistoryEntity()
    {
    }

    protected HistoryEntity(TEntity entity, object auditorId)
    {
        EntityId = entity.Id;
        AuditorId = auditorId.ToString();
        Snapshot = GetSnapShot(entity);
    }

    protected string GetSnapShot(TEntity entity)
    {
        var snapshotObject = CreateSnapshotFromAggregate(entity);
        return JsonSerializer.Serialize(snapshotObject, new JsonSerializerOptions(JsonSerializerDefaults.General));
    }

    protected abstract object CreateSnapshotFromAggregate(TEntity entity);
}