# Path to the Plugin.cs file
$pluginFilePath = "Plugin.cs"

# Path to the csproj file
$csprojFilePath = "kvan-RaidSkillInfo.csproj"

# Read the Plugin.cs file
$pluginContent = Get-Content $pluginFilePath

# Use regex to find the version number in Plugin.cs
$versionRegex = [regex]'\[BepInPlugin\("kvan\.RaidSkillInfo", "kvan-RaidSkillInfo", "(\d+\.\d+\.\d+)"\)\]'
$versionMatch = $versionRegex.Match($pluginContent)

if ($versionMatch.Success) {
    # Extract the version number
    $version = $versionMatch.Groups[1].Value
    Write-Host "Found version: $version"

    # Load the csproj file as XML
    [xml]$csprojXml = Get-Content $csprojFilePath

    # Find the <Version> element and update its value
    $versionElement = $csprojXml.Project.PropertyGroup.Version
    if ($versionElement -ne $null) {
        $csprojXml.Project.PropertyGroup.Version = $version
        Write-Host "Updated version in csproj file to: $version"
    } else {
        # If the <Version> element doesn't exist, create it
        $propertyGroup = $csprojXml.Project.PropertyGroup
        $newVersionElement = $csprojXml.CreateElement("Version")
        $newVersionElement.InnerText = $version
        $propertyGroup.AppendChild($newVersionElement)
        Write-Host "Added new version element to csproj file with version: $version"
    }

    # Save the updated XML back to the csproj file
    $csprojXml.Save($csprojFilePath)
} else {
    Write-Host "Version not found in Plugin.cs"
}
