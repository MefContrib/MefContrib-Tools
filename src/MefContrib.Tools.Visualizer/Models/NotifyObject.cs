namespace MefContrib.Tools.Visualizer.Models
{
	using System;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Linq.Expressions;
	using System.Reflection;

	/// <summary>
	/// Implements the INotifyPropertyChanged interface and provides
	/// a RaisePropertyChanged method for derived classes to use.
	/// </summary>
	public abstract class NotifyObject : INotifyPropertyChanged
	{
		#region Events
		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged
		{
			add { this._propertyChanged += value; }
			remove { this._propertyChanged -= value; }
		}

		/// <summary>
		/// The private event.
		/// </summary>
		private event PropertyChangedEventHandler _propertyChanged = delegate { };
		#endregion

		#region Methods
		/// <summary>
		/// Raises the property changed event for the given property.
		/// </summary>
		/// <typeparam name="TProperty">The type of the property.</typeparam>
		/// <param name="property">The property expression.</param>
		protected virtual void RaisePropertyChanged<TProperty>(Expression<Func<TProperty>> property)
		{
			RaisePropertyChanged(GetMemberInfo(property).Name);
		}

		/// <summary>
		/// Raises the property changed event for the given property.
		/// </summary>
		/// <param name="property">The property that is raising the event.</param>
		private void RaisePropertyChanged(string property)
		{
			this.RaisePropertyChanged(property, true);
		}

		/// <summary>
		/// Raises the property changed event for the given property.
		/// </summary>
		/// <param name="property">The property that is raising the event.</param>
		/// <param name="verifyProperty">if set to <c>true</c> the property should be verified.</param>
		private void RaisePropertyChanged(string property, bool verifyProperty)
		{
			if (verifyProperty)
			{
				this.VerifyProperty(property);
			}

			var handler = this._propertyChanged;

			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(property));
			}
		}

		/// <summary>
		/// Verifies that the given property exists on the object.
		/// </summary>
		/// <param name="property">The property name to verify.</param>
		[Conditional("DEBUG")]
		private void VerifyProperty(string property)
		{
			var type = GetType();

			var propertyInfo = type.GetProperty(property);

			if (propertyInfo == null)
			{
				var message = String.Format(
					"'{0}' is not a property of {1}",
					property,
					type.FullName);

				throw new InvalidOperationException(message);
			}
		}

		/// <summary>
		/// Converts an expression into a <see cref="MemberInfo"/>.
		/// </summary>
		/// <param name="expression">The expression to convert.</param>
		/// <returns>The member info.</returns>
		/// <remarks>
		/// Originally developed by Rob Eisenberg.  http://caliburnmicro.codeplex.com/
		/// </remarks>
		private static MemberInfo GetMemberInfo(Expression expression)
		{
			var lambda = (LambdaExpression)expression;

			MemberExpression memberExpression;
			if (lambda.Body is UnaryExpression)
			{
				var unaryExpression = (UnaryExpression)lambda.Body;
				memberExpression = (MemberExpression)unaryExpression.Operand;
			}
			else memberExpression = (MemberExpression)lambda.Body;

			return memberExpression.Member;
		}
		#endregion
	}
}
