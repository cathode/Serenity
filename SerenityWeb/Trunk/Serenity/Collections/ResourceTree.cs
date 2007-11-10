/******************************************************************************
 * Serenity - The next evolution of web server technology.                    *
 * Copyright © 2006-2007 Serenity Project - http://SerenityProject.net/       *
 *----------------------------------------------------------------------------*
 * This software is released under the terms and conditions of the Microsoft  *
 * Public License (Ms-PL), a copy of which should have been included with     *
 * this distribution as License.txt.                                          *
 *****************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Serenity.Collections
{
    /// <summary>
    /// Represents a tree of resources.
    /// </summary>
    public sealed class ResourceTree
    {
        #region Methods - Public
        /// <summary>
        /// Initializes a new instance of the ResourceTree class.
        /// </summary>
        public ResourceTree()
        {
            this.trunk = new ResourceNode(this, null, "/");
            this.nodes.Add(this.trunk);
        }
        #endregion
        #region Fields - Private
        private ResourceNode trunk;
        private ResourceNodeCollection nodes = new ResourceNodeCollection(true);
        #endregion
        #region Methods - Public
        /// <summary>
        /// Adds a resource to the current ResourceTree.
        /// </summary>
        /// <param name="relativeUri"></param>
        /// <param name="resource"></param>
        public void Add(string basePath, Resource resource)
        {
            
        }
        public bool ContainsNode(string path)
        {
            path = ResourceTree.SanitizePath(path);

            return this.nodes.Contains(path);
        }
        public bool ContainsResource(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }
            return this.ContainsResource(uri.AbsolutePath);
        }
        public bool ContainsResource(string path)
        {
            path = ResourceTree.SanitizePath(path);

            if (path.EndsWith("/") && this.nodes.Contains(path))
            {
                return true;
            }
            string container = System.IO.Path.GetDirectoryName(path);
            if (this.nodes.Contains(container))
            {
                return false;
            }
            else
            {
                return false;
            }
        }
        public ResourceNode GetNode(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }
            return this.GetNode(uri.AbsolutePath);
        }
        public ResourceNode GetNode(string path)
        {
            path = ResourceTree.SanitizePath(path);

            if (this.nodes.Contains(path))
            {
                return this.nodes[path];
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }
        public static string GetParentPath(string path)
        {
            return ResourceTree.GetParentPathUnchecked(ResourceTree.SanitizePath(path));
        }
        public static string GetParentPathUnchecked(string path)
        {
            string[] segments = ResourceTree.GetPathSegmentsUnchecked(path);
            return string.Join("", segments, 0, segments.Length - 1);
        }
        public static string[] GetPathSegments(string path)
        {
            return ResourceTree.GetPathSegmentsUnchecked(ResourceTree.SanitizePath(path));
        }
        public static string[] GetPathSegmentsUnchecked(string path)
        {
            string[] segments = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            string[] newSegments = new string[segments.Length + 1];
            segments.CopyTo(newSegments, 1);
            for (int i = 0; i < newSegments.Length-1; i++)
            {
                newSegments[i] += "/";
            }
            if (path.EndsWith("/"))
            {
                newSegments[newSegments.Length - 1] += "/";
            }
            return newSegments;
        }
        public Resource GetResource(Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }
            return this.GetResource(uri.AbsolutePath);
        }
        public Resource GetResource(string path)
        {
            path = ResourceTree.SanitizePath(path);

            if (path.EndsWith("/") && this.nodes.Contains(path))
            {
                return this.nodes[path].DirectoryResource;
            }

            string container = System.IO.Path.GetDirectoryName(path);
            if (this.nodes.Contains(container))
            {
                return this.nodes[container].GetResource(System.IO.Path.GetFileName(path));
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }
        public static string SanitizePath(string path)
        {
            bool changed;
            return ResourceTree.SanitizePath(path, out changed);
        }
        public static string SanitizePath(string path, out bool changed)
        {
            string originalPath = path.Clone() as string;
            if (string.IsNullOrEmpty(path))
            {
                changed = true;
                return "/";
            }
            bool directory = path.EndsWith("/");
            path = path.Trim('/');
            Stack<string> stack = new Stack<string>();
            string[] parts = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string part in parts)
            {
                if (part == "..")
                {
                    if (stack.Count > 0)
                    {
                        stack.Pop();
                    }
                }
                else if (part != ".")
                {
                    stack.Push(part);
                }
            }
            parts = stack.ToArray();
            Array.Reverse(parts);
            string result = string.Join("/", parts);
            result = directory ? result + "/" : result;
            result = (result[0] != '/') ? "/" + result : result;

            if (originalPath == result)
            {
                changed = false;
            }
            else
            {
                changed = true;
            }
            return result;
        }
        #endregion
        #region Properties - Public
        public ResourceNode Trunk
        {
            get
            {
                return this.trunk;
            }
        }
        #endregion
    }

}
