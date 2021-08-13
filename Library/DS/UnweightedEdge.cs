using System;

namespace Library
{
    public class UnweightedEdge<TVertex> : IEdge<TVertex> where TVertex : IComparable<TVertex>
    {
        private const int _edgeWeight = 0;

        public TVertex Source { get; set; }
        public TVertex Destination { get; set; }
        public Int64 Weight
        {
            get { throw new NotImplementedException("Unweighted edges don't have weights."); }
            set { throw new NotImplementedException("Unweighted edges can't have weights."); }
        }
        public bool IsWeighted
        {
            get
            { return false; }
        }
        public UnweightedEdge(TVertex src, TVertex dst)
        {
            Source = src;
            Destination = dst;
        }

        public int CompareTo(IEdge<TVertex> other)
        {
            if (other == null)
                return -1;

            bool areNodesEqual = Source.IsEqualTo<TVertex>(other.Source) && Destination.IsEqualTo<TVertex>(other.Destination);

            if (!areNodesEqual)
                return -1;
            return 0;
        }
    }
}

