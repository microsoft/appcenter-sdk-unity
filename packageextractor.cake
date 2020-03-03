// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Path =  System.IO.Path;

class PackageExtractor
{
    static PackageSaveMode packageSaveMode = PackageSaveMode.Defaultv3;

    public static void Extract(string fileName, string targetPath)
    {
        using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
        {
            Extract(stream, targetPath);
        }
    }

    public static void Extract(Stream nupkgStream, string targetPath)
    {
        var targetNuspec = Path.Combine(targetPath, Path.GetRandomFileName());
        var targetNupkg = Path.Combine(targetPath, Path.GetRandomFileName());
        targetNupkg = Path.ChangeExtension(targetNupkg, ".nupkg");

        using (var packageReader = new PackageArchiveReader(nupkgStream))
        {
            var nuspecFile = packageReader.GetNuspecFile();
            if ((packageSaveMode & PackageSaveMode.Nuspec) == PackageSaveMode.Nuspec)
            {
                packageReader.ExtractFile(nuspecFile, targetNuspec, new NullLogger());
            }

            if ((packageSaveMode & PackageSaveMode.Files) == PackageSaveMode.Files)
            {
                var nupkgFileName = Path.GetFileName(targetNupkg);
                var packageFiles = packageReader.GetFiles()
                    .Where(file => Path.GetExtension(file) == ".dll");
                var packageFileExtractor = new PackageFileExtractor(
                    packageFiles,
                    XmlDocFileSaveMode.None);
                packageReader.CopyFiles(
                    targetPath,
                    packageFiles,
                    packageFileExtractor.ExtractPackageFile,
                    new NullLogger(),
                    CancellationToken.None);
            }
        }
    }
}
