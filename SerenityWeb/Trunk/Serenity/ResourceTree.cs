/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Serenity
{
    /// <summary>
    /// Represents a tree of resources, and contains methods to add and remove them.
    /// </summary>
    public sealed class ResourceTree
    {
        #region Constructors - Public
        public ResourceTree()
        {
            this.root = new ResourceNode("", null);
        }
        #endregion
        #region Fields - Private
        private ResourceNode root;
        #endregion
        #region Methods - Public
        /// <summary>
        /// Adds a resource to the current ResourceTree. The resource will be added at the position
        /// specified by 'path'.
        /// </summary>
        /// <param name="path">The path to the node where 'resource' will be added.</param>
        /// <param name="resource">The resource to add.</param>
        public void Add(ResourcePath path, Resource resource)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            else if (!path.IsDirectory)
            {
                throw new ArgumentException("Argument 'path' must represent a directory path.", "path");
            }

            ResourceNode node = this.root;
            foreach (string segment in path)
            {
                if (!node.ContainsNode(segment))
                {
                    ResourceNode n2 = new ResourceNode(segment, node);
                    node.AddNode(n2);
                    node = n2;
                    continue;
                }
                else
                {
                    node = node.GetNode(segment);
                }
            }
            node.Add(resource);
        }
        public ResourceNode GetNode(string name)
        {
            return this.root.GetNode(name);
        }
        public ResourceNode GetNode(ResourcePath path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            ResourceNode node = this.root;
            for (int i = 0; i < path.Depth; i++)
            {
                if (node.ContainsNode(path.Segments[i]))
                {
                    node = node.GetNode(path.Segments[i]);
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
            return node;
        }
        public Resource GetResource(ResourcePath path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            
            ResourceNode node = this.root;
            for (int i = 0; i < path.Depth; i++)
            {
                if (node.ContainsNode(path.Segments[i]))
                {
                    node = node.GetNode(path.Segments[i]);
                }
                else
                {
                    throw new KeyNotFoundException();
                }
            }
            if (path.IsDirectory)
            {
                return node.DirectoryResource;
            }
            else
            {
                return node.GetResource(path.Segments[path.Segments.Length - 1]);
            }
        }
        #endregion
    }
}
