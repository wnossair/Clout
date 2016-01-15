namespace Clout
{
    public interface IEngine
    {
        CommandResult AddLink(string follower, string leader);

        CommandResult Calculate();

        CommandResult Calculate(string name);
    }
}