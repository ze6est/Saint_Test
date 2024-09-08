using SaintTest.CodeBase.Items;
using SaintTest.CodeBase.Logic;

namespace SaintTest.CodeBase.Storages
{
    public interface IStorage : ISender, ITaker
    {
        bool HasItem(Item item);
    }
}