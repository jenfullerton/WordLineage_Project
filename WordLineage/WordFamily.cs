﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WordLineage
{
	public class WordFamily
	{
        #region Fields
        /// <summary>
        /// The name of the WordFamily.
        /// </summary>
        public string Name;
        /// <summary>
        /// The list of all WordNodes in this WordFamily in topological order.
        /// </summary>
        /// <remarks>The list contains a single copy of each word. 
        ///     The list enforces topological order (i.e. each node
        ///     precedes the next in terms of lineage. Roots are found
        ///     at the head of the list; there can be multiple roots.
        /// </remarks>
		public ObservableCollection<WordNode> T_Nodes;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new DAG to contain a family of WordNodes.
        /// </summary>
        /// <param name="name">The name of this word family. Defaults to "Word Family 1."</param>
        public WordFamily(string name = "Word Family 1")
		{
			this.Name = name;
			this.T_Nodes = new();
		}
        #endregion

        /// <summary>
        /// Adds a new word to the WordFamily.
        /// </summary>
        public void AddWord(WordNode word)
        {
            if (this.T_Nodes.Contains(word))
            {   // change later to checking words themselves, OR have
                // a "block duplicates" option for users
                return;
            }
            else
            {
                this.T_Nodes.Add(word);
            }
        }

        /// <summary>
        /// Adds multiple words to this WordFamily.
        /// </summary>
        /// <param name="words"></param>
        public void AddWords(params WordNode[] words)
        {
            foreach (WordNode word in words)
            {
                this.AddWord(word);
            }
        }

        /// <summary>
        /// Sorts this family's T_Nodes into toplological order.
        /// </summary>
        /// <returns>
        /// Returns null upon succesful sort.
        /// If the sort fails, returns a list of Nodes that caused a cycle. No change is made.
        /// </returns>
        /// <remarks>
        /// Adapted from example on interviewcake https://www.interviewcake.com/concept/java/topological-sort
        /// </remarks>
        public List<WordNode>? Sort()
        {
            // Dictionary tracks the remaining indegrees for each node
            Dictionary<WordNode, int> indegrees = new(T_Nodes.Count);

            // Queue tracks nodes with indegree of zero for processing
            Queue<WordNode> nodes_with_indegree_zero = new();
            
            // collect indegree (# of parents) of each node; push nodes with indegree zero into the queue
            foreach (WordNode node in T_Nodes)
            {
                indegrees[node] = node.Parents.Count;
                if(node.Parents.Count == 0)
                {
                    nodes_with_indegree_zero.Enqueue(node);
                }
            }

            // create a new list to store the sorted nodes
            ObservableCollection<WordNode> topological_ordering = new();
            
            // begin the sorting process
            // the sort will continue until there are no more nodes left to process
            // OR if a cycle is found (in which case it will break and return problem nodes
            while(nodes_with_indegree_zero.Count > 0)
            {
                WordNode node = nodes_with_indegree_zero.Dequeue();
                topological_ordering.Add(node);

                // go through each child of this node 
                foreach (WordNode child in node.Children)
                {
                    // decrement the number of remaining indegrees for each child of the parent node that has been removed.
                    indegrees[child]--;
                    if(indegrees[child] == 0)
                    {
                        // if any of the children have all parents removed, add them to the processing queue.
                        nodes_with_indegree_zero.Enqueue(child);
                    }
                }
            }

            // final checks!
            // if the new length of the list matches the the old length, sorting is successful
            if( topological_ordering.Count == T_Nodes.Count)
            {
                // set the old list equal to the new ordered list (return null)
                T_Nodes = topological_ordering;
                return null;
            } else
            {
                // graph has a cycle, return bad nodes
                // go through each dictionary entry and find who has it
                // return list of problem nodes
                List<WordNode> problem_nodes = new();

                foreach(WordNode n in T_Nodes)
                {
                    if( indegrees[n] > 0)
                    {
                        problem_nodes.Add(n);
                    }
                }
                return problem_nodes;
            }
        }

        /// <summary>
        /// Returns the string Name of this WordFamily.
        /// </summary>
        /// <returns>The string Name of this WordFamily.</returns>
        public override string ToString()
        {
            return this.Name;
        }
    }

}