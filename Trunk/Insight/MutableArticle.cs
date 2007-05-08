/*
Insight - The Intelligent Wiki Engine

Copyright � 2006-2007 Serenity Project (http://serenityproject.net/)

This file is protected by the terms and conditions of the
Microsoft Community License (Ms-CL), a copy of which should
have been distributed along with this software. If not,
you may find the license information at the following URL:

http://www.microsoft.com/resources/sharedsource/licensingbasics/communitylicense.mspx
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Insight
{
    internal abstract class MutableArticle : Article
    {
        protected internal MutableArticle()
        {
            this._Categories = new string[0];
        }
        private string[] _Categories;

        internal void AddCategory(string Category)
        {

        }
        internal void RemoveCategory(string Category)
        {

        }
        internal string[] Categories
        {
            get
            {
                return this._Categories;
            }
        }
    }
}
