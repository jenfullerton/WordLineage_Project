using System;
using System.Collections.Generic;

namespace WordLineage
{
    public class WordNode
    {
        #region Fields
        /// <summary>
        /// The name of the word contained by this WordNode.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// An description of the word contained by this WordNode.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// An list of tags to apply to the WordNode.
        /// </summary>
        public List<string> Tags { get; private set; }
        /// <summary>
        /// A list of definitions for the word contained by this WordNode.
        /// </summary>
        public List<string> Definition { get; private set; }
        /// <summary>
        /// A list of parent WordNodes for this WordNode.
        /// If it is empty, this WordNode is a root.
        /// </summary>
        public List<WordNode> Parents { get; private set; }
        /// <summary>
        /// A list of child WordNodes for this WordNode.
        /// If the list is empty, then this WordNode is a leaf.
        /// </summary>
        public List<WordNode> Children { get; private set; }
        #endregion

        #region Constructors
        // *** Constructors *** //
        /// <summary>
        /// A simple constructor for WordNode that only requires a Name.
        /// Users may optionally include a desription, a tag, and a definition.
        /// </summary>
        /// <param name="name">WordNode must have a name parameter in the form of a string.</param>
        /// <param name="definition">A string definition for the word in node. Defaults to empty string.</param>
        /// <param name="description">A string description of the word in node. Defaults to empty string.</param>
        /// <param name="tags">A string tag for the word in node. Defaults to empty string.</param>        
        public WordNode(string name, string definition = "", string description = "", 
            string tag = "", List<WordNode>? parents = null, List<WordNode>? children = null)
        {
            this.Name = name;
            this.Description = description;
            this.Definition = new List<string>();
            if(definition != "")
            {   //adds definition if it is not empty
                this.Definition.Add(definition);
            }
            this.Tags = new List<string>();
            if(tag != "")
            {   // adds tag if it is not empty
                this.Tags.Add(tag);
            }
            
            if(parents != null)
            {
                this.Parents = parents;
            } else
            {
                this.Parents = new List<WordNode>();
            }

            if(children != null)
            {
                this.Children = children;
            } else
            {
                this.Children = new List<WordNode>();
            }
        }

        /// <summary>
        /// Creates a node containing the name of the word, a list of definitions,
        /// a list of tags, and a string description.
        /// </summary>
        /// <param name="name">The name of the word contained by this node.</param>
        /// <param name="definition">A list of string definitions for the node. Empty strings will be removed.</param>        
        /// <param name="description">A string description of the name in WordNode.</param>
        /// <param name="tags">A list of string tags for this node. Empty strings will be removed.</param>
        public WordNode(string name, List<string>? definition = null, 
            string description = "", List<string>? tags = null,
            List<WordNode>? parents = null, List<WordNode>? children = null)
        {
            this.Name = name;
            this.Description = description;

            this.Definition = new List<string>();
            if (definition != null && definition.Count > 0)
            {
                foreach (string def in definition)
                {
                    if (def != "") { this.Definition.Add(def); }
                }
            }
            
            this.Tags = new List<string>();
            if(tags != null && tags.Count > 0)
            {
                foreach (string tag in tags)
                {
                    if (tag != "") { this.Tags.Add(tag); }
                }
            }

            if(parents != null && parents.Count > 0)
            {
                this.Parents = parents;
            } else
            {
                this.Parents = new List<WordNode>();
            }

            if (children != null && children.Count > 0)
            {
                this.Children = children;
            } else
            {
                this.Children = new List<WordNode>();
            }
            
        }
        #endregion

        #region Definition_Controls
        // *** Definition Controls *** //

        /// <summary>
        /// Returns the definition at the specified index.
        /// </summary>
        /// <param name="index">Index must be a valid index.</param>
        /// <returns>Returns the definition at specified index.
        /// Returns null if the index is out of bounds.</returns>
        public string? GetDefinition(int index)
        {
            if (index >= this.Definition.Count || index < 0)
            {
                return null;
            }
            else
            {
                return this.Definition[index];
            }
        }

        /// <summary>
        /// Returns the current Count for the definitions list.
        /// </summary>
        public int GetDefinitionCount()
        {
            return this.Definition.Count;
        }

        /// <summary>
        /// Appends a definition to the end of the definitions list.
        /// </summary>
        /// <param name="def">The new definition to add. Cannot be empty string.</param>
        public bool AddDefinition(string def)
        {
            if(def != "") 
            { 
                this.Definition.Add(def);
                return true;
            } else
            {
                return false;
            }
        }

        /// <summary>
        /// Appends a list of definitions to this Word Node.
        /// </summary>
        /// <param name="defs">A list of definition to add. Definitions that are empty strings are skipped.</param>
        /// <returns>Returns true if no attempts were made to add an empty string.
        /// Returns false if one or more definitions is the empty string.</returns>
        public bool AddDefinitions(List<string> defs)
        {
            bool noEmpties = true;
            foreach(string def in defs)
            {
                if (def != "")
                {
                    this.Definition.Add(def);
                } else
                {
                    noEmpties = false;
                }
            }
            return noEmpties;
        }

        /// <summary>
        /// Replaces the definition at index with a new definition.
        /// </summary>
        /// <param name="index">The index to replace. Must be a valid index.</param>
        /// <param name="newDef">The definition to replace the old one. Cannot be empty string.</param>
        /// <returns>Returns true if the definition was successfully replaced.
        /// Returns false if index is out of bounds or new definition is an empty string.</returns>
        public bool ReplaceDefinition(int index, string newDef)
        {
            if(index >= this.Definition.Count || index < 0 || newDef == "")
            {
                return false;
            }
            else
            {
                this.Definition.Insert(index+1, newDef);
                this.Definition.RemoveAt(index);
                return true;
            }
        }

        /// <summary>
        /// Removes the defintion at the specified index.
        /// </summary>
        /// <param name="index">Must be a valid index.</param>
        /// <returns>Returns true if the definition was successfully removed.
        /// Returns false if the index is out of range. (No change).</returns>
        public bool RemoveDefinition(int index)
        {
            if(index < 0 || index >= this.Definition.Count)
            {
                return false;
            }
            else
            {
                this.Definition.RemoveAt(index);
                return true;
            }
        }

        /// <summary>
        /// Clears all definitions from this WordNode.
        /// </summary>
        public void RemoveAllDefinitions()
        {
            this.Definition.Clear();
        }

        #endregion

        #region Description_Controls
        // *** Description Controls *** //
        // replace description somewhat unnecessary, can just use set

        /// <summary>
        /// Clears the description field by setting it to be an empty string.
        /// </summary>
        public void RemoveDescription()
        {
            this.Description = "";
        }

        #endregion

        #region Tag_Controls
        // *** Tag Controls *** //
        /// <summary>
        /// Returns a single tag at the specified index.
        /// </summary>
        /// <param name="index">Integer index of tag to be returned. </param>
        /// <returns>Returns the tag at the specified index.
        /// If the index is out of bounds, returns null.</returns>
        public string? GetTag(int index)
        {
            if(index >= this.Tags.Count || index < 0)
            {
                return null;
            } 
            else
            { 
                return this.Tags[index]; 
            }
        }

        /// <summary>
        /// Adds a single string tag to the list of Tags.
        /// </summary>
        /// <param name="tag">The string tag to be added. Cannot be empty string.</param>
        public void AddTag(string tag)
        {
            if(tag != "")
            {
                this.Tags.Add(tag);
            }
        }

        /// <summary>
        /// Appends a string List of tags to the list of tags.
        /// </summary>
        /// <param name="tags">This list of tags to be added.
        /// Empty strings will be removed before adding.</param>
        public void AddTags(List<string> tags)
        {
            foreach(string tag in tags)
            {
                if (tag != "") { this.Tags.Add(tag); }
            }
        }

        /// <summary>
        /// Replaces an old tag with a new one. The new tag keeps the original position. 
        /// If ReplaceTag cannot find the index or empty string is supplied, no change is made.</summary>
        /// <param name="tagIndex">The index of the tag to be replaced.</param>
        /// <param name="newTag">The text of the new tag. Cannot empty string.</param>
        /// <returns>Returns true if the tag was successfully replaced.
        /// Returns false if the index is invalid or the replacement tag is an empty string.</returns>
        public bool ReplaceTag(int tagIndex, string newTag)
        {
            // if index is out of bounds or newTag is empty, return false
            if(tagIndex < 0 || tagIndex >= this.Tags.Count || newTag == "") { return false; }

            this.Tags.Insert(tagIndex + 1, newTag);
            this.Tags.RemoveAt(tagIndex);
            return true; 
        }

        /// <summary>
        /// Removes the tag at the specified index.
        /// The index cannot be found, no change is made.
        /// </summary>
        /// <param name="tagIndex">The index of the tag to be removed.</param>
        /// <returns>Returns true if the tag was successfully removed.
        /// Returns false if the index cannot be found.</returns>
        public bool RemoveTag(int tagIndex)
        {
            // if index is out of bounds, return false
            if( tagIndex >= this.Tags.Count || tagIndex < 0 ) { return false; }
            
            this.Tags.RemoveAt(tagIndex);
            return true;
        }

        /// <summary>
        /// Removes all tags in this WordNode.
        /// </summary>
        public void RemoveAllTags()
        {
            this.Tags.Clear();
        }

        /// <summary>
        /// Returns the amount of Tags in this WordNode.
        /// </summary>
        public int GetTagCount()
        {
            return this.Tags.Count;
        }
        #endregion

        #region Parents_Controls
        /// <summary>
        /// Removes a parent from the parents list for this WordNode.
        /// </summary>
        /// <returns>Returns true if removal is successful, false otherwise.</returns>
        public bool RemoveParent(int index)
        {
            if (index < 0 || index >= this.Parents.Count)
            {
                return false;
            } else
            {
                this.Parents.RemoveAt(index);
                return true;
            }
        }

        public void RemoveAllParents()
        {
            this.Parents.Clear();
        }

        #endregion

        #region Children_Controls
        /// <summary>
        /// Removes a Child fromt the children list for this WordNode.
        /// </summary>
        /// <returns>Returns true if the child was successfully removed, false otherwise.</returns>
        public bool RemoveChild(int index)
        {
            if (index < 0 || index > this.Children.Count)
            {
                return false ;
            } else
            {
                this.Children.RemoveAt(index);
                return true;
            }
        }

        /// <summary>
        /// Clears the list of Children for this WordNode.
        /// </summary>
        public void RemoveAllChildren()
        {
            this.Children.Clear();
        }


        #endregion

        #region Utilities
        // *** Utilities *** //
        // toString returns the name of the WordNode
        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}