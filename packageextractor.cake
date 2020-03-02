// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

class PackageExtractor
{
    static PackageSaveMode packageSaveMode = PackageSaveMode.Defaultv3;

    public static void Extract(string fileName)
    {
        var dir = Environment.CurrentDirectory;
        Extract(fileName, dir);
    }

    public static void Extract(string fileName, string targetPath)
    {
        using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
        {
            Extract(stream, targetPath);
        }
    }

    public static void Extract(Stream nupkgStream, string targetPath)
    {
        var tempHashPath = System.IO.Path.Combine(targetPath, System.IO.Path.GetRandomFileName());
        var targetNuspec = System.IO.Path.Combine(targetPath, System.IO.Path.GetRandomFileName());
        var targetNupkg = System.IO.Path.Combine(targetPath, System.IO.Path.GetRandomFileName());
        targetNupkg = System.IO.Path.ChangeExtension(targetNupkg, ".nupkg");
        var hashPath = System.IO.Path.Combine(targetPath, System.IO.Path.GetRandomFileName());

        using (var packageReader = new PackageArchiveReader(nupkgStream))
        {
            var nuspecFile = packageReader.GetNuspecFile();
            if ((packageSaveMode & PackageSaveMode.Nuspec) == PackageSaveMode.Nuspec)
            {
                packageReader.ExtractFile(nuspecFile, targetNuspec, new NullLogger());
            }

            if ((packageSaveMode & PackageSaveMode.Files) == PackageSaveMode.Files)
            {
                var nupkgFileName = System.IO.Path.GetFileName(targetNupkg);
                var hashFileName = System.IO.Path.GetFileName(hashPath);
                var packageFiles = packageReader.GetFiles()
                    .Where(file => ShouldInclude(file, nupkgFileName, nuspecFile, hashFileName));
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

    private static bool ShouldInclude(
        string fullName,
        string nupkgFileName,
        string nuspecFile,
        string hashFileName)
    {
        // Not all the files from a zip file are needed
        // So, files such as '.rels' and '[Content_Types].xml' are not extracted
        var fileName = System.IO.Path.GetFileName(fullName);
        if (fileName != null)
        {
            if (fileName == ".rels")
            {
                return false;
            }
            if (fileName == "[Content_Types].xml")
            {
                return false;
            }
        }

        var extension = System.IO.Path.GetExtension(fullName);
        if (extension == ".psmdcp")
        {
            return false;
        }

        if (string.Equals(fullName, nupkgFileName, StringComparison.OrdinalIgnoreCase)
            || string.Equals(fullName, hashFileName, StringComparison.OrdinalIgnoreCase)
            || string.Equals(fullName, nuspecFile, StringComparison.OrdinalIgnoreCase))
        {
            // Return false when the fullName is the nupkg file or the hash file.
            // Some packages accidentally have the nupkg file or the nupkg hash file in the package.
            // We filter them out during package extraction
            return false;
        }

        return true;
    }
}
