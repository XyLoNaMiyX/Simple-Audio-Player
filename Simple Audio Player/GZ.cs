
using System;
using System.IO;
using System.IO.Compression;

public static class GZ
{

    public static void Compress(string srcFile, string dstFile = "", bool overwrite = true)
    {	
    	if (dstFile.Length == 0)
    		dstFile = srcFile + ".gz";
    	
    	if (File.Exists(dstFile))
    		if (overwrite)
    			File.Delete(dstFile);
	    	else
	    		return;
    	
    	var fi = new FileInfo(srcFile);
    	
        using (FileStream inFile = fi.OpenRead())
            using (FileStream outFile = File.Create(dstFile))
                using (var compress = new GZipStream(outFile, CompressionMode.Compress))
                	inFile.CopyTo(compress);
    }

    public static void Decompress(string srcFile, string dstFile = "", bool overwrite = true)
    {
    	
    	if (dstFile.Length == 0)
    		dstFile = srcFile.Remove(srcFile.Length - Path.GetExtension(srcFile).Length);
    	
    	if (File.Exists(dstFile))
    		if (overwrite)
    			File.Delete(dstFile);
	    	else
	    		return;
    	
    	var fi = new FileInfo(srcFile);
    	
        using (FileStream inFile = fi.OpenRead())
            using (FileStream outFile = File.Create(dstFile))
                using (var decompress = new GZipStream(inFile, CompressionMode.Decompress))
        		    decompress.CopyTo(outFile);
    }
}
