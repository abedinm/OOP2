using SenseAssist.Data;
using SenseAssist.Models;
using SenseAssist.Services;
using SenseAssist.UI.Controls;

namespace SenseAssist.UI.Pages;

/// <summary>
/// Shared list screen with Add / Edit / Delete / Read aloud / Search.
/// Subclasses only describe their columns and editor dialog.
/// </summary>
public abstract class CrudPage<T> : PageBase where T : Entity
{
    protected readonly User User;
    protected readonly IRepository<T> Repo;
    protected readonly DataGridView Grid = new();
    protected readonly FlowLayoutPanel Toolbar;
    protected List<T> Items = [];

    readonly InputBox search;
    readonly Label emptyLabel;
    readonly ModernButton addButton;
    bool binding;

    protected abstract string Noun { get; }
    protected abstract void ConfigureColumns(DataGridView grid);
    protected abstract object[] RowValues(T item);
    protected abstract string DisplayName(T item);
    /// <summary>Show the add/edit dialog. Returns the edited item, or null when cancelled.</summary>
    protected abstract T? ShowEditor(T? existing);
    protected virtual void StyleRow(DataGridViewRow row, T item) { }
    protected virtual void AddExtraTools() { }

    protected CrudPage(User user, IRepository<T> repo)
    {
        User = user;
        Repo = repo;

        var card = new CardPanel { Dock = DockStyle.Fill, Padding = new Padding(18, 16, 18, 18) };
        var top = new Panel { Dock = DockStyle.Top, Height = Theme.Px(58), BackColor = Theme.Surface };
        Toolbar = new FlowLayoutPanel { Dock = DockStyle.Fill, BackColor = Theme.Surface, WrapContents = false, Padding = new Padding(0, 2, 0, 0) };
        var searchHost = new Panel { Dock = DockStyle.Right, Width = Theme.Px(260), BackColor = Theme.Surface, Padding = new Padding(0, 0, 0, 12) };
        search = new InputBox($"Search {Noun}s", placeholder: $"Search {Noun}s…") { Dock = DockStyle.Fill };
        searchHost.Controls.Add(search);
        top.Controls.Add(Toolbar);
        top.Controls.Add(searchHost);

        addButton = Tool($"Add {Noun}", Glyph.Add, ButtonKind.Primary, AddNew);
        addButton.AccessibleDescription = "Control N";
        Tool("Edit", Glyph.Edit, ButtonKind.Secondary, EditSelected).AccessibleDescription = "Control E or Enter";
        Tool("Delete", Glyph.Delete, ButtonKind.Danger, DeleteSelected).AccessibleDescription = "Delete key";
        AddExtraTools();
        Tool("Read aloud", Glyph.Speak, ButtonKind.Ghost, () => Speech.Say(ReadCurrent())).AccessibleDescription = "Control R or Space";

        GridStyler.Style(Grid);
        Grid.Dock = DockStyle.Fill;
        ConfigureColumns(Grid);
        Grid.AccessibleName = $"{Noun}s";
        Grid.CellDoubleClick += (_, e) => { if (e.RowIndex >= 0) EditSelected(); };
        Grid.KeyDown += OnGridKeyDown;
        Grid.SelectionChanged += (_, _) =>
        {
            if (!binding && Grid.Focused && AppSettings.Current.ReadOnNavigate && Selected is { } item)
                Speech.Say(item.Describe());
        };
        Grid.Enter += (_, _) =>
        {
            if (!VoiceFocus.Muted)
                Speech.Say($"{Title} list, {Grid.Rows.Count} items. Arrow keys move, Enter edits, Delete removes, Space reads the row.", caption: false);
        };

        emptyLabel = new Label
        {
            Dock = DockStyle.Fill,
            Text = $"No {Noun}s yet.\nPress “Add {Noun}” or Control N to create one.",
            Font = Theme.Font(12.5f),
            ForeColor = Theme.Muted,
            BackColor = Theme.Surface,
            TextAlign = ContentAlignment.MiddleCenter,
            Visible = false,
        };

        card.Controls.Add(Grid);
        card.Controls.Add(emptyLabel);
        card.Controls.Add(new Panel { Dock = DockStyle.Top, Height = 8, BackColor = Theme.Surface });
        card.Controls.Add(top);
        Controls.Add(card);

        search.Box.TextChanged += (_, _) => Bind();
    }

    protected T? Selected => Grid.CurrentRow?.Tag as T;
    public override Control? FirstFocus => Grid.Rows.Count > 0 ? Grid : addButton;
    public override string ReadCurrent() => Selected?.Describe() ?? Summary;

    protected ModernButton Tool(string text, string glyph, ButtonKind kind, Action action)
    {
        var button = new ModernButton(text, glyph, kind);
        button.Click += (_, _) => action();
        Toolbar.Controls.Add(button);
        return button;
    }

    public override void RefreshData()
    {
        try
        {
            Items = Repo.GetAll(User.Id);
            Bind();
        }
        catch (Exception ex)
        {
            AppLog.Error($"Loading {Noun}s failed", ex);
            Speech.Say($"Could not load your {Noun}s from the database.");
        }
    }

    void Bind()
    {
        var keepId = Selected?.Id;
        binding = true;
        Grid.Rows.Clear();
        var term = search.Text.Trim();
        foreach (var item in Items.Where(i => term.Length == 0 || i.Describe().Contains(term, StringComparison.OrdinalIgnoreCase)))
        {
            var row = Grid.Rows[Grid.Rows.Add(RowValues(item))];
            row.Tag = item;
            StyleRow(row, item);
            if (item.Id == keepId)
                Grid.CurrentCell = row.Cells[0];
        }
        binding = false;
        emptyLabel.Visible = Grid.Rows.Count == 0;
        Grid.Visible = !emptyLabel.Visible;
    }

    void OnGridKeyDown(object? sender, KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.Enter:
                EditSelected();
                e.Handled = e.SuppressKeyPress = true;
                break;
            case Keys.Delete:
                DeleteSelected();
                e.Handled = true;
                break;
            case Keys.Space:
                Speech.Say(ReadCurrent());
                e.Handled = e.SuppressKeyPress = true;
                break;
        }
    }

    public override void AddNew()
    {
        var item = ShowEditor(null);
        if (item is null)
        {
            Speech.Say("Cancelled. Nothing was added.");
            return;
        }
        item.UserId = User.Id;
        Save(() => Repo.Add(item), "Add", $"Added {Noun}: {DisplayName(item)}.");
    }

    public override void EditSelected()
    {
        if (Selected is not { } current)
        {
            Speech.Say($"Select a {Noun} first.");
            return;
        }
        var item = ShowEditor(current);
        if (item is null)
        {
            Speech.Say("Cancelled. No changes were saved.");
            return;
        }
        Save(() => Repo.Update(item), "Update", $"Saved changes to {Noun}: {DisplayName(item)}.");
    }

    public void DeleteSelected()
    {
        if (Selected is not { } current)
        {
            Speech.Say($"Select a {Noun} first.");
            return;
        }
        var name = DisplayName(current);
        Speech.Say($"Delete {name}? Press Y for yes or N for no.");
        var answer = MessageBox.Show($"Delete “{name}”?\n\nThis cannot be undone.", $"Delete {Noun}",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
        if (answer != DialogResult.Yes)
        {
            Speech.Say($"Kept {name}.");
            return;
        }
        Save(() => Repo.Delete(current.Id, User.Id), "Delete", $"Deleted {Noun}: {name}.");
    }

    protected void Save(Action change, string action, string message)
    {
        try
        {
            change();
            ActivityLogger.Log(User.Id, action, message);
            RefreshData();
            Speech.Say(message);
        }
        catch (Exception ex)
        {
            AppLog.Error(message, ex);
            Speech.Say("That did not work. The database could not be reached.");
            MessageBox.Show("The change could not be saved.\n\n" + ex.Message, "SenseAssist", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
