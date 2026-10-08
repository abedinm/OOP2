namespace SenseAssist.UI.Controls;

/// <summary>Clean, roomy DataGridView styling (big rows, no clutter, clear selection).</summary>
public static class GridStyler
{
    public static void Style(DataGridView grid)
    {
        grid.BorderStyle = BorderStyle.None;
        grid.BackgroundColor = Theme.Surface;
        grid.GridColor = Theme.Border;
        grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        grid.EnableHeadersVisualStyles = false;
        grid.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Theme.SurfaceAlt,
            ForeColor = Theme.Muted,
            SelectionBackColor = Theme.SurfaceAlt,
            SelectionForeColor = Theme.Muted,
            Font = Theme.Font(9.5f, FontStyle.Bold),
            Padding = new Padding(10, 0, 0, 0),
            Alignment = DataGridViewContentAlignment.MiddleLeft,
        };
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        grid.ColumnHeadersHeight = Theme.Px(44);
        grid.DefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Theme.Surface,
            ForeColor = Theme.Text,
            SelectionBackColor = Theme.AccentSoft,
            SelectionForeColor = Theme.Text,
            Font = Theme.Font(10.5f),
            Padding = new Padding(10, 0, 0, 0),
            Alignment = DataGridViewContentAlignment.MiddleLeft,
        };
        grid.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Theme.SurfaceAlt };
        grid.RowTemplate.Height = Theme.Px(46);
        grid.RowHeadersVisible = false;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.MultiSelect = false;
        grid.ReadOnly = true;
        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        grid.AllowUserToResizeRows = false;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.StandardTab = true; // Tab leaves the grid instead of moving between cells.
    }

    public static void AddColumn(DataGridView grid, string header, float weight) =>
        grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = header,
            FillWeight = weight,
            SortMode = DataGridViewColumnSortMode.NotSortable,
        });
}
