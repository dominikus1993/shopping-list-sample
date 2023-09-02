namespace ShoppingList.Core.Abstractions;

public abstract class AggregateRoot
{
    private readonly List<Event> _changes = new();
    public int Version { get; private set; }

    public IEnumerable<Event> GetUncommittedChanges()
    {
        return _changes;
    }

    public void MarkChangesAsCommitted()
    {
        _changes.Clear();
    }

    protected void ApplyChange(Event e)
    {
        e.Version = Version + 1;
        Version = e.Version;
        ApplyChange(e, true);
    }

    protected abstract void Apply(Event @event);

    private void ApplyChange(Event e, bool isNew)
    {
        this.Apply(e);
        if (isNew)
        {
            _changes.Add(e);
        }
    }

    public void LoadsFromHistory(IEnumerable<Event> history)
    {
        foreach (var e in history)
        {
            ApplyChange(e, false);
            Version = e.Version;
        }
    }
}