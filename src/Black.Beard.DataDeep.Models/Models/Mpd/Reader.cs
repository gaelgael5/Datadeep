
namespace Bb.DataDeep.Models.Mpd
{
    public abstract class Reader<T>
    {

        public abstract StructureBase Resolve(T source, StructureBase parent);

    }

}
