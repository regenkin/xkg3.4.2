using System;

public class UploadResult
{
	public UploadState State
	{
		get;
		set;
	}

	public string Url
	{
		get;
		set;
	}

	public string OriginFileName
	{
		get;
		set;
	}

	public string ErrorMessage
	{
		get;
		set;
	}
}
