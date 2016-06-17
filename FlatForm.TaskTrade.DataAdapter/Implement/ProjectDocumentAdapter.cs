using Peacock.PEP.DataAdapter.Interface;
using Peacock.PEP.Service;
using Peacock.PEP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Peacock.PEP.Data.Entities;
using Peacock.PEP.Model.DTO;
using ResourceLibraryExtension.Helper;
using ResourceLibraryExtension.Untity;
using ResourceLibraryExtension.Untity.Enum;

namespace Peacock.PEP.DataAdapter.Implement
{
    public class ProjectDocumentAdapter : IProjectDocumentAdapter
    {
        public IList<ProjectDocumentModel> GetDocumentList(long onLineBusinessId)
        {
            IList<ProjectDocumentModel> documents = ProjectDocumentService.Instance.GetDocumentList(onLineBusinessId).ToListModel<ProjectDocumentModel, ProjectDocument>();
            foreach (ProjectDocumentModel item in documents)
            {
                item.OriginalImageUrl= SingleFileManager.GetFileUrl(item.ResourceId);
                item.ThumbnailUrl= SingleFileManager.GetFileUrl(item.ResourceId,ResourceImageSize.小图);
                item.Extension= SingleFileManager.GetFileFormat(item.ResourceId);
            }
            return documents;
        }
        
    }
}
