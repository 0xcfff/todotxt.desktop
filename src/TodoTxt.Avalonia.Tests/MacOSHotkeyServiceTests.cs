using NUnit.Framework;
using TodoTxt.Avalonia.Services;
using System.Runtime.InteropServices;

namespace TodoTxt.Avalonia.Tests;

/// <summary>
/// Tests for MacOSHotkeyService functionality
/// </summary>
[TestFixture]
public class MacOSHotkeyServiceTests
{
    private MacOSHotkeyService _hotkeyService = null!;

    [SetUp]
    public void Setup()
    {
        // Only create the service if we're on macOS to avoid P/Invoke issues
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            _hotkeyService = new MacOSHotkeyService();
        }
        else
        {
            // Create a mock service for testing on non-macOS platforms
            _hotkeyService = new MacOSHotkeyService();
        }
    }

    [TearDown]
    public void TearDown()
    {
        _hotkeyService?.Dispose();
    }

    /// <summary>
    /// Verifies that the service correctly reports support status based on the platform
    /// </summary>
    [Test]
    public void IsSupported_WithServiceCreated_ReturnsCorrectPlatformSupport()
    {
        // arrange
        
        // act
        var isSupported = _hotkeyService.IsSupported;
        
        // assert
        var expectedSupport = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        Assert.That(isSupported, Is.EqualTo(expectedSupport));
    }

    /// <summary>
    /// Verifies that the service can be instantiated without throwing exceptions
    /// </summary>
    [Test]
    public void MacOSHotkeyServiceCtor_WithDefaultValuesProvided_CreatesServiceInstance()
    {
        // arrange
        
        // act
        var service = new MacOSHotkeyService();
        
        // assert
        Assert.That(service, Is.Not.Null);
        Assert.That(service.IsSupported, Is.EqualTo(RuntimeInformation.IsOSPlatform(OSPlatform.OSX)));
        
        // Clean up
        service.Dispose();
    }

    /// <summary>
    /// Verifies that the service can be disposed without throwing exceptions
    /// </summary>
    [Test]
    public void Dispose_WithServiceInstanceProvided_DisposesWithoutException()
    {
        // arrange
        var service = new MacOSHotkeyService();
        
        // act
        // assert
        Assert.DoesNotThrow(() => service.Dispose());
    }

    /// <summary>
    /// Verifies that GetRegisteredHotkeys returns an empty collection initially
    /// </summary>
    [Test]
    public void GetRegisteredHotkeys_WithNewServiceProvided_ReturnsEmptyCollection()
    {
        // arrange
        
        // act
        var registeredHotkeys = _hotkeyService.GetRegisteredHotkeys();
        
        // assert
        Assert.That(registeredHotkeys, Is.Not.Null);
        Assert.That(registeredHotkeys, Is.Empty);
    }

    /// <summary>
    /// Verifies that IsHotkeyAvailable returns true for basic hotkeys when supported
    /// </summary>
    [Test]
    public void IsHotkeyAvailable_WithBasicHotkeyProvided_ReturnsTrueWhenSupported()
    {
        // arrange
        var hotkey = new Hotkey(true, false, false, false, 65); // Ctrl+A
        
        // act
        var isAvailable = _hotkeyService.IsHotkeyAvailable(hotkey);
        
        // assert
        if (_hotkeyService.IsSupported)
        {
            Assert.That(isAvailable, Is.True);
        }
        else
        {
            Assert.That(isAvailable, Is.False);
        }
    }

    /// <summary>
    /// Verifies that RegisterHotkey returns false when not supported
    /// </summary>
    [Test]
    public void RegisterHotkey_WithServiceNotSupported_ReturnsFalse()
    {
        // arrange
        var hotkey = new Hotkey(true, false, false, false, 65); // Ctrl+A
        var id = 1;
        
        // act
        var result = _hotkeyService.RegisterHotkey(hotkey, id);
        
        // assert
        if (!_hotkeyService.IsSupported)
        {
            Assert.That(result, Is.False);
        }
        // If supported, we can't easily test registration without actual macOS APIs
    }

    /// <summary>
    /// Verifies that UnregisterHotkey returns false for non-existent hotkey
    /// </summary>
    [Test]
    public void UnregisterHotkey_WithNonExistentIdProvided_ReturnsFalse()
    {
        // arrange
        var nonExistentId = 999;
        
        // act
        var result = _hotkeyService.UnregisterHotkey(nonExistentId);
        
        // assert
        Assert.That(result, Is.False);
    }

    /// <summary>
    /// Verifies that UnregisterAllHotkeys can be called without exceptions
    /// </summary>
    [Test]
    public void UnregisterAllHotkeys_WithServiceInstanceProvided_ExecutesWithoutException()
    {
        // arrange
        
        // act
        // assert
        Assert.DoesNotThrow(() => _hotkeyService.UnregisterAllHotkeys());
    }

    /// <summary>
    /// Verifies that the HotkeyPressed event can be subscribed to without exceptions
    /// </summary>
    [Test]
    public void HotkeyPressed_WithEventSubscriptionProvided_CanSubscribeWithoutException()
    {
        // arrange
        
        // act
        _hotkeyService.HotkeyPressed += (sender, args) => { /* Event handler */ };
        
        // assert
        Assert.DoesNotThrow(() => _hotkeyService.HotkeyPressed -= (sender, args) => { /* Event handler */ });
    }
}
