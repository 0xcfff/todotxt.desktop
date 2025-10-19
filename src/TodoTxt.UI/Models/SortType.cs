using System.ComponentModel;

namespace TodoTxt.UI.Models;

public enum SortType
{
    [Description("Order in file")]
    None,
    Alphabetical,
    Priority,
    [Description("Due Date")]
    DueDate,
    [Description("Creation Date")]
    Created,
    Project,
    Context,
    Completed
}
