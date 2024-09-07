using SaintTest.CodeBase.Items;

namespace SaintTest.CodeBase.Storages
{
    public interface IStorage : ISender, ITaker
    {
        bool HasItem(Item item);
    }
}