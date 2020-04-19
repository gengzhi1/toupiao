using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace toupiao.Controllers
{
    public static class ControllerMix
    {
        public static string GetContentType(string FileNameWithExtension)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(FileNameWithExtension, out string contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }


        public static async Task<string> SaveFormFileAsync(
            IFormFile formFile,
            IWebHostEnvironment env,
            ModelStateDictionary modelState,
            String modelStateKey)
        {
            var maxFileLength =  4; // 4M
            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);

                if (memoryStream.Length < maxFileLength*Math.Pow(1024,2) )
                {
                    var fileBytes = memoryStream.ToArray();

                    var hashBytes = SHA256.Create().ComputeHash(fileBytes);
                    var hash = new System.Text.StringBuilder();

                    foreach (byte theByte in hashBytes)
                    {
                        hash.Append(theByte.ToString("x2"));
                    }
                    var fileName = hash.ToString() +"."+ 
                        formFile.FileName.Split('.')
                        .LastOrDefault() ;
                    var parentPath = env.ContentRootPath + "/Data/Files/";

                    if (!Directory.Exists(parentPath))
                    {
                        Directory.CreateDirectory(parentPath);
                    }

                    if( ! File.Exists(parentPath+fileName))
                    {
                        using var fs = new FileStream(
                            parentPath+fileName,
                            FileMode.Create, FileAccess.Write);

                        fs.Write(fileBytes, 0, fileBytes.Length);
                    }

                    return fileName;

                }
                else
                {
                    modelState.AddModelError(
                        modelStateKey, $"文件不要大于{maxFileLength}M哦!");
                    return null;
                }
            }
        }
    }
    // https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/sort-filter-page?view=aspnetcore-3.1#add-paging-to-index-method
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(
            List<T> items, 
            int count, 
            int pageIndex, 
            int pageSize,
            ViewDataDictionary viewData,
            int totalPages)
        {
            TotalPages = totalPages;
            PageIndex = pageIndex;

            viewData.Add(nameof(PageIndex), pageIndex);
            viewData.Add(nameof(TotalPages),TotalPages );
            viewData.Add(nameof(HasPreviousPage), HasPreviousPage);
            viewData.Add(nameof(HasNextPage), HasNextPage);
        
            this.AddRange(items);
        }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;


        public static async Task<PaginatedList<T>> CreateAsync(
            IQueryable<T> source, 
            int pageIndex, 
            int pageSize,
            ViewDataDictionary viewData)
        {
            var count = await source.CountAsync();

            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            // 以页数参数大于总页数的话页数参数设为总页数
            pageIndex =  pageIndex >= totalPages ?  totalPages : pageIndex ;
            
            var items = await source.Skip(     (pageIndex - 1) * pageSize  )
                .Take(pageSize).ToListAsync();

            return new PaginatedList<T>(items, count, pageIndex, 
                pageSize, viewData, totalPages);
        }
    }
}
