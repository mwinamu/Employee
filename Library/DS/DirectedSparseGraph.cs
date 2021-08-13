
using System;
using System.Collections.Generic;

namespace Library
{
    public class DSG<T> : IGraph<T> where T : IComparable<T>
    {
        protected virtual int _edgesCount { get; set; }
        protected virtual T _firstInsertedNode { get; set; }
        protected virtual Dictionary<T, DLinkedList<T>> _adjacencyList { get; set; }


        public DSG() : this(10) { }

        public DSG(uint initialCapacity)
        {
            _edgesCount = 0;
            _adjacencyList = new Dictionary<T, DLinkedList<T>>((int)initialCapacity);
        }
        protected virtual bool _doesEdgeExist(T vertex1, T vertex2)
        {
            return (_adjacencyList[vertex1].Contains(vertex2));
        }
        public virtual bool IsDirected
        {
            get { return true; }
        }
        public virtual bool IsWeighted
        {
            get { return false; }
        }
        public virtual int VerticesCount
        {
            get { return _adjacencyList.Count; }
        }
        public virtual int EdgesCount
        {
            get { return _edgesCount; }
        }
        public virtual IEnumerable<T> Vertices
        {
            get
            {
                foreach (var vertex in _adjacencyList)
                    yield return vertex.Key;
            }
        }


        IEnumerable<IEdge<T>> IGraph<T>.Edges
        {
            get { return this.Edges; }
        }

        IEnumerable<IEdge<T>> IGraph<T>.IncomingEdges(T vertex)
        {
            return this.IncomingEdges(vertex);
        }

        IEnumerable<IEdge<T>> IGraph<T>.OutgoingEdges(T vertex)
        {
            return this.OutgoingEdges(vertex);
        }
        public virtual IEnumerable<UnweightedEdge<T>> Edges
        {
            get
            {
                foreach (var vertex in _adjacencyList)
                    foreach (var adjacent in vertex.Value)
                        yield return (new UnweightedEdge<T>(
                            vertex.Key,   
                            adjacent   
                        ));
            }
        }
        public virtual IEnumerable<UnweightedEdge<T>> IncomingEdges(T vertex)
        {
            if (!HasVertex(vertex))
                throw new KeyNotFoundException("Vertex doesn't belong to graph.");
            
            foreach(var adjacent in _adjacencyList.Keys)
            {
                if (_adjacencyList[adjacent].Contains(vertex))
                    yield return (new UnweightedEdge<T>(
                        adjacent,   // from
                        vertex      // to
                    ));
            }
        }
        public virtual IEnumerable<UnweightedEdge<T>> OutgoingEdges(T vertex)
        {
            if (!HasVertex(vertex))
                throw new KeyNotFoundException("Vertex doesn't belong to graph.");

            foreach(var adjacent in _adjacencyList[vertex])
                yield return (new UnweightedEdge<T>(
                    vertex,     
                    adjacent    
                ));
        }


        /// <summary>
        /// Connects two vertices together in the direction: first->second.
        /// </summary>
        public virtual bool AddEdge(T source, T destination)
        {
            // Check existence of nodes and non-existence of edge
            if (!HasVertex(source) || !HasVertex(destination))
                return false;
            if (_doesEdgeExist(source, destination))
                return false;

            // Add edge from source to destination
            _adjacencyList[source].Append(destination);

            // Increment edges count
            ++_edgesCount;

            return true;
        }
        public virtual bool RemoveEdge(T source, T destination)
        {
            if (!HasVertex(source) || !HasVertex(destination))
                return false;
            if (!_doesEdgeExist(source, destination))
                return false;
            _adjacencyList[source].Remove(destination);
            --_edgesCount;

            return true;
        }
        public virtual void AddVertices(IList<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException();

            foreach (var vertex in collection)
                AddVertex(vertex);
        }
        public virtual bool AddVertex(T vertex)
        {
            if (HasVertex(vertex))
                return false;

            if (_adjacencyList.Count == 0)
                _firstInsertedNode = vertex;

            _adjacencyList.Add(vertex, new DLinkedList<T>());

            return true;
        }
        public virtual bool RemoveVertex(T vertex)
        {
            // Check existence of vertex
            if (!HasVertex(vertex))
                return false;

            // Subtract the number of edges for this vertex from the total edges count
            _edgesCount = _edgesCount - _adjacencyList[vertex].Count;

            // Remove vertex from graph
            _adjacencyList.Remove(vertex);

            // Remove destination edges to this vertex
            foreach (var adjacent in _adjacencyList)
            {
                if (adjacent.Value.Contains(vertex))
                {
                    adjacent.Value.Remove(vertex);

                    // Decrement the edges count.
                    --_edgesCount;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks whether there is an edge from source to destination.
        /// </summary>
        public virtual bool HasEdge(T source, T destination)
        {
            return (_adjacencyList.ContainsKey(source) && _adjacencyList.ContainsKey(destination) && _doesEdgeExist(source, destination));
        }
        public virtual bool HasVertex(T vertex)
        {
            return _adjacencyList.ContainsKey(vertex);
        }
        public virtual DLinkedList<T> Neighbours(T vertex)
        {
            if (!HasVertex(vertex))
                return null;

            return _adjacencyList[vertex];
        }
        public virtual int Degree(T vertex)
        {
            if (!HasVertex(vertex))
                throw new KeyNotFoundException();

            return _adjacencyList[vertex].Count;
        }
        public virtual string ToReadable()
        {
            string output = string.Empty;

            foreach (var node in _adjacencyList)
            {
                var adjacents = string.Empty;

                output = String.Format("{0}\r\n{1}: [", output, node.Key);

                foreach (var adjacentNode in node.Value)
                    adjacents = String.Format("{0}{1},", adjacents, adjacentNode);

                if (adjacents.Length > 0)
                    adjacents = adjacents.TrimEnd(new char[] { ',', ' ' });

                output = String.Format("{0}{1}]", output, adjacents);
            }

            return output;
        }
        public virtual IEnumerable<T> DepthFirstWalk()
        {
            return DepthFirstWalk(_firstInsertedNode);
        }
        public virtual IEnumerable<T> DepthFirstWalk(T source)
        {
            // Check for existence of source
            if (VerticesCount == 0)
                return new ArrayList<T>(0);
            if (!HasVertex(source))
                throw new KeyNotFoundException("The source vertex doesn't exist.");

            var visited = new HashSet<T>();
            var stack = new Stack<T>();
            var listOfNodes = new ArrayList<T>(VerticesCount);

            stack.Push(source);

            while (!stack.IsEmpty)
            {
                var current = stack.Pop();

                if (!visited.Contains(current))
                {
                    listOfNodes.Add(current);
                    visited.Add(current);

                    foreach (var adjacent in Neighbours(current))
                        if (!visited.Contains(adjacent))
                            stack.Push(adjacent);
                }
            }

            return listOfNodes;
        }
        public virtual void Clear()
        {
            _edgesCount = 0;
            _adjacencyList.Clear();
        }

    }

}
