using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Infrastructure;
using Pang.AutoMapperMiddleware;
using System.Collections;
using WeChatApp.Shared.Entity;
using WeChatApp.Shared.Extensions;
using WeChatApp.Shared.FormBody;
using WeChatApp.Shared.GlobalVars;
using WeChatApp.WebApp.Extensions;
using WeChatApp.WebApp.Services;

namespace WeChatApp.WebApp.Controllers
{
    public class AppHistoryController : ApiController<AppHistory, AppHistoryDto>
    {
        private readonly IServiceGen _serviceGen;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppHistoryController(IServiceGen serviceGen,
            IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor) : base(serviceGen)
        {
            _serviceGen = serviceGen;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 获取最新版本的文件
        /// </summary>
        /// <returns> </returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetNewestVersion()
        {
            var entity = await _serviceGen.Query<AppHistory>()
                .OrderByDescending(x => x.CreateTime)
                .FirstOrDefaultAsync();

            if (entity is null)
                return Fail("没有最新的版本");

            var result = entity.MapTo<AppHistoryDto>();

            result.ConfigValue = new ConfigValue
            {
                Code = 0,
                Version = result.Version,
                Describe = result.Description ?? "",
                DownloadUrl = $"{_httpContextAccessor.HttpContext!.Request.Scheme}://{_httpContextAccessor.HttpContext!.Request.Host}/{result.Source}"
            }.ToJson();

            return Success("获取成功", result);
        }

        /// <summary>
        /// 添加一个版本
        /// </summary>
        /// <param name="dto"> </param>
        /// <returns> </returns>
        [HttpPost]
        public async Task<ActionResult> AddAppVersion(AppHistoryUploadDto dto)
        {
            var entity = new AppHistory
            {
                Remark = dto.Remark,
                Description = dto.Descrption,
                Version = dto.Version,
                Source = dto.Path
            };

            entity.Create();

            await _serviceGen.Db.AddAsync(entity);

            if (await _serviceGen.SaveAsync())
                return Success("保存成功");

            return Fail("保存失败");
        }

        /// <summary>
        /// 上传最新版本的apk
        /// </summary>
        /// <param name="files"> </param>
        /// <param name="dto">   </param>
        /// <returns> </returns>
        [HttpPost]
        public ActionResult UploadAppVersion(IFormCollection files)
        {
            try
            {
                //var form = Request.Form;//直接从表单里面获取文件名不需要参数
                string dd = files["file"];
                var form = files;//定义接收类型的参数
                Hashtable hash = new Hashtable();

                var fileName = string.Empty;

                IFormFileCollection cols = Request.Form.Files;
                if (cols.Count == 0)
                {
                    return Fail("没有上传文件", hash);
                }
                foreach (IFormFile file in cols)
                {
                    //定义图片数组后缀格式
                    string[] limitPictureType = { ".APK" };
                    //获取图片后缀是否存在数组中
                    string currentPictureExtension = Path.GetExtension(file.FileName).ToUpper();
                    if (limitPictureType.Contains(currentPictureExtension))
                    {
                        //为了查看图片就不在重新生成文件名称了
                        // var new_path = DateTime.Now.ToString("yyyyMMdd")+ file.FileName;

                        fileName = $"{Guid.NewGuid()}-{file.FileName}";

                        var rootPath = $"{_webHostEnvironment.WebRootPath}/{GlobalVars.ApksPath}";

                        DirectoryInfo di = new DirectoryInfo(rootPath);
                        if (!di.Exists)
                        {
                            di.Create();
                        }

                        var filePath = $"{rootPath}/{fileName}";

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            //再把文件保存的文件夹中
                            file.CopyTo(stream);
                            hash.Add("file", "/" + filePath);
                        }
                    }
                    else
                    {
                        return Fail("请上传指定格式的文件", hash);
                    }
                }

                return Success("上传成功", new
                {
                    hash,
                    path = $"{GlobalVars.ApksPath}{fileName}"
                });
            }
            catch (Exception ex)
            {
                return Fail("上传失败", ex.Message);
            }
        }
    }
}