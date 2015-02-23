using System;

namespace Quarks
{
	public abstract class Disposable : IDisposable
	{
		/// <summary>
		/// Override this method to dispose of Managed resources only.
		/// This will be run once only when the IDisposable.Dispose() method is invoked.
		/// It will not be invoked when the garbage collector destroys the object so that managed objects that may have
		/// already been garbage-collected will not be accessed.
		/// </summary>
		protected virtual void DisposeManagedResources()
		{
		}

		/// <summary>
		/// Override this method to dispose of Unmanaged resources only.
		/// This will be run once when either IDisposable.Dispose is invoked or the garbage collector destorys the object.
		/// </summary>
		protected virtual void DisposeUnmanagedResources()
		{
		}

		# region IDisposable Implementation

		private bool _disposed;

		protected bool Disposed
		{
			get { return _disposed; }
		}

		// Implement IDisposable.
		// Do not make this method virtual.
		// A derived class should not be able to override this method.
		public void Dispose()
		{
			dispose(true);
			// This object will be cleaned up by the Dispose method.
			// Therefore, you should call GC.SupressFinalize to
			// take this object off the finalization queue 
			// and prevent finalization code for this object
			// from executing a second time.
			GC.SuppressFinalize(this);
		}

		// Dispose(bool disposing) executes in two distinct scenarios.
		// If disposing equals true, the method has been called directly
		// or indirectly by a user's code. Managed and unmanaged resources
		// can be disposed.
		// If disposing equals false, the method has been called by the 
		// runtime from inside the finalizer and you should not reference 
		// other objects. Only unmanaged resources can be disposed.
		private void dispose(bool disposing)
		{
			// Check to see if Dispose has already been called.
			if (!_disposed)
			{
				// If disposing equals true, dispose all managed 
				// and unmanaged resources.
				if (disposing)
					DisposeManagedResources();

				DisposeUnmanagedResources();
			}
			_disposed = true;
		}

		// Use C# destructor syntax for finalization code.
		// This destructor will run only if the Dispose method 
		// does not get called.
		// It gives your base class the opportunity to finalize.
		// Do not provide destructors in types derived from this class.
		~Disposable()
		{
			// Do not re-create Dispose clean-up code here.
			// Calling Dispose(false) is optimal in terms of
			// readability and maintainability.
			dispose(false);
		}

		# endregion
	}
}
