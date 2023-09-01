namespace ShoppingList.Core.Abstractions;

public abstract class AggregateRoot
{
    private readonly List<IEvent> _changes = new();
    public int Version { get; private set; }

    public IEnumerable<IEvent> GetUncommittedChanges()
    {
        return _changes;
    }

    public void MarkChangesAsCommitted()
    {
        _changes.Clear();
    }

    protected void ApplyChange(IEvent e)
    {
        e.Version = Version + 1;
        ApplyChange(e, true);
    }

    protected abstract void Apply(IEvent @event);

    private void ApplyChange(IEvent e, bool isNew)
    {
        this.Apply(e);
        if (isNew)
        {
            _changes.Add(e);
        }
    }

    public void LoadsFromHistory(IEnumerable<IEvent> history)
    {
        foreach (var e in history)
        {
            ApplyChange(e, false);
            Version = e.Version;
        }
    }
}