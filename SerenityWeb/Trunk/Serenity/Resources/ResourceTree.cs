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

namespace Serenity.Resources
{
    /// <summary>
    /// Represents a tree of resources, and contains methods to add and remove them.
    /// </summary>
    public sealed class ResourceTree
    {
        #region Constructors - Public
        public ResourceTree()
        {
            this.root = new ResourceNode("/", null);
        }
        #endregion
        #region Fields - Private
        private ResourceTree fallback;
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
                throw new ArgumentException(__StringLiterals.MustBeDirectoryResource, "path");
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
            node.AddResource(resource);
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
                else if (this.HasFallback)
                {
                    return this.Fallback.GetNode(path);
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
            int depth = (path.IsDirectory) ? path.Depth : path.Depth - 1;
            if (depth > 0)
            {
                for (int i = 0; i < depth; i++)
                {
                    if (node.ContainsNode(path.Segments[i]))
                    {
                        node = node.GetNode(path.Segments[i]);
                    }
                    else if (this.HasFallback)
                    {
                        return this.fallback.GetResource(path);
                    }
                    else
                    {
                        throw new KeyNotFoundException();
                    }
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
        public IEnumerable<Resource> GetResources(ResourcePath path)
        {
            return this.GetResources(path, false);
        }
        public IEnumerable<Resource> GetResources(ResourcePath path, bool includeFallback)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            else if (!path.IsDirectory)
            {
                throw new ArgumentException(__StringLiterals.MustBeDirectoryResource);
            }

            yield break;
        }
        #endregion
        #region Properties - Public
        public ResourceTree Fallback
        {
            get
            {
                return this.fallback;
            }
            internal set
            {
                this.fallback = value;
            }
        }
        public bool HasFallback
        {
            get
            {
                return !(this.fallback == null);
            }
        }
        public ResourceNode Root
        {
            get
            {
                return this.root;
            }
        }
        #endregion
    }
}
