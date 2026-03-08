Add-Type -AssemblyName System.Drawing

$sourcePath = "c:\Users\Nasr\Desktop\VacationTracker\con.png"
$destPath = "c:\Users\Nasr\Desktop\VacationTracker\Platforms\Windows\appicon.ico"

# Load the image
$image = [System.Drawing.Image]::FromFile($sourcePath)

# Create a high-quality bitmap scaled to an icon-appropriate size (e.g. 256x256)
$bitmap = New-Object System.Drawing.Bitmap($image, 256, 256)

# Create the ICO file stream
$fs = New-Object System.IO.FileStream($destPath, [System.IO.FileMode]::Create)
$bw = New-Object System.IO.BinaryWriter($fs)

# Write the ICO header
$bw.Write([int16]0)   # reserved
$bw.Write([int16]1)   # type: 1 for icon
$bw.Write([int16]1)   # number of images

# Write the image directory entry
$bw.Write([byte]0)    # width (0 = 256)
$bw.Write([byte]0)    # height (0 = 256)
$bw.Write([byte]0)    # num colors (0 = >=8bpp)
$bw.Write([byte]0)    # reserved

$bw.Write([int16]1)   # color planes
$bw.Write([int16]32)  # bits per pixel

# We must write the image data payload to a memory stream to know its size
$ms = New-Object System.IO.MemoryStream
$bitmap.Save($ms, [System.Drawing.Imaging.ImageFormat]::Png)
$ms.Position = 0
$imageBytes = $ms.ToArray()

$bw.Write([int]$imageBytes.Length) # image data size

# Calculate the byte offset to the image data (header 6 bytes + directory 16 bytes = 22 bytes)
$bw.Write([int]22)                 # image data offset

# Write the actual image data payload (we embed the PNG bytes, which is valid for 256x256 icons)
$bw.Write($imageBytes)

# Cleanup
$bw.Dispose()
$fs.Dispose()
$ms.Dispose()
$bitmap.Dispose()
$image.Dispose()

Write-Host "Success: Converted $sourcePath to $destPath"
