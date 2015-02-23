using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Quarks
{
	static class FileStore
	{
		/// <summary>
		/// Deserializes a data file into an object of type {T}.
		/// </summary>
		/// <typeparam name="T">Type of object to deserialize and return.</typeparam>
		/// <param name="path">Full path to file containing serialized data.</param>
		/// <returns>If object is successfully deserialized, the object of type {T}, 
		/// otherwise null.</returns>
		/// <exception cref="ArgumentNullException">Thrown if the path parameter is null or empty.</exception>
		internal static T Load<T>(string path) where T : class
		{
			if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("Invalid path", "path");

			try
			{
				using (var file = File.Open(path, FileMode.Open))
				{
					return new BinaryFormatter().Deserialize(file) as T;
				}
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// Serialize the given object of type {T} to a file at the given path.
		/// </summary>
		/// <typeparam name="T">Type of object to serialize.</typeparam>
		/// <param name="obj">Object to serialize and store in a file.</param>
		/// <param name="path">Full path of file to store the serialized data.</param>
		/// <exception cref="ArgumentNullException">Thrown if obj or path parameters are null.</exception>
		internal static void Save<T>(T obj, string path) where T : class
		{
			if (obj == null) throw new ArgumentNullException("obj", "Object cannot be null");
			if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("Invalid path", "path");

			using (var file = File.Open(path, FileMode.Create))
			{
				new BinaryFormatter().Serialize(file, obj);
			}
		}
	}
}
