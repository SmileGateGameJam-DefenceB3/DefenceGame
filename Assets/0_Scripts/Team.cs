public enum Team
{
    Player,
    CPU,
}

public static class TeamExtension
{
    public static Team GetEnemy(this Team team)
    {
        return team == Team.Player ? Team.CPU : Team.Player;
    }
}