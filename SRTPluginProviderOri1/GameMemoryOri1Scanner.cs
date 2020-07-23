using ProcessMemory;
using System;
using System.Diagnostics;

namespace SRTPluginProviderOri1
{
    internal class GameMemoryOri1Scanner : IDisposable
    {
        // Variables
        private ProcessMemory.ProcessMemory memoryAccess;
        private GameMemoryOri1 gameMemoryValues;
        public bool HasScanned;
        public bool ProcessRunning => memoryAccess != null && memoryAccess.ProcessRunning;
        public int ProcessExitCode => (memoryAccess != null) ? memoryAccess.ProcessExitCode : 0;

        // Pointer Address Variables
        private long PointerAddressSecretAreas;

        // Pointer Classes
        private long BaseAddress { get; set; }
        private MultilevelPointer PointerSecretAreas { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proc"></param>
        internal GameMemoryOri1Scanner(Process process = null)
        {
            gameMemoryValues = new GameMemoryOri1();
            if (process != null)
                Initialize(process);

            // Setup the pointers.

        }

        internal void Initialize(Process process)
        {
            if (process == null)
                return; // Do not continue if this is null.

            SelectPointerAddresses();

            int pid = GetProcessId(process).Value;
            memoryAccess = new ProcessMemory.ProcessMemory(pid);

            if (ProcessRunning)
            {
                BaseAddress = NativeWrappers.GetProcessBaseAddress(pid, PInvoke.ListModules.LIST_MODULES_64BIT).ToInt64(); // Bypass .NET's managed solution for getting this and attempt to get this info ourselves via PInvoke since some users are getting 299 PARTIAL COPY when they seemingly shouldn't.

                PointerSecretAreas = new MultilevelPointer(memoryAccess, BaseAddress + PointerAddressSecretAreas, 0x468L);
            }
        }

        private void SelectPointerAddresses()
        {
            PointerAddressSecretAreas = 0x00F26C58;
        }


        /// <summary>
        /// 
        /// </summary>
        internal void UpdatePointers()
        {
            PointerSecretAreas.UpdatePointers();
        }

        internal IGameMemoryOri1 Refresh()
        {
            // Secret Areas Found 43 Total
            gameMemoryValues.SecretAreas = PointerSecretAreas.DerefInt(0x120);
            HasScanned = true;
            return gameMemoryValues;
        }

        private int? GetProcessId(Process process) => process?.Id;

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if (memoryAccess != null)
                        memoryAccess.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~REmake1Memory() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
