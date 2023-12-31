﻿using AsposeTestTask.DTO.Company.Requests;

namespace AsposeTestTask.Web.Controllers.Company.Create
{
    public class CreateCompanyRequest
    {
        public string Name { get; set; }
        public int? ParentCompanyId { get; set; }


        public CreateCompanyRequest()
        {
            
        }
        public CreateCompanyRequest(string name)
        {
            Name = name;
        }


        public CreateCompanyRequestDTO GetDTO() => new()
        {
            CompanyName = Name,
            ParentCompanyId = ParentCompanyId
        };
    }
}
