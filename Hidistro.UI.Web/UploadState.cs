using System;

public enum UploadState
{
	Success,
	SizeLimitExceed = -1,
	TypeNotAllow = -2,
	FileAccessError = -3,
	NetworkError = -4,
	Unknown = 1
}
