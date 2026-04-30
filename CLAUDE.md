# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

### Building and Running

```bash
# Build all projects
dotnet build PJ-Phone.sln

# Build specific project
dotnet build SoftPhone/SoftPhone.csproj
dotnet build PJ.SoftPhoneSdk/PJ.SoftPhoneSdk.csproj
dotnet build PJ-Phone-Core/PJ-Phone-Core.csproj

# Run the GUI application
dotnet run --project SoftPhone/SoftPhone.csproj

# Run the console application
dotnet run --project PJ-Phone-Core/PJ-Phone-Core.csproj -- --number <ext> --password <pass> --server <ip> --port <port>

# Clean build artifacts
dotnet clean PJ-Phone.sln

# Restore packages
dotnet restore PJ-Phone.sln
```

### Testing

```bash
# Run tests for all projects
dotnet test PJ-Phone.sln

# Run tests for specific project
dotnet test SoftPhone/SoftPhone.csproj
dotnet test PJ.SoftPhoneSdk/PJ.SoftPhoneSdk.csproj
dotnet test PJ-Phone-Core/PJ-Phone-Core.csproj
```

### Development Tasks

```bash
# Watch mode for development (Avalonia)
dotnet watch run --project SoftPhone/SoftPhone.csproj

# Generate Avalonia XAML compilation
dotnet avalonia generate-xamlc --project SoftPhone/SoftPhone.csproj
```

## Code Architecture

### Project Structure

```
PJ-Phone/
├── PJ.SoftPhoneSdk/           # Reusable SIP phone SDK
│   ├── Sip/                   # SIP protocol implementation
│   │   ├── SipPhone.cs        # Main SIP phone manager
│   │   ├── SipAccount.cs      # SIP account management
│   │   └── SipCall.cs         # Individual call handling
│   ├── IPHone.cs              # Public API interface
│   └── IPhoneLogger.cs        # Logging interface
│
├── PJ-Phone-Core/             # Console SIP phone
│   ├── Sip/                   # SIP implementation (mirrors SDK)
│   └── PhoneApp.cs            # Console application logic
│
├── SoftPhone/                 # GUI desktop application
│   ├── Automation/            # Call automation features
│   ├── ValueConvertor/        # UI value converters
│   ├── MainWindow.axaml.cs    # Main window logic
│   └── PhoneView.axaml.cs     # Individual phone instance view
│
└── PJ.SoftPhoneSdk.PjPhone/   # Bridge between SDK and applications
    └── PhoneApp.cs            # Application integration layer
```

### Key Components

#### 1. SIP Protocol Implementation (PJ.SoftPhoneSdk/Sip/)
- **SipPhone.cs**: Static manager class that handles PJSIP endpoint initialization, transport creation, and global event routing
- **SipAccount.cs**: Represents individual SIP accounts with registration, authentication, and call management
- **SipCall.cs**: Handles individual call lifecycle, audio media management, recording, and playback

#### 2. Public API (PJ.SoftPhoneSdk/IPhone.cs)
Defines the interface for external applications:
- `Login()`: Register with SIP server
- `Call()`: Make outbound calls with optional recording
- `Hangup()`: Terminate current call
- `Pickup()`: Answer incoming calls
- `Play()`: Play audio files during active calls
- Event handlers for registration state, incoming calls, call connected/hangup

#### 3. GUI Application (SoftPhone/)
- **MainWindow**: Manages multiple phone profiles, loads/saves configurations from JSON files
- **PhoneView**: Individual phone instance with dialer, call controls, and status display
- **PhoneProfile**: Data model for phone configuration (server, credentials, recording settings)
- **AutomationManager**: Automated call/pickup functionality

#### 4. Core Dependencies
- **PJSIP**: SIP protocol stack through C# bindings
- **Avalonia UI**: Cross-platform desktop UI framework
- **ini-parser**: Configuration file parsing

### Data Flow

1. **Initialization**: `SipPhone.Init()` creates PJSIP endpoint and transport
2. **Account Management**: `SipPhone.AddSipAccount()` registers credentials with SIP server
3. **Call Handling**: 
   - Incoming calls trigger `OnIncomingCall` events
   - Active calls managed through `SipCall` instances
   - Audio routing handled by `ConnectAudioDevice()` method
4. **Recording**: Enabled per-call basis, stored as WAV files (16kHz, mono, PCM)

### Configuration

- **Profiles**: Stored as JSON files in `profiles/` directory
- **Settings**: Application configuration via Avalonia UI
- **Logging**: Custom `IPhoneLogger` implementations, defaults to trace output

### Key Design Patterns

- **Singleton Pattern**: `SipPhone` uses static methods for global phone management
- **Observer Pattern**: Event-driven architecture for call/registration state changes
- **Dependency Injection**: Avalonia MVVM pattern for UI components
- **Factory Pattern**: Account and call object creation through static factory methods

### Important Considerations

1. **Thread Safety**: All SIP operations use proper locking mechanisms (`lock` statements)
2. **Resource Management**: Proper disposal of audio devices, recorders, and players
3. **Audio Format Requirements**: 
   - Recording: 16kHz sample rate, mono, 16-bit PCM WAV
   - Playback: Same format required for compatibility
4. **Multiple Accounts**: GUI supports multiple concurrent SIP accounts
5. **Cross-Platform**: Built with .NET 8 and Avalonia for Windows/Linux/macOS support

### Common Development Tasks

- **Adding new SIP features**: Extend `SipPhone`, `SipAccount`, or `SipCall` classes
- **GUI modifications**: Update Avalonia XAML files and corresponding code-behind
- **Audio processing**: Modify `SipCall.cs` for different audio formats or processing
- **Configuration changes**: Update `PhoneProfile` model and JSON serialization
- **Event handling**: Subscribe to existing events or add new ones in appropriate classes