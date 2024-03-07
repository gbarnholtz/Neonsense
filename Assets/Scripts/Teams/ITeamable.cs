public interface ITeamable
{
    public Team Team { get; set; }
    
}
public enum Team
{
    Neutral,
    Ally,
    Enemy
}