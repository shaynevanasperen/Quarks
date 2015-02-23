using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using NHibernate.Engine;
using NHibernate.Type;
using NHibernate.UserTypes;

namespace Quarks.NHibernate.UserTypes
{
	public class CompositeUserTypeColumn
	{
		public string Name { get; set; }
		public IType Type { get; set; }
	}

	[Serializable]
	public abstract class ImmutableCompositeUserTypeBase<T> : ICompositeUserType
	{
		/// <summary>
		/// A method that can override the default behavior to map properties. Handy if some kind of lookup to match
		/// the set of input values is how a new type is returned.
		/// </summary>
		/// <param name="values">The set of values corresponding to the <see cref="PropertyNames"/> expectations.</param>
		/// <returns>A <b>new</b> instance of the mapped type with values hydrated.</returns>
		public delegate T GetMap(object[] values);

		readonly IType[] _propertyTypes;
		readonly string[] _propertyNames;
		readonly GetMap _get;

		protected ImmutableCompositeUserTypeBase(IType[] propertyTypes, string[] propertyNames, GetMap getMap)
		{
			_propertyTypes = propertyTypes;
			_propertyNames = propertyNames;
			_get = getMap;
		}

		protected ImmutableCompositeUserTypeBase(IReadOnlyCollection<CompositeUserTypeColumn> columnDefinitions, GetMap getMap)
			: this(columnDefinitions.Select(x => x.Type).ToArray(), columnDefinitions.Select(x => x.Name).ToArray(), getMap)
		{
		}

		/// <summary>
		/// Delegate for mapping values being returned from record to the implementor's Type.
		/// </summary>
		/// <remarks>
		/// We can pass an anonymous method into the constructor of the subclass for this mapping method.
		/// Implementors must do two things during this method if it is used:
		/// <list type="bullet">
		/// <item>A new instance of the mapped type must be created and returned.</item>
		/// <item>The values passed in must hydrate this new instance.</item>
		/// </list>
		/// </remarks>
		public GetMap Get
		{
			get { return _get; }
		}

		public object GetPropertyValue(object component, int property)
		{
			var prop = typeof(T).GetProperty(PropertyNames[property],
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);

			var target = component;
			//Accomodate static properties (get/set accessors can be static)
			if (prop.GetGetMethod(true).IsStatic)
			{
				//NULL the target for static property handling
				target = null;
			}

			return prop.GetValue(target, null);
		}

		public void SetPropertyValue(object component, int property, object value)
		{
			throw new NotSupportedException(typeof(T) + " is an immutable object. SetPropertyValue isn't supported.");
		}

		public new bool Equals(object x, object y)
		{
			return Object.Equals(x, y);
		}

		public int GetHashCode(object x)
		{
			return x == null ? 0 : x.GetHashCode();
		}

		/// <summary>
		/// This first checks to see if an implementation of <see cref="GetMap"/> has been provided. If not,
		/// it attempts to create an instance of &lt;T&gt; with full constructor initialization. If this fails
		/// an error is thrown. <b>NOTE</b> that if your constructors are all <b>string</b> types, an error will not
		/// be thrown but a incomplete instance will be created. Don't rely on this and be sure that your type
		/// provides a full constructor that has the args that match those defined in the <see cref="PropertyNames"/>
		/// property.
		/// </summary>
		public virtual object NullSafeGet(IDataReader dr, string[] names, ISessionImplementor session, object owner)
		{
			if (dr == null) return null;

			var vals = new object[PropertyNames.Length];
			for (var i = 0; i < PropertyNames.Length; i++)
			{
				vals[i] = PropertyTypes[i].NullSafeGet(dr, names[i], session, owner);
			}

			if (Get != null)
				return Get(vals);

			try
			{
				return (T)Activator.CreateInstance(typeof(T), BindingFlags.NonPublic | BindingFlags.Static, null, vals, null);
			}
			catch (MissingMethodException)
			{
				throw new MissingMethodException(String.Format("Since a GetMap Method wasn't provided and a constructor with {0} " +
												 "args couldn't be found NHibernate couldn't map the returning values" +
												 "to create a new instance of type '{1}'.", vals.Length, typeof(T)));
			}
		}

		/// <summary>
		/// Write an instance of the mapped class to the input 'prepared statement'.
		/// </summary>
		/// <param name="value">The class being typed. (&lt;T&gt;)</param>
		/// <param name="index">The index to start the property mapping from.</param>
		public void NullSafeSet(IDbCommand cmd, object value, int index, bool[] settable, ISessionImplementor session)
		{
			if (value == null) return;

			var propIndexer = index;
			var target = value;
			for (var i = 0; i < PropertyNames.Length; i++)
			{
				var prop = typeof(T).GetProperty(PropertyNames[i]);
				//Accomodate static properties (get/set accessors can be static)
				if (prop.GetGetMethod(true).IsStatic)
					target = null;
				var propValue = prop.GetValue(target, null);
				PropertyTypes[i].NullSafeSet(cmd, propValue, propIndexer, session);
				propIndexer++;
			}
		}

		/// <summary>
		/// Objects are immutable, DeepCopy(value) always returns value.
		/// </summary>
		public object DeepCopy(object value)
		{
			return value;
		}

		public object Disassemble(object value, ISessionImplementor session)
		{
			return DeepCopy(value);
		}

		public object Assemble(object cached, ISessionImplementor session, object owner)
		{
			return DeepCopy(cached);
		}

		public object Replace(object original, object target, ISessionImplementor session, object owner)
		{
			return target;
		}

		/// <summary>
		/// An array of property names to map to the &lt;T&gt;. This should correspond to the column structure left-to-right.
		/// </summary>
		public string[] PropertyNames
		{
			get { return _propertyNames; }
		}

		/// <summary>
		/// The <see cref="IType"/> correspondents to the properties being mapped. use <b>NHibernateUtil.<i>x</i></b> static types
		/// for these.
		/// </summary>
		public IType[] PropertyTypes
		{
			get { return _propertyTypes; }
		}

		/// <summary>
		/// The <see cref="Type"/> of the class being mapped.
		/// </summary>
		public Type ReturnedClass
		{
			get { return typeof(T); }
		}

		/// <summary>
		/// Obviously, Value Objects should be false.
		/// </summary>
		public bool IsMutable
		{
			get { return false; }
		}
	}
}
