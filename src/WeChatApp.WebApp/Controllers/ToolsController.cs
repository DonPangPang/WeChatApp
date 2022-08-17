using Microsoft.AspNetCore.Mvc;
using System.Collections;
using WeChatApp.Shared.GlobalVars;
using WeChatApp.WebApp.Services;

namespace WeChatApp.WebApp.Controllers
{
    public class ToolsController : ApiController
    {
        private readonly IServiceGen _serviceGen;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ToolsController(IServiceGen serviceGen, IWebHostEnvironment webHostEnvironment)
        {
            _serviceGen = serviceGen;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public async Task<ActionResult> Upload(IFormCollection files)
        {
            try
            {
                //var form = Request.Form;//直接从表单里面获取文件名不需要参数
                string dd = files["file"];
                var form = files;//定义接收类型的参数
                Hashtable hash = new Hashtable();
                IFormFileCollection cols = Request.Form.Files;
                if (cols.Count == 0)
                {
                    return Fail("没有上传文件", hash);
                }
                foreach (IFormFile file in cols)
                {
                    //定义图片数组后缀格式
                    string[] limitPictureType = { ".JPG", ".JPEG", ".GIF", ".PNG", ".BMP" };
                    //获取图片后缀是否存在数组中
                    string currentPictureExtension = Path.GetExtension(file.FileName).ToUpper();
                    if (limitPictureType.Contains(currentPictureExtension))
                    {
                        //为了查看图片就不在重新生成文件名称了
                        // var new_path = DateTime.Now.ToString("yyyyMMdd")+ file.FileName;
                        var newPath = Path.Combine(GlobalVars.ImagesPath, $"{Guid.NewGuid()}-{file.FileName}");
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", newPath);

                        Console.WriteLine(path);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            //再把文件保存的文件夹中
                            file.CopyTo(stream);
                            hash.Add("file", "/" + newPath);
                        }
                    }
                    else
                    {
                        return Fail("请上传指定格式的图片", hash);
                    }
                }

                return Success("上传成功", hash);
            }
            catch (Exception ex)
            {
                return Fail("上传失败", ex.Message);
            }
        }
    }
}