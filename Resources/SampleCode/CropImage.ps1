$InputPath = 'C:\Users\lerwi\Downloads\Source.png';
$OutputPath = 'C:\Users\lerwi\Downloads\00Target.png';
$TargetWidth = 1680;
$TargetHeight = 1048;
$Image = [System.Drawing.Image]::FromFile($InputPath);
try {
    $Bitmamp = [System.Drawing.Bitmap]::new($TargetWidth, $TargetHeight);
    try {
        $Graphics = [System.Drawing.Graphics]::FromImage($Bitmamp);
        try {
            $Graphics.DrawImageUnscaled($Image, 0, 0);
            $Graphics.Flush();
            $Bitmamp.Save($OutputPath, [System.Drawing.Imaging.ImageFormat]::Png);
        } finally { $Graphics.Dispose() }
    } finally { $Bitmamp.Dispose() }
} finally { $Image.Dispose() }
