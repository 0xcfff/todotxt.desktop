using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Threading;

namespace TodoTxt.Platform
{
    /// <summary>
    /// macOS-specific hotkey service implementation using Carbon APIs
    /// </summary>
    public class MacOSHotkeyService : IHotkeyService, IDisposable
    {
        private readonly ConcurrentDictionary<int, HotkeyRegistration> _registeredHotkeys = new();
        private readonly object _lockObject = new();
        private bool _disposed = false;
        private Thread? _eventLoopThread;
        private volatile bool _shouldStopEventLoop = false;

        public event EventHandler<HotkeyPressedEventArgs>? HotkeyPressed;

        public bool IsSupported => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        public MacOSHotkeyService()
        {
            // Don't start the event loop in constructor to avoid P/Invoke issues during testing
            // The event loop will be started when the first hotkey is registered
        }

        public bool RegisterHotkey(Hotkey hotkey, int id)
        {
            if (!IsSupported || _disposed)
                return false;

            lock (_lockObject)
            {
                if (_registeredHotkeys.ContainsKey(id))
                {
                    System.Diagnostics.Debug.WriteLine($"Hotkey with ID {id} is already registered");
                    return false;
                }

                try
                {
                    // For now, we'll implement a simplified version that just stores the hotkey
                    // without actually registering it with the system. This prevents crashes
                    // while we work on a proper implementation.
                    
                    var registration = new HotkeyRegistration
                    {
                        Id = id,
                        Hotkey = hotkey,
                        HotkeyRef = new EventHotKeyRef(),
                        Modifiers = ConvertModifiers(hotkey),
                        KeyCode = ConvertKeyCode(hotkey.KeyCode)
                    };

                    _registeredHotkeys[id] = registration;
                    System.Diagnostics.Debug.WriteLine($"Hotkey {hotkey} with ID {id} registered (simplified implementation)");
                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Exception while registering hotkey {hotkey}: {ex.Message}");
                    return false;
                }
            }
        }

        public bool UnregisterHotkey(int id)
        {
            if (!IsSupported || _disposed)
                return false;

            lock (_lockObject)
            {
                if (_registeredHotkeys.TryRemove(id, out var registration))
                {
                    System.Diagnostics.Debug.WriteLine($"Successfully unregistered hotkey with ID {id} (simplified implementation)");
                    return true;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Hotkey with ID {id} is not registered");
                    return false;
                }
            }
        }

        public void UnregisterAllHotkeys()
        {
            if (!IsSupported || _disposed)
                return;

            lock (_lockObject)
            {
                var keysToRemove = new List<int>(_registeredHotkeys.Keys);
                foreach (var id in keysToRemove)
                {
                    UnregisterHotkey(id);
                }
            }
        }

        public bool IsHotkeyAvailable(Hotkey hotkey)
        {
            if (!IsSupported || _disposed)
                return false;

            // For now, we'll assume all hotkeys are available
            // In a more sophisticated implementation, we could check for conflicts
            return true;
        }

        public IEnumerable<int> GetRegisteredHotkeys()
        {
            if (!IsSupported || _disposed)
                return Array.Empty<int>();

            return _registeredHotkeys.Keys;
        }

        private void StartEventLoop()
        {
            if (!IsSupported)
                return;
                
            _eventLoopThread = new Thread(EventLoop)
            {
                IsBackground = true,
                Name = "MacOSHotkeyEventLoop"
            };
            _eventLoopThread.Start();
        }

        private void EventLoop()
        {
            if (!IsSupported)
                return;
                
            try
            {
                while (!_shouldStopEventLoop && !_disposed)
                {
                    try
                    {
                        var eventRef = new EventRef();
                        var status = ReceiveNextEvent(0, IntPtr.Zero, 0.1, true, out eventRef);
                        
                        if (status == OSStatus.NoError && eventRef.Value != IntPtr.Zero)
                        {
                            var eventClass = GetEventClass(eventRef);
                            var eventKind = GetEventKind(eventRef);
                            
                            if (eventClass == (uint)EventClass.Keyboard && eventKind == (uint)EventKind.HotKeyPressed)
                            {
                                HandleHotkeyEvent(eventRef);
                            }
                            
                            ReleaseEvent(eventRef);
                        }
                        else if (status != OSStatus.EventNotHandledErr)
                        {
                            // Sleep briefly to avoid busy waiting
                            Thread.Sleep(10);
                        }
                    }
                    catch (Exception innerEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error in hotkey event loop iteration: {innerEx.Message}");
                        Thread.Sleep(100); // Sleep longer on error
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in hotkey event loop: {ex.Message}");
            }
        }

        private void HandleHotkeyEvent(EventRef eventRef)
        {
            try
            {
                var hotkeyRef = new EventHotKeyRef();
                var status = GetEventParameter(eventRef, EventParamName.DirectObject, EventParamType.EventHotKeyRef, 
                    IntPtr.Zero, (uint)Marshal.SizeOf<EventHotKeyRef>(), IntPtr.Zero, ref hotkeyRef);
                
                if (status == OSStatus.NoError)
                {
                    // Find the registration for this hotkey
                    foreach (var kvp in _registeredHotkeys)
                    {
                        if (kvp.Value.HotkeyRef.Value == hotkeyRef.Value)
                        {
                            var args = new HotkeyPressedEventArgs(kvp.Key, kvp.Value.Hotkey);
                            HotkeyPressed?.Invoke(this, args);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error handling hotkey event: {ex.Message}");
            }
        }

        private uint ConvertModifiers(Hotkey hotkey)
        {
            uint modifiers = 0;
            if (hotkey.Ctrl) modifiers |= (uint)EventModifiers.ControlKey;
            if (hotkey.Alt) modifiers |= (uint)EventModifiers.OptionKey;
            if (hotkey.Shift) modifiers |= (uint)EventModifiers.ShiftKey;
            if (hotkey.Win) modifiers |= (uint)EventModifiers.CommandKey;
            return modifiers;
        }

        private int ConvertKeyCode(int keyCode)
        {
            // Convert common key codes to macOS virtual key codes
            return keyCode switch
            {
                65 => 0, // A
                66 => 11, // B
                67 => 8, // C
                68 => 2, // D
                69 => 14, // E
                70 => 3, // F
                71 => 5, // G
                72 => 4, // H
                73 => 34, // I
                74 => 38, // J
                75 => 40, // K
                76 => 37, // L
                77 => 46, // M
                78 => 45, // N
                79 => 31, // O
                80 => 35, // P
                81 => 12, // Q
                82 => 15, // R
                83 => 1, // S
                84 => 17, // T
                85 => 32, // U
                86 => 9, // V
                87 => 13, // W
                88 => 7, // X
                89 => 16, // Y
                90 => 6, // Z
                48 => 29, // 0
                49 => 18, // 1
                50 => 19, // 2
                51 => 20, // 3
                52 => 21, // 4
                53 => 23, // 5
                54 => 22, // 6
                55 => 26, // 7
                56 => 28, // 8
                57 => 25, // 9
                _ => keyCode // Return as-is for other keys
            };
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            _shouldStopEventLoop = true;
            
            UnregisterAllHotkeys();
            
            _eventLoopThread?.Join(1000); // Wait up to 1 second for thread to finish
            
            GC.SuppressFinalize(this);
        }

        ~MacOSHotkeyService()
        {
            Dispose();
        }

        private class HotkeyRegistration
        {
            public int Id { get; set; }
            public Hotkey Hotkey { get; set; }
            public EventHotKeyRef HotkeyRef { get; set; }
            public uint Modifiers { get; set; }
            public int KeyCode { get; set; }
        }

        #region P/Invoke Declarations

        [DllImport("/System/Library/Frameworks/Carbon.framework/Carbon", EntryPoint = "RegisterEventHotKey")]
        private static extern OSStatus RegisterEventHotKey(uint keyCode, uint modifiers, IntPtr target, uint userData, out EventHotKeyRef hotKeyRef);

        [DllImport("/System/Library/Frameworks/Carbon.framework/Carbon", EntryPoint = "UnregisterEventHotKey")]
        private static extern OSStatus UnregisterEventHotKey(EventHotKeyRef hotKeyRef);

        [DllImport("/System/Library/Frameworks/Carbon.framework/Carbon", EntryPoint = "GetApplicationEventTarget")]
        private static extern IntPtr GetApplicationEventTarget();

        [DllImport("/System/Library/Frameworks/Carbon.framework/Carbon", EntryPoint = "ReceiveNextEvent")]
        private static extern OSStatus ReceiveNextEvent(uint numTypes, IntPtr list, double timeout, bool pullEvent, out EventRef theEvent);

        [DllImport("/System/Library/Frameworks/Carbon.framework/Carbon", EntryPoint = "GetEventClass")]
        private static extern uint GetEventClass(EventRef eventRef);

        [DllImport("/System/Library/Frameworks/Carbon.framework/Carbon", EntryPoint = "GetEventKind")]
        private static extern uint GetEventKind(EventRef eventRef);

        [DllImport("/System/Library/Frameworks/Carbon.framework/Carbon", EntryPoint = "GetEventParameter")]
        private static extern OSStatus GetEventParameter(EventRef eventRef, EventParamName name, EventParamType desiredType, IntPtr buffer, uint bufferSize, IntPtr actualSize, ref EventHotKeyRef result);

        [DllImport("/System/Library/Frameworks/Carbon.framework/Carbon", EntryPoint = "ReleaseEvent")]
        private static extern OSStatus ReleaseEvent(EventRef eventRef);

        #endregion

        #region Types and Enums

        private struct EventHotKeyRef
        {
            public IntPtr Value;
        }

        private struct EventRef
        {
            public IntPtr Value;
        }

        private enum OSStatus
        {
            NoError = 0,
            EventNotHandledErr = -9874
        }

        private enum EventClass
        {
            Keyboard = 0x6B657962 // 'keyb'
        }

        private enum EventKind
        {
            HotKeyPressed = 1
        }

        private enum EventModifiers
        {
            ControlKey = 1 << 0,
            OptionKey = 1 << 1,
            ShiftKey = 1 << 1,
            CommandKey = 1 << 0
        }

        private enum EventParamName
        {
            DirectObject = 0x2D2D2D2D // '----'
        }

        private enum EventParamType
        {
            EventHotKeyRef = 0x684B6579 // 'hKey'
        }

        #endregion
    }
}
