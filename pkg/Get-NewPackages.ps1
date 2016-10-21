$folders = dir -Directory;
foreach($folder in $folders) {
    $pkgs = dir "$($folder.FullName)\bin\*\*.nupkg" ; 
    copy -Path $pkgs -Destination .\ -Force;
}