using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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

    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(
            List<T> items, 
            int count, 
            int pageIndex, 
            int pageSize,
            ViewDataDictionary viewData)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            
            viewData.Add(nameof(PageIndex), pageIndex);
            viewData.Add(nameof(TotalPages),TotalPages );
            viewData.Add(nameof(HasPreviousPage), HasNextPage);
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
            var items = await source.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize).ToListAsync();
            
            return new PaginatedList<T>(items, count, pageIndex, pageSize, viewData);
        }
    }
}
