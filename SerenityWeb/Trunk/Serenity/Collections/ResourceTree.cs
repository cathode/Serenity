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
    public sealed class ResourceTree : ResourceNode
    {
        #region Methods - Public
        /// <summary>
        /// Initializes a new instance of the ResourceTree class.
        /// </summary>
        public ResourceTree()
            : base("/")
        {
        }
        #endregion
        #region Fields - Private
        #endregion
        #region Methods - Public
        public void Add(string basePath, Resource resource)
        {
            string[] segments = ResourceTree.GetPathSegments(basePath);
            if (segments.Length > 1)
            {
                ResourceNode node = this;
                foreach (string segment in segments)
                {
                    if (!node.ContainsNode(segment))
                    {
                        node.Add(new ResourceNode(segment));
                    }
                    node = node.GetNode(segment);
                }
                node.Add(resource);
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
            if (path.EndsWith("/"))
            {
                segments[segments.Length - 1] += "/";
            }
            return segments;
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
    }

}
