using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

public class ListFileManager : Handler
{
	private enum ResultState
	{
		Success,
		InvalidParam,
		AuthorizError,
		IOError,
		PathNotFound
	}

	private int Start;

	private int Size;

	private int Total;

	private ListFileManager.ResultState State;

	private string PathToList;

	private string[] FileList;

	private string[] SearchExtensions;

	public ListFileManager(System.Web.HttpContext context, string pathToList, string[] searchExtensions) : base(context)
	{
		this.SearchExtensions = (from x in searchExtensions
		select x.ToLower()).ToArray<string>();
		this.PathToList = pathToList;
	}

	public override void Process()
	{
		try
		{
			this.Start = (string.IsNullOrEmpty(base.Request["start"]) ? 0 : System.Convert.ToInt32(base.Request["start"]));
			this.Size = (string.IsNullOrEmpty(base.Request["size"]) ? Config.GetInt("imageManagerListSize") : System.Convert.ToInt32(base.Request["size"]));
		}
		catch (System.FormatException)
		{
			this.State = ListFileManager.ResultState.InvalidParam;
			this.WriteResult();
			return;
		}
		System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
		try
		{
			string localPath = base.Server.MapPath(this.PathToList);
			list.AddRange(from x in System.IO.Directory.GetFiles(localPath, "*", System.IO.SearchOption.AllDirectories)
			where this.SearchExtensions.Contains(System.IO.Path.GetExtension(x).ToLower())
			select this.PathToList + x.Substring(localPath.Length).Replace("\\", "/"));
			this.Total = list.Count;
			this.FileList = (from x in list
			orderby x
			select x).Skip(this.Start).Take(this.Size).ToArray<string>();
		}
		catch (System.UnauthorizedAccessException)
		{
			this.State = ListFileManager.ResultState.AuthorizError;
		}
		catch (System.IO.DirectoryNotFoundException)
		{
			this.State = ListFileManager.ResultState.PathNotFound;
		}
		catch (System.IO.IOException)
		{
			this.State = ListFileManager.ResultState.IOError;
		}
		finally
		{
			this.WriteResult();
		}
	}

    private void WriteResult()
    {
        string strState = this.GetStateString();
        var query = from x in this.FileList
                    select new
                    {
                        url = x
                    };
        base.WriteJson(new
        {
            state = strState,
            list = query,
            start = this.Start,
            size = this.Size,
            total = this.Total
        });
    }

    private string GetStateString()
	{
		switch (this.State)
		{
		case ListFileManager.ResultState.Success:
			return "SUCCESS";
		case ListFileManager.ResultState.InvalidParam:
			return "参数不正确";
		case ListFileManager.ResultState.AuthorizError:
			return "文件系统权限不足";
		case ListFileManager.ResultState.IOError:
			return "文件系统读取错误";
		case ListFileManager.ResultState.PathNotFound:
			return "路径不存在";
		default:
			return "未知错误";
		}
	}
}
