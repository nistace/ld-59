using System.Collections.Generic;
using UnityEngine;

namespace LD59.ExtractMoles.Signals
{
   public static class SignalSystem
   {
      private static HashSet<ISignalListener> _registeredSignalListeners = new();

      public static void Register( ISignalListener listener ) => _registeredSignalListeners.Add( listener );
      public static void Unregister( ISignalListener listener ) => _registeredSignalListeners.Remove( listener );

      public static void EmitSignal()
      {
         foreach(var listener in _registeredSignalListeners)
         {
            listener.ReactToSignal();
         }
      }
   }
}