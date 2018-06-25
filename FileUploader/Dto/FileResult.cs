using System;
using System.Collections.Generic;
using System.Text;

namespace FileUploader.Dto
{
    public class FileResult: BaseModel<FileResult>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Version  { get; set; }
        public string Remark { get; set; }
    }
}
