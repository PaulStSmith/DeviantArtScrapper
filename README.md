# DeviantArt Scrapper

[![.NET 9](https://img.shields.io/badge/.NET-9.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/9.0)
[![Windows](https://img.shields.io/badge/Platform-Windows-lightgrey.svg)](https://www.microsoft.com/windows)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

A professional Windows desktop application for scraping and exporting DeviantArt content metadata with multiple export formats and advanced features.

![DeviantArt Scrapper Interface](docs/screenshot.png)

## Features

### 🎯 **Dual Functionality**
- **Gallery Scraping**: Collect all public artwork uploaded by any DeviantArt user
- **Favorites Scraping**: Export user's favorited content from other artists

### 📊 **Multiple Export Formats**
- **CSV**: Perfect for data analysis and research
- **HTML**: Professional galleries with dark theme and pagination
- **Excel (XLSX)**: Business-ready reports with advanced formatting

### 🚀 **Professional Features**
- **Real-Time Progress**: Live item counters and transfer rate monitoring
- **Cancellation Support**: Stop operations safely with partial export options
- **Retry Logic**: Automatic retry with exponential backoff for network issues
- **Rate Limiting**: Respectful API usage with built-in delays
- **Single Instance**: Mutex protection prevents multiple instances

### 🔒 **Security & Privacy**
- **Encrypted Storage**: DPAPI-encrypted credential storage
- **OAuth2 Authentication**: Secure token-based API access
- **Local Processing**: All data processing happens on your machine
- **No Telemetry**: No data collection or external reporting

## Quick Start

### System Requirements
- **OS**: Windows 10 (1809) or Windows 11
- **Framework**: .NET 9 Runtime (auto-installed)
- **Memory**: 2GB RAM minimum, 4GB recommended
- **Storage**: 100MB + space for exported data

### Installation

#### Option 1: Installer (Recommended)
1. Download the latest installer from [Releases](releases/latest)
2. Run `DeviantArtScrapper-Setup.exe`
3. Follow the installation wizard
4. Launch from Start Menu or Desktop shortcut

#### Option 2: Portable
1. Download the portable ZIP from [Releases](releases/latest)
2. Extract to your preferred folder
3. Run `DeviantArtScrapper.exe`

### Setup
1. **Get DeviantArt API Credentials**:
   - Visit [DeviantArt Developer Portal](https://www.deviantart.com/developers/apps)
   - Create a new application with "Client Credentials" grant type
   - Note your Client ID and Client Secret

2. **Configure Application**:
   - Click "Settings" in the application
   - Enter your API credentials
   - Test connection and save

3. **Start Scraping**:
   - Enter a DeviantArt username
   - Choose export format and filename
   - Click "Start Scraping"

## Usage Examples

### Gallery Export
```
Username: sakimichan
Filename: sakimichan_gallery_2025
Format: CSV
Result: Complete catalog of uploaded artwork with metadata
```

### Favorites Analysis
```
Username: artlover123
Filename: favorites_analysis
Format: Excel
Result: Professional report of favorited content by artist
```

### Research Collection
```
Username: conceptartist
Filename: research_data
Format: HTML
Result: Browsable gallery with pagination and statistics
```

## Export Format Details

### CSV Export
- **Best For**: Data analysis, statistical research, Excel import
- **Contains**: Title, Author, URL, Date, Stats, Download URLs
- **Features**: UTF-8 encoding, proper escaping, analysis-ready format

### HTML Export
- **Best For**: Visual browsing, presentations, offline viewing
- **Features**: Responsive design, dark theme, pagination (100 items/page)
- **Contains**: Thumbnails, metadata, clickable links, mature content badges

### Excel Export
- **Best For**: Business reports, professional presentations
- **Features**: Hyperlinked URLs, conditional formatting, auto-filtering
- **Contains**: Professional styling, frozen headers, multiple worksheets

## Architecture

### Project Structure
```
DeviantArtScrapper/
├── DeviantArtScrapper/          # Main application
│   ├── Forms/                   # Windows Forms UI
│   ├── Models/                  # Data models and API responses
│   ├── Services/                # Business logic and API clients
│   ├── Extensions/              # Utility extensions
│   ├── Converters/              # JSON converters
│   └── Resources/               # Embedded templates
├── docs/                        # Documentation and assets
├── installer/                   # NSIS installer script
├── CLAUDE.md                    # Development guide
├── USER_MANUAL.md               # Complete user documentation
└── README.md                    # This file
```

### Key Technologies
- **.NET 9**: Modern framework with Windows Forms
- **RestSharp**: HTTP client for DeviantArt API
- **Newtonsoft.Json**: JSON serialization with custom converters
- **Syncfusion.XlsIO**: Professional Excel export capabilities
- **Windows DPAPI**: Secure credential encryption

### Design Patterns
- **Service-Oriented Architecture**: Clean separation of concerns
- **Repository Pattern**: Data access abstraction
- **Observer Pattern**: Real-time progress updates
- **Command Pattern**: Export format strategy implementation

## Development

### Building from Source
```batch
# Clone the repository
git clone https://github.com/yourusername/DeviantArtScrapper.git
cd DeviantArtScrapper

# Build the solution using MSBuild (automatically detects VS installation)
build-installer.bat

# Or manual build process:
# 1. Find MSBuild from Visual Studio installation
# 2. Restore NuGet packages
msbuild DeviantArtScrapper.sln /t:Restore /p:Configuration=Debug
# 3. Build the solution
msbuild DeviantArtScrapper.sln /p:Configuration=Debug /p:Platform="Any CPU"
```

### Publishing
```batch
# Create release build and installer (automated)
build-installer.bat

# Or manual publishing:
# 1. Build release version
msbuild DeviantArtScrapper.sln /p:Configuration=Release /p:Platform="Any CPU"
# 2. Publish application
msbuild DeviantArtScrapper\DeviantArtScrapper.csproj /t:Publish /p:Configuration=Release /p:RuntimeIdentifier=win-x64
# 3. Create installer (requires NSIS)
makensis installer/DeviantArtScrapper.nsi
```

### Dependencies
- **Newtonsoft.Json** (13.0.4): JSON serialization
- **RestSharp** (112.1.0): HTTP client
- **Syncfusion.XlsIO.Net.Core** (31.1.22): Excel export

## API Integration

### DeviantArt API Endpoints
- **Authentication**: `/oauth2/token` (OAuth2 client credentials)
- **Gallery**: `/api/v1/oauth2/gallery/all` (User uploaded content)
- **Favorites**: `/api/v1/oauth2/collections/all` (User favorited content)
- **Health Check**: `/api/v1/oauth2/placebo` (Connection testing)

### Rate Limiting
- **Respectful Usage**: 500ms minimum delay between requests
- **Retry Logic**: 3 attempts with exponential backoff (1s, 2s, 4s)
- **Pagination**: 20 items per request, unlimited total collection
- **Single Instance**: Prevents API abuse through multiple instances

## Contributing

We welcome contributions! Please see our [Contributing Guidelines](CONTRIBUTING.md) for details.

### Areas for Contribution
- **Export Formats**: New export formats (JSON, XML, etc.)
- **UI Improvements**: Enhanced user interface and experience
- **Performance**: Optimization for large collections
- **Documentation**: Improved guides and examples
- **Testing**: Unit tests and integration tests

### Development Setup
1. Install Visual Studio 2022 or VS Code with C# extension
2. Install .NET 9 SDK
3. Clone the repository and open the solution
4. Build and run for development

## Troubleshooting

### Common Issues

**"Application Already Running"**
- Only one instance can run at a time by design
- Close existing instance before starting new one

**"Connection Test Failed"**
- Verify your DeviantArt API credentials
- Check internet connection and firewall settings
- Ensure API application is active on DeviantArt

**"No Gallery Items Found"**
- Verify username spelling and existence
- Check if user's gallery is public
- Some users may have empty galleries

### Getting Help
- **User Manual**: Complete guide in [USER_MANUAL.md](USER_MANUAL.md)
- **Issues**: Report bugs via [GitHub Issues](issues)
- **Documentation**: Technical details in [CLAUDE.md](CLAUDE.md)

## Legal

### License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

### Disclaimer
- This tool is for educational and research purposes
- Respect DeviantArt's Terms of Service and API usage guidelines
- Only collect publicly available information
- Credit artists appropriately when using collected data

### Attribution
- DeviantArt is a trademark of DeviantArt, Inc.
- This project is not affiliated with or endorsed by DeviantArt
- All scraped content remains property of respective artists

## Changelog

### Version 0.1.25.1005 (October 5, 2025)
- ✅ Complete dual-tab interface (Gallery + Favorites)
- ✅ Multiple export formats (CSV, HTML, Excel)
- ✅ Real-time progress monitoring with transfer rates
- ✅ Cancellation support with partial export
- ✅ Single-instance mutex protection
- ✅ Professional HTML templates with dark theme
- ✅ Excel export with advanced formatting
- ✅ Comprehensive error handling and retry logic
- ✅ Secure credential storage with DPAPI encryption

## Support

### Getting Support
- 📖 **Documentation**: Start with [USER_MANUAL.md](USER_MANUAL.md)
- 🐛 **Bug Reports**: Use [GitHub Issues](issues) with detailed information
- 💡 **Feature Requests**: Submit via [GitHub Issues](issues) with enhancement label
- 💬 **Discussions**: Join community discussions for tips and best practices

### Roadmap
- 🔄 **Bulk Processing**: Queue multiple users for batch processing
- 📱 **Mobile Exports**: Mobile-optimized HTML templates
- 🔍 **Advanced Filtering**: Filter by date, popularity, content type
- 📊 **Analytics Dashboard**: Built-in statistics and trending analysis
- 🌐 **Internationalization**: Multi-language support

---

**Made with ❤️ for the DeviantArt community**

*DeviantArt Scrapper - Professional content metadata collection for researchers, artists, and enthusiasts.*