using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeChatApp.Shared.RequestBody.WebApi;
using WeChatApp.Shared.FormBody;

namespace WeChatApp.AdminClient.Services
{
    public interface IDepartmentService
    {
        Task<T> GetDepartmentListAsync<T>(ParameterBase parameter);

        Task<T> GetDepartmentAsync<T>(Guid departmentId);

        Task<T> AddDepartmentAsync<T>(DepartmentDto dto);

        Task<T> EditDepartmentAsync<T>(DepartmentDto dto);

        Task<T> DeleteDepartmentAsync<T>(DepartmentDto dto);

        Task<T> GetDepartmentTreeAsync<T>();
    }
}