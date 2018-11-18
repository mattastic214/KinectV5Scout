using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectDataBase
{
    public abstract class Controller
    {
        protected string basePath;
        protected string specPath;

        protected int i;
        protected Random random = new Random();

        public Controller(string basePath, string specPath)
        {
            this.basePath = basePath;
            this.specPath = specPath;
        }
    }
}
