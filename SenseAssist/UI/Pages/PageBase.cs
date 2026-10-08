namespace SenseAssist.UI.Pages;

/// <summary>A screen shown in the main window's content area.</summary>
public abstract class PageBase : UserControl
{
    protected PageBase()
    {
        BackColor = Theme.Background;
        DoubleBuffered = true;
    }

    public abstract string Title { get; }
    public abstract string Subtitle { get; }

    /// <summary>Spoken when the page opens, e.g. "You have 3 reminders, 1 overdue."</summary>
    public virtual string Summary => "";
    public virtual string Help => "";

    public virtual void RefreshData() { }
    public virtual void AddNew() { }
    public virtual void EditSelected() { }
    public virtual string ReadCurrent() => Summary;
    public virtual Control? FirstFocus => null;
}
