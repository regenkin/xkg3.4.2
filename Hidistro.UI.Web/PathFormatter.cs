using System;
using System.IO;
using System.Text.RegularExpressions;

public static class PathFormatter
{
	public static string Format(string originFileName, string pathFormat)
	{
		if (string.IsNullOrWhiteSpace(pathFormat))
		{
			pathFormat = "{filename}{rand:6}";
		}
		System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("[\\\\\\/\\:\\*\\?\\042\\<\\>\\|]");
		originFileName = regex.Replace(originFileName, "");
		string extension = System.IO.Path.GetExtension(originFileName);
		string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(originFileName);
		pathFormat = pathFormat.Replace("{filename}", fileNameWithoutExtension);
		pathFormat = new System.Text.RegularExpressions.Regex("\\{rand(\\:?)(\\d+)\\}", System.Text.RegularExpressions.RegexOptions.Compiled).Replace(pathFormat, delegate(System.Text.RegularExpressions.Match match)
		{
			int num = 6;
			if (match.Groups.Count > 2)
			{
				num = System.Convert.ToInt32(match.Groups[2].Value);
			}
			System.Random random = new System.Random();
			return random.Next((int)System.Math.Pow(10.0, (double)num), (int)System.Math.Pow(10.0, (double)(num + 1))).ToString();
		});
		pathFormat = pathFormat.Replace("{time}", System.DateTime.Now.Ticks.ToString());
		pathFormat = pathFormat.Replace("{yyyy}", System.DateTime.Now.Year.ToString());
		pathFormat = pathFormat.Replace("{yy}", (System.DateTime.Now.Year % 100).ToString("D2"));
		pathFormat = pathFormat.Replace("{mm}", System.DateTime.Now.Month.ToString("D2"));
		pathFormat = pathFormat.Replace("{dd}", System.DateTime.Now.Day.ToString("D2"));
		pathFormat = pathFormat.Replace("{hh}", System.DateTime.Now.Hour.ToString("D2"));
		pathFormat = pathFormat.Replace("{ii}", System.DateTime.Now.Minute.ToString("D2"));
		pathFormat = pathFormat.Replace("{ss}", System.DateTime.Now.Second.ToString("D2"));
		return pathFormat + extension;
	}
}
