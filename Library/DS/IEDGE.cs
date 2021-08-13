using System;

namespace Library
{
    public interface IEdge<TVertex> : IComparable<IEdge<TVertex>> where TVertex : IComparable<TVertex>
    {
        bool IsWeighted { get; }

        TVertex Source { get; set; }

        TVertex Destination { get; set; }
        Int64 Weight { get; set; }
    }
}

