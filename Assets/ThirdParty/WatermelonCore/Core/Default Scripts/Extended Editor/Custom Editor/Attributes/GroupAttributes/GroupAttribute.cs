namespace Watermelon
{
    public abstract class GroupAttribute : ExtendedEditorAttribute
    {
        public GroupAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}