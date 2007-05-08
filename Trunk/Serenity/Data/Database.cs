/*
Serenity - The next evolution of web server technology
Serenity/Data/Database.cs
Copyright © 2006-2007 Serenity Project (http://serenityproject.net/)

This file is protected by the terms and conditions of the
Microsoft Community License (Ms-CL), a copy of which should
have been distributed along with this software. If not,
you may find the license information at the following URL:

http://www.microsoft.com/resources/sharedsource/licensingbasics/communitylicense.mspx
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Serenity.Data
{
    //WS: Everything goes in this file for now, because there will probably be
    // a lot of name changes of classes and such and doing source control renames
    // is a hassle for the most part.

    public class Row
    {
        public void Populate(params string[] values)
        {

        }
    }

    public class Query
    {

    }
    public class QueryCondition
    {
        private QueryConditionOperator conditionOperator;

        public QueryConditionOperator ConditionOperator
        {
            get
            {
                return this.conditionOperator;
            }
        }

        private string leftOperand;

        public string LeftOperand
        {
            get
            {
                return leftOperand;
            }
            set
            {
                leftOperand = value;
            }
        }
        private string rightOperand;

        public string RightOperand
        {
            get
            {
                return rightOperand;
            }
            set
            {
                rightOperand = value;
            }
        }
     
    }
    public enum QueryConditionOperator
    {
        EqualTo,
        NotEqualTo,
        Like,
        GreaterThan,
        LessThan,
    }
}
