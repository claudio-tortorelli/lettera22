using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lettera22.Compiler
{
    class ResourcesManager
    {
        /// <project>Lettera22</project>
        /// <copyright company="Claudio Tortorelli">
        /// Copyright (c) 2018 All Rights Reserved
        /// </copyright>        
        /// <author>Claudio Tortorelli</author>
        /// <email>claudio.tortorelli@gmail.com</email>
        /// <web>http://www.claudiotortorelli.it</web>
        /// <date>Oct 2018</date>
        /// <summary>
        /// </summary>
        /// https://choosealicense.com/licenses/mit/

        private List<ExternalResource> resources = new List<ExternalResource>();

        public void AddRes(ExternalResource res) 
        {
            // avoid duplicates
            for (int i = 0; i < resources.Count; i++)
            {
                if (resources[i].GetFileName().Equals(res.GetFileName()))
                {                    
                    return;
                }
            }
            resources.Add(res);
        }

        public ExternalResource GetRes(string fileName) 
        {
            for (int iRes = 0; iRes < resources.Count; iRes++)
            {
                if (resources[iRes].GetFileName().Equals(fileName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return resources[iRes];
                }
            }
            return null;
        }



    }
}
