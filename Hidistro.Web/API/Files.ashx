<%@ WebHandler Language="c#" Class="File_WebHandler" Debug="true" %>

using System;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing;

public class File_WebHandler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        string action = context.Request["action"];

        switch (action)
        {
            case "OneTao":
                context.Response.ContentType = "text/html";
                OneTaoUp(context);
                break;
            case "LogoUpload":
                LogoUpload(context);
                break;
            case "BakUpload":
                BakUpload(context);
                break;
            case "BackgroundUpload":
                BackgroundUpload(context); //掌柜名片背景上传
                break;
            case "MemberLogoUpload":
                 MemberLogoUpload(context); //掌柜名片背景上传
                break;
            default:
                break;
        }
    }

    public void OneTaoUp(HttpContext context)
    {
        HttpFileCollection files = context.Request.Files;
        if (files.Count > 0)
        {
            HttpPostedFile file = files[0];

            if (file.ContentLength > 0)
            {
                string imageUrl = string.Empty;
                if (file != null && !string.IsNullOrEmpty(file.FileName))
                {


                    string uploadPath = Hidistro.Core.Globals.GetStoragePath() + "/OneTao";
                    string newFilename = Guid.NewGuid().ToString("N", System.Globalization.CultureInfo.InvariantCulture) +
                                         Path.GetExtension(file.FileName);
                    //当前待上传的服务端路径
                    imageUrl = uploadPath + "/" + newFilename;
                    //当前文件后缀名
                    string ext = Path.GetExtension(file.FileName).ToLower();
                    //验证文件类型是否正确gif，jpeg，jpg，bmp，png
                    if (!ext.Equals(".gif") && !ext.Equals(".jpg") && !ext.Equals(".jpeg") && !ext.Equals(".png") && !ext.Equals(".bmp"))
                    {
                         context.Response.Write("2");
                        context.Response.End();
                    }

                  

                    string imageFilePath = context.Request.MapPath(imageUrl);

                    file.SaveAs(imageFilePath);
                  
                    Bitmap s = new Bitmap(imageFilePath);
                    s = GetThumbnail(s,100, 320);
                    s.Save(imageFilePath);
                    s.Dispose();
                        
                    context.Response.Write(imageUrl);
                    context.Response.End();
                }
                else
                {
                    //上传失败
                    context.Response.Write("0");
                    context.Response.End();
                }
            }
  
              
        }
        else
        {
            //没有选择图片 
            context.Response.Write("3");
            context.Response.End();
        }
    }
    
    
    public void MemberLogoUpload(HttpContext context)
    {
        HttpFileCollection files = context.Request.Files;
        if (files.Count > 0)
        {
            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFile file = files[i];

                if (file.ContentLength > 0)
                {
                    string imageUrl = string.Empty;
                    if (file != null && !string.IsNullOrEmpty(file.FileName))
                    {

                        
                        string uploadPath = Hidistro.Core.Globals.GetStoragePath() + "/Logo";
                        string newFilename = Guid.NewGuid().ToString("N", System.Globalization.CultureInfo.InvariantCulture) +
                                             Path.GetExtension(file.FileName);
                        //当前待上传的服务端路径
                        imageUrl = uploadPath + "/" + newFilename;
                        //当前文件后缀名
                        string ext = Path.GetExtension(file.FileName).ToLower();
                        //验证文件类型是否正确gif，jpeg，jpg，bmp，png
                        if (!ext.Equals(".gif") && !ext.Equals(".jpg") && !ext.Equals(".jpeg") && !ext.Equals(".png") && !ext.Equals(".bmp"))
                        {
                            //这里window.parent.uploadSuccess()是我在前端页面中写好的javascript function,此方法主要用于输出异常和上传成功后的图片地址
                            context.Response.Write("2");
                            context.Response.End();
                        }

                        string dlogo = context.Request["dlogo"];

                       // Hidistro.Core.Globals.Debuglog(i.ToString() + "--222--" + imageUrl);
                        
                        //开始上传

                        string imageFilePath = context.Request.MapPath(imageUrl);

                        file.SaveAs(imageFilePath);

                        if (!string.IsNullOrEmpty(dlogo))
                        {
                            //对LOGO图片进行处理

                            Bitmap s = new Bitmap(imageFilePath);

                            if (s.Height > 300 || s.Width > 300)
                            {
                                s = GetThumbnail(s, 300, 300);
                                s.Save(imageFilePath);
                                s.Dispose();
                            }


                        }

                     
                        context.Response.Write(imageUrl);
                        context.Response.End();
                    }
                    else
                    {
                        //上传失败
                        context.Response.Write("0");
                        context.Response.End();
                    }
                }
            }
        }
        else
        {
            //没有选择图片 
            context.Response.Write("3");
            context.Response.End();
        }
    }
    

    public void BackgroundUpload(HttpContext context)
    {
        HttpFileCollection files = context.Request.Files;
        if (files.Count > 0)
        {
            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFile file = files[i];

                if (file.ContentLength > 0)
                {
                    string imageUrl = string.Empty;
                    if (file != null && !string.IsNullOrEmpty(file.FileName))
                    {
                        string uploadPath = Hidistro.Core.Globals.GetStoragePath() + "/Logo";
                        string newFilename = "StoreCardBckImg" +
                                             Path.GetExtension(file.FileName);
                        //当前待上传的服务端路径
                        imageUrl = uploadPath + "/" + newFilename;
                        //当前文件后缀名
                        string ext = Path.GetExtension(file.FileName).ToLower();
                        //验证文件类型是否正确gif，jpeg，jpg，bmp，png
                        if (!ext.Equals(".gif") && !ext.Equals(".jpg") && !ext.Equals(".jpeg") && !ext.Equals(".png") && !ext.Equals(".bmp"))
                        {
                            //这里window.parent.uploadSuccess()是我在前端页面中写好的javascript function,此方法主要用于输出异常和上传成功后的图片地址
                            context.Response.Write("2");
                            context.Response.End();
                        }

                     
                        //验证文件的大小,小于1M
                        if (file.ContentLength > 1048578)
                        {
                           context.Response.Write("1");
                           context.Response.End();
                        }

                        //开始上传

                        string imageFilePath = context.Request.MapPath(imageUrl);

                        file.SaveAs(imageFilePath);
                        
                       
                            //对LOGO图片进行处理

                            Bitmap s = new Bitmap(imageFilePath);

                            if (s.Height > 1030 || s.Width > 640)
                            {
                                s = GetThumbnail(s,1030, 640);
                                s.Save(imageFilePath);
                            }
                            s.Dispose();

                        context.Response.Write(imageUrl);
                        context.Response.End();
                    }
                    else
                    {
                        //上传失败
                        context.Response.Write("0");
                        context.Response.End();
                    }
                }
            }
        }
        else
        {
            //没有选择图片 
            context.Response.Write("3");
            context.Response.End();
        }
    }
    
    public void LogoUpload(HttpContext context)
    {
        HttpFileCollection files = context.Request.Files;
        if (files.Count > 0)
        {
            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFile file = files[i];

                if (file.ContentLength > 0)
                {
                    string imageUrl = string.Empty;
                    if (file != null && !string.IsNullOrEmpty(file.FileName))
                    {
                        string uploadPath = Hidistro.Core.Globals.GetStoragePath() + "/Logo";
                        string newFilename = Guid.NewGuid().ToString("N", System.Globalization.CultureInfo.InvariantCulture) +
                                             Path.GetExtension(file.FileName);
                        //当前待上传的服务端路径
                        imageUrl = uploadPath + "/" + newFilename;
                        //当前文件后缀名
                        string ext = Path.GetExtension(file.FileName).ToLower();
                        //验证文件类型是否正确gif，jpeg，jpg，bmp，png
                        if (!ext.Equals(".gif") && !ext.Equals(".jpg") && !ext.Equals(".jpeg") && !ext.Equals(".png") && !ext.Equals(".bmp"))
                        {
                            //这里window.parent.uploadSuccess()是我在前端页面中写好的javascript function,此方法主要用于输出异常和上传成功后的图片地址
                            context.Response.Write("2");
                            context.Response.End();
                        }

                        string dlogo = context.Request["dlogo"];
                        
                        //验证文件的大小,小于120KB
                        if (file.ContentLength > 4122880 && string.IsNullOrEmpty(dlogo))
                        {
                            //这里window.parent.uploadSuccess()是我在前端页面中写好的javascript function,此方法主要用于输出异常和上传成功后的图片地址 
                            context.Response.Write("1");
                            context.Response.End();
                        }
                        
                        //开始上传

                        string imageFilePath = context.Request.MapPath(imageUrl);

                        file.SaveAs(imageFilePath);

                        if (!string.IsNullOrEmpty(dlogo))
                        {
                            //对LOGO图片进行处理
                            
                            Bitmap s = new Bitmap(imageFilePath);

                            if (s.Height > 300 || s.Width > 300)
                            {
                            s = GetThumbnail(s, 300, 300);
                            s.Save(imageFilePath);
                            s.Dispose();
                            }
                            
                            
                        }
                        
                        
                        context.Response.Write(imageUrl);
                        context.Response.End();
                    }
                    else
                    {
                        //上传失败
                        context.Response.Write("0");
                        context.Response.End();
                    }
                }
            }
        }
        else
        {
            //没有选择图片 
            context.Response.Write("3");
            context.Response.End();
        }
    }
    public void BakUpload(HttpContext context)
    {
        HttpFileCollection files = context.Request.Files;
        string imageurl = "";
        if (files.Count > 0)
        {

            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFile file = files[i];

                if (file.ContentLength > 0)
                {
                    string imageUrl = string.Empty;
                    if (file != null && !string.IsNullOrEmpty(file.FileName))
                    {
                        string uploadPath = "/Storage/data/DistributorBackgroundPic";
                        string newFilename = Guid.NewGuid().ToString("N", System.Globalization.CultureInfo.InvariantCulture) +
                                             Path.GetExtension(file.FileName);
                        //当前待上传的服务端路径
                        imageUrl = uploadPath + "/" + newFilename;
                        //当前文件后缀名
                        string ext = Path.GetExtension(file.FileName).ToLower();
                        //验证文件类型是否正确
                        if (!ext.Equals(".gif") && !ext.Equals(".jpg") && !ext.Equals(".png") && !ext.Equals(".bmp"))
                        {
                            //这里window.parent.uploadSuccess()是我在前端页面中写好的javascript function,此方法主要用于输出异常和上传成功后的图片地址
                            context.Response.Write("2");
                            context.Response.End();
                        }
                        //验证文件的大小
                        if (file.ContentLength > 1048576)
                        {
                            //这里window.parent.uploadSuccess()是我在前端页面中写好的javascript function,此方法主要用于输出异常和上传成功后的图片地址 
                            context.Response.Write("1");
                            context.Response.End();
                        }
                        //开始上传
                        file.SaveAs(context.Request.MapPath(imageUrl));
                        imageurl += imageUrl + "|";
                        
                    }
                    else
                    {
                        //上传失败
                        context.Response.Write("0");
                        context.Response.End();
                    }
                }
            }
            if(!String.IsNullOrEmpty(imageurl))
            {
                context.Response.Write(imageurl);
                context.Response.End();
            }
        }
        else
        {
            //没有选择图片 
            context.Response.Write("3");
            context.Response.End();
        }
    }


    public static Bitmap GetThumbnail(Bitmap b, int destHeight, int destWidth)
    {

        System.Drawing.Image imgSource = b;
        System.Drawing.Imaging.ImageFormat thisFormat = imgSource.RawFormat;
        int sW = 0, sH = 0;

        // 按比例缩放           
        
        int sWidth = imgSource.Width;
        int sHeight = imgSource.Height;

        if (sHeight > destHeight || sWidth > destWidth)
        {

            if ((sWidth * destHeight) < (sHeight * destWidth))
            {

                sW = destWidth;

                sH = (destWidth * sHeight) / sWidth;

            }

            else
            {

                sH = destHeight;

                sW = (sWidth * destHeight) / sHeight;

            }

        }

        else
        {
            sW = destWidth; //sWidth;
            sH = destHeight;// sHeight;   
        }

        Bitmap outBmp = new Bitmap(destWidth, destHeight);

        Graphics g = Graphics.FromImage(outBmp);

        g.Clear(Color.Transparent);

        // 设置画布的描绘质量         

        g.CompositingQuality = CompositingQuality.HighQuality;

        g.SmoothingMode = SmoothingMode.HighQuality;

        g.InterpolationMode = InterpolationMode.HighQualityBicubic;

        g.DrawImage(imgSource, new Rectangle((destWidth - sW) / 2, (destHeight - sH) / 2, sW, sH), 0, 0, imgSource.Width, imgSource.Height, GraphicsUnit.Pixel);

        g.Dispose();

        // 以下代码为保存图片时，设置压缩质量     

        EncoderParameters encoderParams = new EncoderParameters();

        long[] quality = new long[1];

        quality[0] = 100;

        EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

        encoderParams.Param[0] = encoderParam;

        imgSource.Dispose();

        return outBmp;

    }
    
    
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}