using System;
using Caliburn.Micro;
using ReactiveUI;
using IScreen = Caliburn.Micro.IScreen;

namespace Quincy.ReactiveCaliburn
{
    /// <summary>
    ///     A base implementation of <see cref="ReactiveUI.IScreen" />.
    /// </summary>
    public class ReactiveScreen : ReactiveViewAware, IScreen, IChild
    {
        private static readonly ILog Log = LogManager.GetLog(typeof(ReactiveScreen));
        private string displayName;

        private bool isActive;
        private bool isInitialized;
        private object parent;

        /// <summary>
        ///     Creates an instance of <see cref="ReactiveScreen" />.
        /// </summary>
        public ReactiveScreen()
        {
            displayName = GetType().FullName;
        }

        /// <summary>
        ///     Indicates whether or not this instance is currently initialized.
        ///     Virtualized in order to help with document oriented view models.
        /// </summary>
        public virtual bool IsInitialized
        {
            get { return isInitialized; }
            private set { this.RaiseAndSetIfChanged(ref isInitialized, value); }
        }

        /// <summary>
        ///     Gets or Sets the Parent <see cref="IConductor" />.
        /// </summary>
        public virtual object Parent
        {
            get { return parent; }
            set { this.RaiseAndSetIfChanged(ref parent, value); }
        }

        /// <summary>
        ///     Gets or Sets the Display Name.
        /// </summary>
        public virtual string DisplayName
        {
            get { return displayName; }
            set { this.RaiseAndSetIfChanged(ref displayName, value); }
        }

        /// <summary>
        ///     Indicates whether or not this instance is currently active.
        ///     Virtualized in order to help with document oriented view models.
        /// </summary>
        public virtual bool IsActive
        {
            get { return isActive; }
            private set { this.RaiseAndSetIfChanged(ref isActive, value); }
        }

        /// <summary>
        ///     Raised after activation occurs.
        /// </summary>
        public virtual event EventHandler<ActivationEventArgs> Activated = delegate { };

        /// <summary>
        ///     Raised before deactivation.
        /// </summary>
        public virtual event EventHandler<DeactivationEventArgs> AttemptingDeactivation = delegate { };

        /// <summary>
        ///     Raised after deactivation.
        /// </summary>
        public virtual event EventHandler<DeactivationEventArgs> Deactivated = delegate { };

        void IActivate.Activate()
        {
            if (IsActive)
                return;

            var initialized = false;

            if (!IsInitialized)
            {
                IsInitialized = initialized = true;
                OnInitialize();
            }

            IsActive = true;
            Log.Info("Activating {0}.", this);
            OnActivate();

            var handler = Activated;
            handler?.Invoke(this, new ActivationEventArgs
            {
                WasInitialized = initialized
            });
        }

        void IDeactivate.Deactivate(bool close)
        {
            if (IsActive || IsInitialized && close)
            {
                var attemptingDeactivationHandler = AttemptingDeactivation;
                attemptingDeactivationHandler?.Invoke(this, new DeactivationEventArgs
                {
                    WasClosed = close
                });

                IsActive = false;
                Log.Info("Deactivating {0}.", this);
                OnDeactivate(close);

                var deactivatedHandler = Deactivated;
                deactivatedHandler?.Invoke(this, new DeactivationEventArgs
                {
                    WasClosed = close
                });

                if (close)
                {
                    Views.Clear();
                    Log.Info("Closed {0}.", this);
                }
            }
        }

        /// <summary>
        ///     Called to check whether or not this instance can close.
        /// </summary>
        /// <param name="callback">The implementor calls this action with the result of the close check.</param>
        public virtual void CanClose(Action<bool> callback)
        {
            callback(true);
        }

        /// <summary>
        ///     Tries to close this instance by asking its Parent to initiate shutdown or by asking its corresponding view to
        ///     close.
        ///     Also provides an opportunity to pass a dialog result to it's corresponding view.
        /// </summary>
        /// <param name="dialogResult">The dialog result.</param>
        public virtual void TryClose(bool? dialogResult = null)
        {
            PlatformProvider.Current.GetViewCloseAction(this, Views.Values, dialogResult).OnUIThread();
        }

        /// <summary>
        ///     Called when initializing.
        /// </summary>
        protected virtual void OnInitialize()
        {
        }

        /// <summary>
        ///     Called when activating.
        /// </summary>
        protected virtual void OnActivate()
        {
        }

        /// <summary>
        ///     Called when deactivating.
        /// </summary>
        /// <param name="close">Inidicates whether this instance will be closed.</param>
        protected virtual void OnDeactivate(bool close)
        {
        }
    }
}