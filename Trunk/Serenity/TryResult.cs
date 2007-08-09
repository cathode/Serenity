/*
Serenity - The next evolution of web server technology

Copyright © 2006-2007 Serenity Project (http://SerenityProject.net/)

This file is protected by the terms and conditions of the
Microsoft Community License (Ms-CL), a copy of which should
have been distributed along with this software. If not,
you may find the license information at the following URL:

http://www.microsoft.com/resources/sharedsource/licensingbasics/communitylicense.mspx
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace Serenity
{
	/// <summary>
	/// Represents a result of method which attempts an operation
	/// and returns the result of the operation and a boolean which
	/// indicates if the operation actually succeeded.
	/// </summary>
	/// <typeparam name="T">The type of the result.</typeparam>
	[Obsolete]
	public sealed class TryResult<T>
	{
		#region Constructors - Private
		private TryResult()
		{
		}
		#endregion
		#region Fields - Private
		private Exception exception;
		private T value;
		private bool isSuccessful;
		#endregion;
		#region Methods - Public
		public static TryResult<T> FailResult(Exception reason)
		{
			TryResult<T> result = new TryResult<T>();
			result.exception = reason;
			result.value = default(T);
			result.isSuccessful = false;

			return result;
		}
		/// <summary>
		/// Creates and returns a successful TryResult&lt;T&gt;
		/// </summary>
		/// <param name="value">The successful result of the operation.</param>
		/// <returns>A TryResult&lt;T&gt; representing the successful result of the operation.</returns>
		public static TryResult<T> SuccessResult(T value)
		{
			TryResult<T> result = new TryResult<T>();
			result.exception = null;
			result.value = value;
			result.isSuccessful = true;

			return result;
		}
		#endregion
		#region Properties - Public
		/// <summary>
		/// Gets a System.Exception which contains information about why the operation failed.
		/// </summary>
		/// <remarks>
		/// If the operation succeeded (the Success property returns true),
		/// this method will return false.
		/// </remarks>
		public Exception Exception
		{
			get
			{
				if (this.isSuccessful == false)
				{
					return this.exception;
				}
				else
				{
					return null;
				}
			}
		}
		/// <summary>
		/// Gets the result of the operation which returned the current
		/// TryResult&lt;T&gt;, or null if the operation did not succeed.
		/// </summary>
		public T Value
		{
			get
			{
				if (this.isSuccessful == true)
				{
					return this.value;
				}
				else
				{
					return default(T);
				}
			}
		}
		/// <summary>
		/// Gets a value indicating if the operation which returned the current
		/// TryResult&lt;T&gt; succeeded or failed.
		/// </summary>
		public bool IsSuccessful
		{
			get
			{
				return this.isSuccessful;
			}
		}

		#endregion
	}
}
