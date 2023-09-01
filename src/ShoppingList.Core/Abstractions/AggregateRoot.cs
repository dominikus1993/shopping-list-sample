namespace ShoppingList.Core.Abstractions;

public abstract class AggregateRoot
{
    private readonly List<IEvent> _changes = new();
    public int Version { get; internal set; }

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

    private void ApplyChange(IEvent e, bool isNew)
    {
        dynamic me = this;
        me.Apply(e);
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