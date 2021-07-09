
namespace Bb.DataDeep.Models.Mpd
{
    public abstract class Reader<T>
    {

        public abstract StructureMpdBase Resolve(T source, StructureMpdBase parent);

    }

}
