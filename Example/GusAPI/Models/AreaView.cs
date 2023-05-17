namespace GusAPI.Models;

public class AreaView : Area
{
    public bool IsEditing { get; set; }

    public AreaView() : base()
    {
    }

    public AreaView(int id, string name, int idParent, int levelId, string levelName, bool isChangeable, bool isEditing)
        : base(id, name, idParent, levelId, levelName, isChangeable)
    {
        IsEditing = isEditing;
    }

    public override string ToString()
    {
        return $"{Id}: {Name}; {IdParent}; {LevelId}; {LevelName}; {IsChangeable}";
    }
}

public static class AreaViewExtensions
{
    public static ICollection<AreaView> ToAreaView(this IEnumerable<Area> areas) =>
        areas.Select(p => new AreaView(p.Id, p.Name, p.IdParent, p.LevelId, p.LevelName, p.IsChangeable, false))
            .ToList();
}