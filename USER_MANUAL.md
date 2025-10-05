# DeviantArt Scrapper - User Manual

## Table of Contents
1. [Introduction](#introduction)
2. [System Requirements](#system-requirements)
3. [Installation & Setup](#installation--setup)
4. [Getting Started](#getting-started)
5. [API Configuration](#api-configuration)
6. [Gallery Scraping](#gallery-scraping)
7. [Favorites Scraping](#favorites-scraping)
8. [Export Formats](#export-formats)
9. [Progress Monitoring](#progress-monitoring)
10. [Troubleshooting](#troubleshooting)
11. [Tips & Best Practices](#tips--best-practices)
12. [FAQ](#faq)

---

## Introduction

DeviantArt Scrapper is a professional desktop application designed to help you collect and export artwork data from DeviantArt. Whether you're a researcher, artist, collector, or data analyst, this tool provides a comprehensive solution for gathering DeviantArt content information.

### Key Features
- **Dual Functionality**: Scrape both user galleries and favorites
- **Multiple Export Formats**: CSV for analysis, HTML for viewing, Excel for reports
- **Real-Time Progress**: Live updates with transfer rates and item counts
- **Professional Quality**: Retry logic, rate limiting, and error handling
- **Secure**: Encrypted credential storage and OAuth2 authentication

---

## System Requirements

### Minimum Requirements
- **Operating System**: Windows 10 version 1809 (build 17763) or later
- **Framework**: .NET 9 Runtime (automatically installed if missing)
- **Memory**: 2 GB RAM
- **Storage**: 100 MB available space (plus space for exported data)
- **Internet**: Stable internet connection for API access

### Recommended Requirements
- **Operating System**: Windows 11
- **Memory**: 4 GB RAM or more
- **Storage**: 1 GB available space
- **Internet**: Broadband connection for faster scraping

---

## Installation & Setup

### Download & Install
1. Download the latest release from the project repository
2. Extract the application to your desired folder
3. Run `DeviantArtScrapper.exe` to launch the application
4. The .NET 9 runtime will be downloaded automatically if not present

### First Launch
On first launch, you'll see the main application window with:
- **Status**: "Not configured - Please configure API settings"
- **Gallery Tab**: Ready for input but requiring API setup
- **Favorites Tab**: Also available after API configuration
- **Settings Button**: Located in the bottom right corner

---

## API Configuration

Before you can scrape any data, you need to configure your DeviantArt API credentials.

### Getting DeviantArt API Access

1. **Visit DeviantArt Developer Portal**
   - Go to [DeviantArt Developer Portal](https://www.deviantart.com/developers/apps)
   - Log in with your DeviantArt account

2. **Create a New Application**
   - Click "Register your Application"
   - Fill in the application details:
     - **Application Name**: "Personal Scraper" (or your choice)
     - **Description**: "Personal data collection tool"
     - **Website**: Your website or "N/A"
   - **Grant Type**: Select "Client Credentials"
   - Accept the terms and create the application

3. **Get Your Credentials**
   - Once created, you'll receive:
     - **Client ID**: A long string of characters
     - **Client Secret**: Another long string (keep this private!)

### Configuring the Application

1. **Open Settings**
   - Click the "Settings" button in the bottom right corner
   - The Settings dialog will open

2. **Enter Your Credentials**
   - **Client ID**: Paste your Client ID from DeviantArt
   - **Client Secret**: Paste your Client Secret from DeviantArt
   - Click "Test Connection" to verify your credentials
   - If successful, you'll see "Connection successful!"

3. **Save Configuration**
   - Click "OK" to save your settings
   - Your credentials are encrypted and stored securely on your computer
   - The main window status will update to "Ready - API configured and authenticated"

### Security Notes
- Your credentials are encrypted using Windows DPAPI
- Credentials are only stored on your local machine
- The application uses OAuth2 for secure API access
- No passwords or personal data are stored

---

## Gallery Scraping

Gallery scraping collects all public artwork uploaded by a specific DeviantArt user.

### Step-by-Step Process

1. **Navigate to Gallery Tab**
   - The Gallery tab is selected by default
   - Ensure your API is configured (status shows "Ready")

2. **Enter Target Username**
   - In the "DeviantArt Username" field, enter the username you want to scrape
   - Example: "sakimichan", "loish", "david-revoy"
   - **Note**: Only public galleries can be scraped

3. **Choose Export Filename**
   - Enter a filename in the "File Name" field (without extension)
   - Default: "gallery_export"
   - You can use the "..." button to browse and select a location

4. **Select Export Format**
   - **CSV**: Best for data analysis and spreadsheets
   - **HTML**: Creates a visual gallery for web viewing
   - **Excel (XLSX)**: Professional reports with advanced formatting

5. **Start Scraping**
   - Click "Start Scraping" to begin the process
   - The progress bar will appear and show live updates
   - You'll see real-time statistics:
     - Number of items collected
     - Transfer rate in KB/s
     - Current operation status

### What Gets Collected
- **Basic Information**: Title, URL, publication date
- **Artist Details**: Username and profile information
- **Engagement Stats**: Favorites count, comments count
- **Media Information**: Download URLs, thumbnail URLs
- **Content Flags**: Mature content indicators
- **Metadata**: All available public data from the API

### Data Sorting
Gallery items are automatically sorted by publication date (newest first) to maintain chronological organization.

---

## Favorites Scraping

Favorites scraping collects all public artwork that a user has favorited from other artists.

### How It Works

1. **Switch to Favorites Tab**
   - Click the "Favorites" tab in the main window
   - The interface is similar to the Gallery tab

2. **Configure Scraping**
   - **Username**: Enter the user whose favorites you want to collect
   - **Filename**: Choose a name for your export (default: "favorites_export")
   - **Format**: Select CSV, HTML, or Excel export

3. **Start Collection**
   - Click "Start Scraping" to begin
   - The process works the same as gallery scraping
   - Progress is shown in real-time

### Favorites-Specific Features
- **Multi-Artist Data**: Collects works from many different artists
- **Special Sorting**: Organized by artist name, then by date
- **Unique Templates**: HTML exports use favorites-specific styling
- **Collection Analysis**: Great for studying user preferences and trends

### Use Cases for Favorites
- **Inspiration Research**: See what inspires specific artists
- **Trend Analysis**: Identify popular styles and themes
- **Community Mapping**: Understand artistic communities and connections
- **Curation Studies**: Analyze how users curate their collections

---

## Export Formats

### CSV (Comma-Separated Values)

**Best For**: Data analysis, spreadsheets, statistical research

**File Structure**:
```
Title,Author,URL,Published Date,Mature Content,Stats Favourites,Stats Comments,Download URL,Thumbnail URL
"Fantasy Landscape","artist123","https://...","2025-01-15 14:30:00",false,"1250","45","https://...","https://..."
```

**Features**:
- UTF-8 encoding for international characters
- Proper CSV escaping for special characters
- Compatible with Excel, Google Sheets, and analysis tools
- Small file size for large datasets

### HTML (Web Gallery)

**Best For**: Visual browsing, presentations, offline viewing

**Features**:
- **Professional Design**: Dark theme with modern styling
- **Responsive Layout**: Grid layout that adapts to screen size
- **Interactive Elements**: Clickable thumbnails with hover effects
- **Pagination**: 100 items per page for performance
- **Metadata Display**: Shows stats and information for each piece
- **Navigation**: Easy browsing between pages

**File Structure**:
- Main files: `{base_name}.html`, `{base_name}_page2_.html`, etc.
- Self-contained: No external dependencies required
- Mobile-friendly responsive design

### Excel (XLSX)

**Best For**: Professional reports, business analysis, detailed documentation

**Features**:
- **Professional Formatting**: Clean, business-ready appearance
- **Hyperlinked URLs**: Click to visit artwork directly
- **Conditional Formatting**: Visual indicators for mature content
- **Auto-Fitted Columns**: Optimal width for all data
- **Frozen Headers**: Header row stays visible while scrolling
- **Auto-Filtering**: Easy sorting and filtering options
- **Number Formatting**: Properly formatted statistics

**Advanced Features**:
- Multiple worksheets for complex analyses
- Formula-ready data for calculations
- Professional styling with corporate color schemes
- Export-ready for sharing with stakeholders

---

## Progress Monitoring

### Real-Time Feedback

The application provides comprehensive progress information during scraping operations:

**Progress Indicators**:
- **Progress Bar**: Visual indication of activity (marquee style)
- **Item Counter**: "Collected X items..." shows current progress
- **Transfer Rate**: Real-time KB/s measurement
- **Status Messages**: Current operation details

**Progress States**:
- **Initializing**: Setting up API connection
- **Collecting**: Actively gathering data
- **Exporting**: Processing and saving data
- **Complete**: Operation finished successfully
- **Error**: Issue encountered (with details)

### Cancellation Support

**How to Cancel**:
- Click the "Cancel" button that appears during scraping
- The operation will stop safely after the current API request
- You'll be offered the option to export partial results

**Partial Export Options**:
- **Export Collected Data**: Save what was gathered before cancellation
- **Discard Results**: Cancel completely without saving
- **Resume Later**: Not supported - restart from beginning

### Performance Monitoring

**Transfer Rate Calculation**:
- Based on items collected per second
- Estimated at ~2KB per artwork metadata
- Updates in real-time during operation
- Helps estimate completion time for large collections

**Typical Performance**:
- **Small Collections** (< 100 items): 30-60 seconds
- **Medium Collections** (100-1000 items): 2-10 minutes
- **Large Collections** (1000+ items): 10+ minutes
- **Factors**: Internet speed, API response time, collection size

---

## Troubleshooting

### Common Issues and Solutions

#### "Status: Not configured"
**Problem**: API credentials not set up
**Solution**: 
1. Click "Settings" button
2. Enter your DeviantArt API credentials
3. Test connection and save

#### "Connection Test Failed"
**Problem**: Invalid API credentials or network issue
**Solutions**:
1. Verify your Client ID and Client Secret are correct
2. Check your internet connection
3. Ensure your DeviantArt application is active
4. Try again in a few minutes (temporary API issues)

#### "No gallery items found"
**Problem**: User has no public gallery or username is incorrect
**Solutions**:
1. Verify the username is spelled correctly
2. Check if the user's gallery is public
3. Try a different username to test
4. Some users may have empty galleries

#### "Request failed (attempt 1/3)"
**Problem**: Temporary API or network issue
**Solution**: The application automatically retries - wait for completion

#### Application Freezes or Crashes
**Problem**: Unexpected error or system issue
**Solutions**:
1. Close and restart the application
2. Check available memory and disk space
3. Temporarily disable antivirus (may interfere)
4. Run as administrator if needed

#### Export File Access Denied
**Problem**: File is open in another program or permissions issue
**Solutions**:
1. Close Excel or other programs using the file
2. Choose a different filename or location
3. Run application as administrator
4. Check folder permissions

### Error Messages

#### API Error Messages
- **"Invalid client credentials"**: Check your Client ID and Secret
- **"Rate limit exceeded"**: Wait and try again (rare with built-in rate limiting)
- **"User not found"**: Verify the username exists and is public
- **"Network timeout"**: Check internet connection and retry

#### File Error Messages
- **"File already exists"**: Choose a different filename or delete existing file
- **"Insufficient disk space"**: Free up storage space
- **"Invalid filename"**: Use only letters, numbers, hyphens, and underscores

### Performance Issues

#### Slow Scraping Speed
**Causes and Solutions**:
1. **Slow Internet**: Use wired connection if possible
2. **Large Collections**: Normal for 1000+ items - be patient
3. **API Throttling**: Built-in rate limiting is working correctly
4. **System Resources**: Close other applications to free memory

#### High Memory Usage
**Normal For**: Large collections (1000+ items)
**Solutions if Excessive**:
1. Restart application between large scraping sessions
2. Export smaller batches if possible
3. Close other memory-intensive applications

---

## Tips & Best Practices

### Maximizing Success

#### Username Research
- **Popular Artists**: Search DeviantArt's popular section
- **Username Format**: Most usernames are lowercase with hyphens or numbers
- **Profile URLs**: Username is in the URL: `deviantart.com/USERNAME`
- **Case Sensitivity**: DeviantArt usernames are case-insensitive

#### Export Planning
- **Large Collections**: Plan for longer processing times
- **File Organization**: Use descriptive filenames with dates
- **Format Selection**: Choose format based on intended use
- **Storage Space**: Large collections can generate significant data

#### Optimal Performance
- **Stable Internet**: Use wired connection for large collections
- **Close Other Apps**: Free up system resources for faster processing
- **Regular Updates**: Keep the application updated for best performance
- **Batch Processing**: Process multiple users separately rather than one massive operation

### Data Management

#### File Organization
```
DeviantArt_Exports/
├── 2025-01-15_artist1_gallery.csv
├── 2025-01-15_artist1_favorites.html
├── 2025-01-16_artist2_gallery.xlsx
└── README.txt (your notes)
```

#### Naming Conventions
- Include date: `2025-01-15_username_gallery`
- Specify type: `_gallery` or `_favorites`
- Use format: `.csv`, `.html`, or `.xlsx`
- Avoid spaces: Use underscores or hyphens

#### Data Backup
- **Regular Backups**: Export files can't be regenerated identically
- **Cloud Storage**: Consider backing up to cloud services
- **Version Control**: Keep dated versions for research continuity

### Ethical Usage

#### Respect Guidelines
- **Public Data Only**: Only scrape publicly available information
- **Rate Limiting**: Don't attempt to bypass built-in rate limiting
- **Academic Use**: Cite DeviantArt and artists in research
- **Commercial Use**: Respect DeviantArt's terms of service

#### Best Practices
- **Attribution**: Credit artists when using their data
- **Privacy**: Don't scrape or share private information
- **Research Ethics**: Follow institutional guidelines for data collection
- **Fair Use**: Use data responsibly and within legal boundaries

---

## FAQ

### General Questions

**Q: Is this application free to use?**<br/>
A: Yes, DeviantArt Scrapper is free software. However, you need your own DeviantArt API credentials.

**Q: Do I need a DeviantArt account?**<br/>
A: Yes, you need a DeviantArt account to create API credentials, but you can scrape any public user's content.

**Q: Can I scrape private or mature content?**<br/>
A: Only content that's publicly visible on DeviantArt can be scraped. Mature content flags are preserved in exports.

**Q: How much data can I collect?**<br/>
A: There are no built-in limits, but be respectful of DeviantArt's API usage guidelines.

### Technical Questions

**Q: Why do I need API credentials?**<br/>
A: DeviantArt requires authentication for all API access to prevent abuse and ensure stable service.

**Q: Is my data secure?**<br/>
A: Yes, credentials are encrypted locally using Windows security features. No data is sent to third parties.

**Q: Can I run multiple instances?**<br/>
A: No, the application prevents multiple instances from running simultaneously. This ensures proper API rate limiting and prevents conflicts during scraping operations.

**Q: What if DeviantArt changes their API?**<br/>
A: The application may need updates. Check for new versions if you encounter API errors.

### Usage Questions

**Q: How do I know if a user has a lot of content?**<br/>
A: Check their DeviantArt profile page - it shows total deviations and favorites counts.

**Q: Can I scrape my own content?**<br/>
A: Yes, you can scrape your own gallery and favorites just like any other public user.

**Q: What's the difference between Gallery and Favorites?**<br/>
A: Gallery contains artwork uploaded by the user; Favorites contains artwork they've favorited from others.

**Q: Can I modify the HTML export appearance?**<br/>
A: The current version uses embedded templates, but future versions may allow customization.

### Troubleshooting Questions

**Q: The application won't start. What should I do?**<br/>
A: Ensure you have Windows 10/11 and that .NET 9 is installed. Try running as administrator.

**Q: I'm getting "Access Denied" errors when exporting.**<br/>
A: Check that the target folder exists and you have write permissions. Close any open files with the same name.

**Q: Scraping is very slow. Is this normal?**<br/>
A: Yes, the application includes respectful rate limiting. Large collections naturally take longer to process.

**Q: Can I pause and resume scraping?**<br/>
A: Currently, you can cancel and export partial results, but pause/resume isn't supported.

---

## Support and Updates

### Getting Help
- **Documentation**: Refer to this manual for technical details
- **Issues**: Report bugs and feature requests through the project repository
- **Community**: Connect with other users for tips and best practices

### Staying Updated
- **Check Regularly**: New versions may include bug fixes and new features
- **API Changes**: DeviantArt occasionally updates their API - new versions adapt to changes
- **Feature Requests**: Suggest new features through project channels

### Contributing
- **Bug Reports**: Detailed reports help improve the application
- **Feature Ideas**: User feedback drives development priorities
- **Documentation**: Help improve this manual and other documentation

---

*DeviantArt Scrapper v0.1.25.1005 - Professional DeviantArt Data Collection Tool*

*This manual covers all major features and common usage scenarios. For technical development information, see CLAUDE.md in the project repository.*