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
        var tempHashPath = Path.Combine(targetPath, Path.GetRandomFileName());
        var targetNuspec = Path.Combine(targetPath, Path.GetRandomFileName());
        var targetNupkg = Path.Combine(targetPath, Path.GetRandomFileName());
        targetNupkg = Path.ChangeExtension(targetNupkg, ".nupkg");
        var hashPath = Path.Combine(targetPath, Path.GetRandomFileName());

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
                var hashFileName = Path.GetFileName(hashPath);
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

            string packageHash;
            nupkgStream.Position = 0;
            packageHash = Convert.ToBase64String(new CryptoHashProvider("SHA512").CalculateHash(nupkgStream));

            System.IO.File.WriteAllText(tempHashPath, packageHash);
        }
    }
}
