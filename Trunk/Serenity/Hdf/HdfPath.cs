using System;
using System.Collections.Generic;
using System.Text;

namespace Serenity.Hdf
{
    /// <summary>
    /// Provides static methods for operating on HDF element paths.
    /// </summary>
    public static class HdfPath
    {
        #region Fields - Public
        /// <summary>
        /// Holds a string used as an empty path.
        /// </summary>
        public const string EmptyPath = "";
        /// <summary>
        /// Holds the char used as a separator between path segments.
        /// </summary>
        public const char Separator = '.';
        #endregion
        #region Methods - Public
        /// <summary>
        /// Combines a target path with one or more other paths.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static string Combine(string target, params string[] paths)
        {
            StringBuilder builder = new StringBuilder(target.Length + paths.Length);

            if (target.Contains(HdfPath.Separator.ToString()))
            {
                string[] parts = target.Split(HdfPath.Separator);
                if (parts.Length > 0)
                {
                    builder.Append(parts[0].Trim());
                    if (parts.Length > 1)
                    {
                        for (int i = 1; i < parts.Length; i++)
                        {
                            builder.Append(HdfPath.Separator);
                            builder.Append(parts[i].Trim());
                        }
                    }
                }
            }
            else
            {
                builder.Append(target.Trim());
            }

            foreach (string path in paths)
            {
                if (path.Contains(HdfPath.Separator.ToString()))
                {
                    string[] parts = path.Split(HdfPath.Separator);
                    foreach (string part in parts)
                    {
                        builder.Append(HdfPath.Separator);
                        builder.Append(part.Trim());
                    }
                }
                else
                {
                    builder.Append(HdfPath.Separator);
                    builder.Append(path.Trim());
                }
            }

            return builder.ToString().Trim(HdfPath.Separator);
        }
        /// <summary>
        /// Enumerates over each level of a supplied HDF element path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IEnumerable<string> EnumeratePath(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                string[] parts = path.Split(HdfPath.Separator);

                foreach (string part in parts)
                {
                    yield return part;
                }
            }
            else
            {
                yield break;
            }
        }
        /// <summary>
        /// Gets the "top-level" parent name of the supplied path string.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetAbsoluteParent(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                string[] parts = path.Split(HdfPath.Separator);
                if (parts.Length > 1)
                {
                    return parts[0];
                }
                else
                {
                    return HdfPath.EmptyPath;
                }
            }
            else
            {
                throw new ArgumentException("Must supply a non-empty path value");
            }
        }
        /// <summary>
        /// Gets the immediate parent name of the supplied path string.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetImmediateParent(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                string[] parts = path.Split(HdfPath.Separator);
                if (parts.Length > 1)
                {
                    string result = "";
                    for (int i = 0; i < parts.Length - 1; i++)
                    {
                        result = HdfPath.Combine(result, parts[i]);
                    }
                    return result;
                }
                else
                {
                    return HdfPath.EmptyPath;
                }
            }
            else
            {
                throw new ArgumentException("Must supply a non-empty path value");
            }
        }
        /// <summary>
        /// Gets the depth (number of path segments) in the supplied path string.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static int GetDepth(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                string[] parts = path.Split(HdfPath.Separator);
                return parts.Length;
            }
            else
            {
                throw new ArgumentException("Must supply a non-empty path value");
            }
        }
        public static string GetName(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                string[] parts = path.Split(HdfPath.Separator);

                if (parts.Length > 1)
                {
                    return parts[parts.Length - 1];
                }
                else
                {
                    return parts[0];
                }
            }
            else
            {
                throw new ArgumentException("Must supply a non-empty path value");
            }
        }
        public static bool HasParent(string path)
        {
            return (HdfPath.GetAbsoluteParent(path) == HdfPath.EmptyPath) ? false : true;
        }
        #endregion
    }
}
