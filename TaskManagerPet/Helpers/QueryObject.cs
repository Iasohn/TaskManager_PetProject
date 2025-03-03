using System;

namespace TaskManagerPet.Helpers;

public class QueryObject
{
    public string? TaskName { get; set; } = null;

    public string? TaskStatus { get; set; } = string.Empty;

    public string? TaskDescription { get; set; } = String.Empty;
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}
