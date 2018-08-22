using System;

public class UploadConfig
{
	public string PathFormat
	{
		get;
		set;
	}

	public string UploadFieldName
	{
		get;
		set;
	}

	public int SizeLimit
	{
		get;
		set;
	}

	public string[] AllowExtensions
	{
		get;
		set;
	}

	public bool Base64
	{
		get;
		set;
	}

	public string Base64Filename
	{
		get;
		set;
	}
}
