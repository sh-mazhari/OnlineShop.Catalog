using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Core.Common
{
    public enum FileType
    {
        [Description(".jpg,.png,.jpeg")]
        Image = 10,
        [Description(".mp4,.avl")]
        Video = 20,
    }
}
